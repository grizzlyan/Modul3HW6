using System.IO;

namespace Modul3HW6.Services
{
    public class DirectoryService : IDirectoryService
    {
        public void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }

        public void DeleteDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }

        public string[] GetFiles(string path) => Directory.GetFiles(path);

        public bool Exists(string path) => Directory.Exists(path);
    }
}
