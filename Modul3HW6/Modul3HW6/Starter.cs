using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Modul3HW6.Configs;
using Modul3HW6.Helpers;
using Modul3HW6.Services;

namespace Modul3HW6
{
    public class Starter
    {
        private readonly IActions _actions;
        private readonly IAsyncLoggerService _logger;
        private static readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1);
        public Starter(
            IActions actions,
            IAsyncLoggerService logger)
        {
            _logger = logger;
            _actions = actions;
            _logger.IsBackUpCount += CompareNumbers;
        }

        public void Run()
        {
            Task.WaitAll(new[] { Task.Run(() => RunAsync("Method1")), Task.Run(() => RunAsync("Method2")) });
        }

        private async Task RunAsync(string methodNumber)
        {
            var rnd = new Random();
            var maxValue = 3;

            for (var i = 0; i < 50; i++)
            {
                try
                {
                    switch (rnd.Next(maxValue))
                    {
                        case 0:
                            await _actions.InfoMethod(methodNumber);
                            break;
                        case 1:
                            _actions.WarningMethod(methodNumber);
                            break;
                        case 2:
                            _actions.ErrorMethod(methodNumber);
                            break;
                    }
                }
                catch (BusinessException ex)
                {
                    await _logger.LogWarning($"Action got this custom Exception : {ex.Message}");
                }
                catch (Exception ex)
                {
                    await _logger.LogError($"Action failed by reason: {ex.Message}");
                }
            }
        }

        private bool CompareNumbers(int count, int backUpCount)
        {
            return count == backUpCount;
        }
    }
}
