using System;
using System.Threading.Tasks;

namespace Modul3HW6.Services
{
    public interface IAsyncFileService
    {
        IDisposable CreateStreamForWrite(string path);
        IDisposable CreateStreamForRead(string path);
        Task WriteToStreamAsync(IDisposable stream, string text);
        Task<string> ReadAllTextAsync(IDisposable stream);
        void Delete(string path);
    }
}
