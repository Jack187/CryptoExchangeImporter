using Microsoft.VisualStudio.TestTools.UnitTesting;
using RichardSzalay.MockHttp;
using System;
using System.Configuration;
using System.Net.Http;

namespace BitfinexApi.Test
{
    [TestClass]
    public class Test
    {
        private static string ApiKey;
        private static string SecretKey;

        [ClassInitialize]
        public static void Initialize(TestContext t)
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
                throw new Exception($"Missing BfApiKey");

            if (string.IsNullOrEmpty(SecretKey))
                throw new Exception("Missing BfApiSecret");
        }

        [TestMethod]
        public void TestMethod1()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("*v2/platform/status")
                .Respond("application/json", "[1]"); // Respond with JSON

            var bfClient = new BitfinexApiClient(ApiKey, SecretKey, new HttpClient(mockHttp));
            var platformStatus = bfClient.GetPlatformStatusAsync().Result;

            Assert.AreEqual(platformStatus.Operative, 1);
        }

        [TestMethod]
        public void TestMethod2()
        {
            var bfClient = new BitfinexApiClient(ApiKey, SecretKey, new HttpClient());

            var wallets = bfClient.GetWalletsAsync().Result;

            Assert.AreEqual(wallets.Count, 6);
        }
    }
}