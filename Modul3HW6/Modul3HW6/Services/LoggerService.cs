using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Modul3HW6.Configs;
using Modul3HW6.Enums;

namespace Modul3HW6.Services
{
    public class LoggerService : IAsyncLoggerService
    {
        private readonly Config _config;
        private readonly SemaphoreSlim _semaphoreSlim;
        private readonly DirectoryService _directoryService;
        private readonly FileService _fileService;
        private IDisposable _fileStreamWriter;
        private int _counter = 1;
        private string _previousLog = string.Empty;
        private string _filePath;

        public LoggerService()
        {
            _semaphoreSlim = new SemaphoreSlim(1);
            _config = new Config();
            _directoryService = new DirectoryService();
            _fileService = new FileService();
            _directoryService.DeleteDirectory(_config.Logger.DirectoryPath);
            Init();
        }

        public event Func<int, int, bool> IsBackUpCount;

        public async Task LogInfo(string message)
        {
            await Log(LogType.Info, message);
        }

        public async Task LogError(string message)
        {
            await Log(LogType.Error, message);
        }

        public async Task LogWarning(string message)
        {
            await Log(LogType.Warning, message);
        }

        public async Task Log(LogType logType, string message)
        {
            await _semaphoreSlim.WaitAsync();

            try
            {
                if (!string.IsNullOrEmpty(_previousLog))
                {
                    await _fileService.WriteToStreamAsync(_fileStreamWriter, _previousLog);
                }

                while (!IsBackUpCount.Invoke(_counter, _config.Logger.BackUpCount))
                {
                    var log = $"{DateTime.UtcNow}:{logType}:{message}";

                    await _fileService.WriteToStreamAsync(_fileStreamWriter, log);
                    _counter++;
                }

                _previousLog = await _fileService.ReadAllTextAsync(_filePath);
                Init();
                _counter = 1;
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        private void Init()
        {
            var directoryPath = _config.Logger.DirectoryPath;
            _directoryService.CreateDirectory(directoryPath);

            var fileName = $"{DateTime.UtcNow.ToString(_config.Logger.NameFormat)}";
            _filePath = $"{directoryPath}{fileName}{_config.Logger.ExtensionFile}";

            _fileStreamWriter = _fileService.CreateStreamForWrite(_filePath);
        }
    }
}
