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
        private readonly IConfigService _config;
        private readonly SemaphoreSlim _semaphoreSlim;
        private readonly IDirectoryService _directoryService;
        private readonly IAsyncFileService _fileService;
        private IDisposable _fileStreamWriter;
        private IDisposable _fileStreamReader;
        private int _counter = 0;
        private string _previousLog = string.Empty;
        private string _filePath;

        public LoggerService(
            IConfigService config,
            IDirectoryService directoryService,
            IAsyncFileService fileService)
        {
            _semaphoreSlim = new SemaphoreSlim(1);
            _config = config;
            _directoryService = directoryService;
            _fileService = fileService;
            _directoryService.DeleteDirectory(_config.LoggerConfig.DirectoryPath);
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

            if (_counter == 0 && !string.IsNullOrEmpty(_previousLog))
            {
                await _fileService.WriteToStreamAsync(_fileStreamWriter, _previousLog);
            }

            if (!IsBackUpCount.Invoke(_counter, _config.LoggerConfig.BackUpCount))
            {
                var log = $"{DateTime.UtcNow}:{logType}:{message}";

                await _fileService.WriteToStreamAsync(_fileStreamWriter, log);
                _counter++;
            }
            else
            {
                _fileStreamWriter.Dispose();
                _fileStreamReader = _fileService.CreateStreamForRead(_filePath);
                _previousLog = await _fileService.ReadAllTextAsync(_fileStreamReader);
                _fileStreamReader.Dispose();
                Init();
                _counter = 0;
            }

            _semaphoreSlim.Release();
        }

        private void Init()
        {
            var directoryPath = _config.LoggerConfig.DirectoryPath;
            _directoryService.CreateDirectory(directoryPath);

            var fileName = $"{DateTime.UtcNow.ToString(_config.LoggerConfig.NameFormat)}";
            _filePath = $"{directoryPath}{fileName}{_config.LoggerConfig.ExtensionFile}";

            _fileStreamWriter = _fileService.CreateStreamForWrite(_filePath);
        }
    }
}
