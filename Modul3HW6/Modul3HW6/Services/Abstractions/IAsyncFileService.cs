using System;
using System.Threading.Tasks;

namespace Modul3HW6.Services
{
    public interface IAsyncFileService
    {
        IDisposable CreateStreamForWrite(string path);
        Task WriteToStreamAsync(IDisposable stream, string text);
        Task<string> ReadAllTextAsync(string path);
        void Delete(string path);
    }
}
