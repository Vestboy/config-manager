using NLog;
using ObjectsInterfacesLibrary;
using ObjectsInterfacesLibrary.Objects;
using System;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigManager
{
    public class FileReader
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IFileSystem _fileSystem;
        private readonly string _baseFolderPath;

        public FileReader() : this(new FileSystem())
        {
            
        }
        
        public FileReader(IFileSystem fileSystem)
        {
            _baseFolderPath = CommonUtils.GetBaseFolderPath();
            _fileSystem = fileSystem;
        }


        public async Task<IMethodResult> GetConfigFile(string folderName, string fileName)
        {
            Logger.Info("ReadConfigFile Start");

            if (_baseFolderPath == null)
            {
                string message = "baseFolderPath doesn't exist";
                Logger.Error(message);
                return new MethodResult(false, message);
            }

            string targetDirPath = Path.Combine(_baseFolderPath, folderName);

            if (!_fileSystem.Directory.Exists(targetDirPath))
            {
                string message = "target folder doesn't exist";
                Logger.Error(message);
                return new MethodResult(false, message);
            }

            string targetFilePath = Path.Combine(targetDirPath, fileName);

            if (_fileSystem.File.Exists(targetFilePath) == false)
            {
                string message = "target file doesn't exist";
                Logger.Error(message);
                return new MethodResult(false, message);
            }
            else
            {
                try
                {
                    using (var reader = _fileSystem.File.OpenText(targetFilePath))
                    {
                        var fileText = await reader.ReadToEndAsync();
                        return new MethodResult(true, null, fileText);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.Message);
                    return new MethodResult(false, ex.Message);
                }
            }
        }

        public async Task<IMethodResult> GetConfigFilesList(string folderName)
        {
            Logger.Info("ReadConfigFile Start");

            if (_baseFolderPath == null)
            {
                string message = "baseFolderPath doesn't exist";
                Logger.Error(message);
                return new MethodResult(false, message);
            }

            string targetDirPath = Path.Combine(_baseFolderPath, folderName);

            if (!_fileSystem.Directory.Exists(targetDirPath))
            {
                string message = "target folder doesn't exist";
                Logger.Error(message);
                return new MethodResult(false, message);
            }

            try
            {
                string[] fileNames = null;
                await Task.Run(() =>
                {
                    string[] filesPaths = _fileSystem.Directory.GetFiles(targetDirPath);
                    fileNames = filesPaths.Select(f => Path.GetFileName(f)).ToArray();
                });

                return new MethodResult(true, null, fileNames);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                return new MethodResult(false, ex.Message);
            }

        }

        //static async Task<string> ReadTextAsync(string filePath)
        //{
        //    using (var reader = File.OpenText(filePath))
        //    {
        //        var fileText = await reader.ReadToEndAsync();
        //        return fileText;
        //    }
        //}
    }
}
