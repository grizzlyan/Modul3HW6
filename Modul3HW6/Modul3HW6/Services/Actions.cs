using System;
using System.Threading.Tasks;
using Modul3HW6.Helpers;

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

        public async Task<bool> InfoMethod(string methodNumber)
        {
            await _logger.LogInfo($"Start method: {nameof(InfoMethod)}. From starter method - {methodNumber}.");
            return true;
        }

        public bool WarningMethod(string methodNumber)
        {
            throw new BusinessException($"Skipped logic in method. From starter method - {methodNumber}.");
        }

        public bool ErrorMethod(string methodNumber)
        {
            throw new Exception($"I broke a logic. From starter method - {methodNumber}.");
        }
    }
}
