namespace Modul3HW6.Services
{
    public interface IDirectoryService
    {
        void CreateDirectory(string path);
        void DeleteDirectory(string path);
        string[] GetFiles(string path);
        bool Exists(string path);
    }
}
