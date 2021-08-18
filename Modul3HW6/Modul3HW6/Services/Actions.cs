using System;
using Modul3HW6.Helpers;
using Modul3HW6.Services.Abstractions;

namespace Modul3HW6.Services
{
    public class Actions : IActions
    {
        private readonly IAsyncLoggerService _logger;

        public Actions(
            IAsyncLoggerService logger)
        {
            _logger = logger;
        }

        public bool InfoMethod()
        {
            _logger.LogInfo($"Start method: {nameof(InfoMethod)}");
            return true;
        }

        public bool WarningMethod()
        {
            throw new BusinessException("Skipped logic in method");
        }

        public bool ErrorMethod()
        {
            throw new Exception("I broke a logic");
        }
    }
}
