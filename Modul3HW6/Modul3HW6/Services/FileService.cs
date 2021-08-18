﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Modul3HW6.Configs;

namespace Modul3HW6.Services
{
    public class FileService : IAsyncFileService
    {
        public IDisposable CreateStreamForWrite(string path)
        {
            return new StreamWriter(path, true, Encoding.Default);
        }

        public async Task WriteToStreamAsync(IDisposable stream, string text)
        {
            var streamWriter = (StreamWriter)stream;
            await streamWriter.WriteLineAsync(text);
        }

        public async Task<string> ReadAllTextAsync(string path)
        {
           return await File.ReadAllTextAsync(path);
        }

        public void Delete(string path)
        {
            File.Delete(path);
        }
    }
}
