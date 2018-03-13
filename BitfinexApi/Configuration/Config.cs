using System;
using System.Configuration;

namespace BitfinexApi.Configuration
{
    // TODO: probably not the final solution
    public static class Config
    {
        public static string ApiKey;
        public static string SecretKey;

        static Config()
        {
            // TODO: find a proper way for handling configuration/settings later
            var pathToConfig = AppDomain.CurrentDomain.BaseDirectory + "BitfinexApi.dll";
            var config = ConfigurationManager.OpenExeConfiguration(pathToConfig);
            var appSettings = config.AppSettings;
            // Log config file not found - no auth access possible just public endpoints will work
            ApiKey = appSettings.Settings["BfApiKey"].Value;
            SecretKey = appSettings.Settings["BfApiSecret"].Value;
            // log if values are isNullOrStringEmpty - just public endpoints

            if (string.IsNullOrEmpty(ApiKey))
                throw new Exception($"Missing BfApiKey in config.");

            if (string.IsNullOrEmpty(SecretKey))
                throw new Exception("Missing BfApiSecretin config.");
        }
    }
}