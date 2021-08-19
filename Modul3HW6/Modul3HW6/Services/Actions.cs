using System;
using System.Threading.Tasks;
using Modul3HW6.Helpers;

namespace Modul3HW6.Services
{
    public class Actions : IAsyncActions
    {
        private readonly IAsyncLoggerService _logger;

        public Actions(
            IAsyncLoggerService logger)
        {
            _logger = logger;
        }

        public async Task<bool> InfoMethodAsync(string methodNumber)
        {
            await _logger.LogInfo($"Start method: {nameof(InfoMethodAsync)}. From Starter Class, Method - {methodNumber}.");
            return true;
        }

        public bool WarningMethod(string methodNumber)
        {
            throw new BusinessException($"Skipped logic in method. From Starter Class, Method - {methodNumber}.");
        }

        public bool ErrorMethod(string methodNumber)
        {
            throw new Exception($"I broke a logic. From Starter Class, Method - {methodNumber}.");
        }
    }
}
