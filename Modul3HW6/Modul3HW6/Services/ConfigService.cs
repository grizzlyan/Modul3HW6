using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modul3HW6.Configs;
using Newtonsoft.Json;

namespace Modul3HW6.Services
{
    public class ConfigService : IConfigService
    {
        private readonly string _filePath = "config.json";
        private readonly LoggerConfig _loggerConfig;

        public ConfigService()
        {
            var config = GetConfig();
            _loggerConfig = config.Logger;
        }

        public LoggerConfig LoggerConfig => _loggerConfig;

        private Config GetConfig()
        {
            var configFile = File.ReadAllText(_filePath);
            return JsonConvert.DeserializeObject<Config>(configFile);
        }
    }
}
