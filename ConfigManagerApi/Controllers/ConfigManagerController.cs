using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Abstractions;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ObjectsInterfacesLibrary;
using ObjectsInterfacesLibrary.Objects.ConfigObjects;

namespace ConfigManager.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ConfigManagerController : ControllerBase
    {

        private readonly ILogger<ConfigManagerController> _logger;

        private readonly IConfiguration _appConfig;
        
        public ConfigManagerController(ILogger<ConfigManagerController> logger, IConfiguration configRoot)
        {
            _logger = logger;
            _appConfig = configRoot;
            
        }

        /// <summary>
        /// Shows if service is started.
        /// </summary>
        /// <response code="200">Returns service status</response>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Get()
        {
            return Ok("service running");
        }

        /// <summary>
        /// Returns config by given folder and file name.
        /// </summary>
        /// <param name="folderName"></param>   
        /// <param name="fileName"></param>   
        /// <response code="200">Returns config item</response>
        /// <response code="400">If input folder or file not found or null</response>
        /// <response code="401">if unauthorized</response>
        
        [HttpGet("{folderName}/{fileName}")]
        public async Task<ActionResult<object>> GetConfig(string folderName, string fileName)
        {
           
            #region INPUT CHECKS

            if (folderName == null)
            {
                string message = "folderName is empty";
                _logger.LogError(message);
                return BadRequest(message);
            }

            if (fileName == null)
            {
                string message = "fileName is empty";
                _logger.LogError(message);
                return BadRequest(message);
            }

            folderName = folderName.Trim();
            fileName = fileName.Trim();

            #endregion

            ConfigManager.FileReader fileReader = new FileReader();

            IMethodResult result = await fileReader.GetConfigFile(folderName, fileName);

            if (result.IsSuccessful)
            {
                return result.Result;
            }
            else
            {
                return BadRequest(result.Message);
            }
        }

        /// <summary>
        /// Returns list of config files by given folder name.
        /// </summary>
        /// <param name="folderName"></param>
        /// <response code="200">Returns list of config items in given folder</response>
        /// <response code="400">If input folder not found or null</response>
        /// <response code="401">if unauthorized</response>
        [Produces("application/json")]
        [HttpGet("{folderName}")]
        public async Task<object> GetConfigList(string folderName)
        {
            _logger.LogInformation("GetConfigList()");

            #region INPUT CHECKS

            if (folderName == null)
            {
                string message = "folderName is empty";
                _logger.LogError(message);
                return BadRequest(message);
            }

            folderName = folderName.Trim();

            #endregion

            ConfigManager.FileReader fileReader = new FileReader();

            IMethodResult result = await fileReader.GetConfigFilesList(folderName);

            if (result.IsSuccessful)
            {
                return result.Result;
            }
            else
            {
                return BadRequest(result.Message);
            }

        }

        /// <summary>
        /// Creates or replaces file content.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /Todo
        ///     {
        ///         "Dsn": "Dsn1",
        ///         "User": "User1",
        ///         "Password": "Password1",
        ///         "Colors": [
        ///             "White",
        ///             "100"
        ///         ],
        ///         "Stat": {
        ///             "Health": 100,
        ///             "Vitality": 100
        ///          }
        ///      }
        ///
        /// </remarks>
        /// <param name="folderName"></param>   
        /// <param name="fileName"></param>
        /// <param name="fileContent"></param>
        /// <response code="200">If file was created or updated successfully</response>
        /// <response code="400">If input folder or file not found or null, if file content is empty or not of known type</response>
        /// <response code="401">if unauthorized</response>
        [HttpPut("{folderName}/{fileName}")]
        public async Task<ActionResult> WriteConfig([FromRoute()] string folderName, [FromRoute()] string fileName, [FromBody()] object fileContent)
        {
            _logger.LogInformation("API WriteConfig()");

            #region INPUT CHECKS

            if (fileContent == null)
            {
                string message = "fileContent is empty";
                _logger.LogError(message);
                return BadRequest(message);
            }

            if (folderName == null)
            {
                string message = "folderName is empty";
                _logger.LogError(message);
                return BadRequest(message);
            }

            if (fileName == null)
            {
                string message = "fileName is empty";
                _logger.LogError(message);
                return BadRequest(message);
            }

            folderName = folderName.Trim();
            fileName = fileName.Trim();

            #endregion

            //check file content type

            object configInstance = null;

            try
            {
                ConfigOne config = JsonConvert.DeserializeObject<ConfigOne>(fileContent.ToString());
                configInstance = config;
            }
            catch (Exception e)
            {
                _logger.LogInformation("Input object is not of type ConfigOne");
            }

            if (configInstance == null)
            {
                try
                {
                    ConfigTwo config = JsonConvert.DeserializeObject<ConfigTwo>(fileContent.ToString());
                    configInstance = config;
                }
                catch (Exception e)
                {
                    _logger.LogInformation("Input object is not of type ConfigTwo");
                }
            }

            if (configInstance == null)
            {
                string message = "Input object is not of known type";
                _logger.LogError(message);
                return BadRequest(message);
            }

            ConfigManager.FileWriter fileWriter = new FileWriter();

            IMethodResult result = await fileWriter.WriteConfigFile(folderName, fileName, JsonConvert.SerializeObject(configInstance));

            if (result.IsSuccessful)
            {
                return Ok("Config file written successfully");
            } else
            {
                return BadRequest(result.Message);
            }
        }

    }
}
