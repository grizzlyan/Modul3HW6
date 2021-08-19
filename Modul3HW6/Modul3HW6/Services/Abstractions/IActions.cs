using System.Threading.Tasks;

namespace Modul3HW6.Services
{
    public interface IActions
    {
        Task<bool> InfoMethod(string methodNumber);
        bool WarningMethod(string methodNumber);
        bool ErrorMethod(string methodNumber);
    }
}
