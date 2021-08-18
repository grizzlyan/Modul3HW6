using System;
using System.Threading.Tasks;
using Modul3HW6.Enums;

namespace Modul3HW6.Services
{
    public interface IAsyncLoggerService
    {
        event Func<int, int, bool> IsBackUpCount;
        Task LogInfo(string message);
        Task LogError(string message);
        Task LogWarning(string message);
        Task Log(LogType type, string message);
    }
}
