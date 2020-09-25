
using ConfigManager;
using ConfigManager.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using ObjectsInterfacesLibrary;
using ObjectsInterfacesLibrary.Objects.ConfigObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using System.Threading.Tasks;
using static ConfigManager.Controllers.ConfigManagerController;

namespace ConfigManagerTests
{
    public class ControllerTest
    {
        string baseFolderPath = null;
        ConfigManagerController controller;

        [SetUp]
        public void Setup()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));

            IConfigurationRoot root = builder.Build();

            ILogger<ConfigManagerController> logger = Mock.Of<ILogger<ConfigManagerController>>();

            controller = new ConfigManagerController(logger, root);

            IConfigurationSection appSection = root.GetSection("AppSettings");

            baseFolderPath = appSection.GetValue<string>("BaseFolder");
        }

        [Test]
        public async Task Get_ReturnsString_WithFileContent()
        {
           
            string folderName = "folderName";
            string fileName = "configOne.json";
            string fileContent = "some data";

            var mockFileSystem = new MockFileSystem();

            var mockInputFile = new MockFileData(fileContent);

            mockFileSystem.AddFile(Path.Combine(baseFolderPath, folderName, fileName), mockInputFile);

            FileReader reader = new FileReader(mockFileSystem);
            IMethodResult result = await reader.GetConfigFile(folderName, fileName);

            Assert.IsInstanceOf(fileContent.GetType(), result.Result);

            Assert.AreEqual(result.Result, fileContent);

        }

        [Test]
        public async Task Get_ReturnsArrayOfStrings_WithListOfFileNames()
        {
            
            string folderName = "folderName";
            string fileName1 = "configOne1.json";
            string fileName2 = "configOne2.json";

            string[] resultArray = { fileName1, fileName2 };
            
            var mockFileSystem = new MockFileSystem();

            var mockInputFile = new MockFileData("");

            mockFileSystem.AddFile(Path.Combine(baseFolderPath, folderName, fileName1), mockInputFile);
            mockFileSystem.AddFile(Path.Combine(baseFolderPath, folderName, fileName2), mockInputFile);

            FileReader reader = new FileReader(mockFileSystem);
            IMethodResult result = await reader.GetConfigFilesList(folderName);

            Assert.IsInstanceOf<Array>(result.Result);

            Assert.AreEqual(resultArray, result.Result);

        }

        [Test]
        public async Task Put_WritesContentsToFile_ReturnsOkResult()
        {

            string folderName = "folderName";
            string fileName = "configOne.json";

            ConfigOne testConfigOne = new ConfigOne();
            testConfigOne.Dsn = "Dsn1";
            testConfigOne.User = "User1";
            testConfigOne.Password = "Password1";
            testConfigOne.Colors = new List<string>();
            testConfigOne.Colors.Add("White");
            testConfigOne.Colors.Add("Green");
            testConfigOne.Stat = new Statistic();
            testConfigOne.Stat.Health = 100;
            testConfigOne.Stat.Vitality = 100;

            string output = JsonConvert.SerializeObject(testConfigOne);

            ActionResult<object> result = await controller.WriteConfig(folderName, fileName, output);

            Assert.IsInstanceOf<OkObjectResult>(result.Result);

            string folderPath = Path.Combine(baseFolderPath, folderName);
            string filePath = Path.Combine(baseFolderPath, folderName, fileName);
            string fileText;
            using (var reader = File.OpenText(filePath))
            {
                fileText = await reader.ReadToEndAsync();  
            }

            File.Delete(filePath);
            Directory.Delete(folderPath);

            Assert.AreEqual(fileText, output);

        }
                
    }
}