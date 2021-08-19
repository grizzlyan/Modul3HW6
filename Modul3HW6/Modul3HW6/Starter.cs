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
        private const int MaxValue = 3;
        private readonly IActions _actions;
        private readonly IAsyncLoggerService _logger;
        private readonly Random _rnd = new Random();

        public Starter(
            IActions actions,
            IAsyncLoggerService logger)
        {
            _logger = logger;
            _actions = actions;
            _logger.IsBackUpCount += CompareNumbers;
        }

        public async Task Run()
        {
            await Task.WhenAll(new[] { Task.Run(() => RunAsync("Method1")), Task.Run(() => RunAsync("Method2")) });
        }

        private async Task RunAsync(string methodNumber)
        {
            for (var i = 0; i < 50; i++)
            {
                try
                {
                    switch (_rnd.Next(MaxValue))
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
