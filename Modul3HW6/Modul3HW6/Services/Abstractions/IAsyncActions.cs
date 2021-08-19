using System.Threading.Tasks;

namespace Modul3HW6.Services
{
    public interface IAsyncActions
    {
        Task<bool> InfoMethodAsync(string methodNumber);
        bool WarningMethod(string methodNumber);
        bool ErrorMethod(string methodNumber);
    }
}
