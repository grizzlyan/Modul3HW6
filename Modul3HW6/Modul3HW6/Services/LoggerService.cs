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
        private IDisposable _backupWriter;
        private IDisposable _logsWriter;
        private string _logFileName;
        private int _counter = 0;

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

            var log = $"{DateTime.UtcNow.ToString(_config.LoggerConfig.NameFormat)}: {logType}: {message}";

            await _fileService.WriteToStreamAsync(_logsWriter, log);

            _counter++;

            if (IsBackUpCount.Invoke(_counter, _config.LoggerConfig.BackUpCount))
            {
                await PrintBackup();
            }

            _semaphoreSlim.Release();
        }

        private async Task PrintBackup()
        {
            _logsWriter.Dispose();
            var log = await _fileService.ReadAllTextAsync(_logFileName);
            var fileName = $"{DateTime.UtcNow.ToString(_config.LoggerConfig.NameFormat)}";
            _filePath = $"{_config.LoggerConfig.DirectoryPath}{fileName}{_config.LoggerConfig.ExtensionFile}";
            _backupWriter = _fileService.CreateStreamForWrite(_filePath);
            await _fileService.WriteToStreamAsync(_backupWriter, log);
            _counter = 0;
            CreateLogWriter();
        }

        private void Init()
        {
            _logFileName = $"{_config.LoggerConfig.LogFileName}{_config.LoggerConfig.ExtensionFile}";
            _fileService.Delete(_logFileName);

            var directoryPath = _config.LoggerConfig.DirectoryPath;
            if (_directoryService.Exists(directoryPath))
            {
                _directoryService.DeleteDirectory(directoryPath);
            }

            _directoryService.CreateDirectory(directoryPath);
            CreateLogWriter();
        }

        private void CreateLogWriter()
        {
            _logsWriter = _fileService.CreateStreamForWrite(_logFileName);
        }
    }
}
