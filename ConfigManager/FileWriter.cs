using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using NLog;
using System.Threading.Tasks;
using ObjectsInterfacesLibrary;
using ObjectsInterfacesLibrary.Objects;
using System.IO.Abstractions;
using System.Diagnostics;

namespace ConfigManager
{
    public class FileWriter
    {
        
        private readonly string baseFolderPath;
        private readonly IFileSystem _fileSystem;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public FileWriter() : this(new FileSystem())
        {

        }
        public FileWriter(IFileSystem fileSystem)
        {
            baseFolderPath = CommonUtils.GetBaseFolderPath();
            _fileSystem = fileSystem;
        }

        public async Task<IMethodResult> WriteConfigFile(string folderName, string fileName, string fileContent)
        {
            Logger.Info("WriteConfigFile Start");

            if (baseFolderPath == null)
            {
                Logger.Error("baseFolderPath == null");
                return new MethodResult(false, "baseFolderPath == null");
            }

            string targetDirPath = Path.Combine(baseFolderPath, folderName);

            try
            {
                var baseDir = _fileSystem.Directory.CreateDirectory(targetDirPath);
                Debug.WriteLine(baseDir.FullName);
            } catch (Exception e)
            {
                Logger.Error(e);
                return new MethodResult(false, e.Message);
            }

            string targetFilePath = Path.Combine(targetDirPath, fileName);

            if (_fileSystem.File.Exists(targetFilePath))
            {
                try
                {
                    _fileSystem.File.Delete(targetFilePath);
                } catch(Exception e)
                {
                    Logger.Error(e);
                    return new MethodResult(false, e.Message);
                }
            }
                       
            try
            {
                //_fileSystem.File.Create(targetFilePath);
                await _fileSystem.File.AppendAllTextAsync(targetFilePath, fileContent);

            }
            catch (Exception e)
            {
                Logger.Error(e);
                return new MethodResult(false, e.Message);
            }

            return new MethodResult(true, null);

        }

    }
}
