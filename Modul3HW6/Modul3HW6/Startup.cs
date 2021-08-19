using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Modul3HW6.Services;

namespace Modul3HW6
{
    public class Startup
    {
        public async Task Run()
        {
            var serviceProvider = new ServiceCollection()
                .AddTransient<IActions, Actions>()
                .AddTransient<IConfigService, ConfigService>()
                .AddTransient<IDirectoryService, DirectoryService>()
                .AddTransient<IAsyncFileService, FileService>()
                .AddSingleton<IAsyncLoggerService, LoggerService>()
                .AddTransient<Starter>()
                .BuildServiceProvider();

            var start = serviceProvider.GetService<Starter>();
            await start.Run();
        }
    }
}
