using Microsoft.VisualStudio.TestTools.UnitTesting;
using RichardSzalay.MockHttp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Text;

namespace BitfinexApi.Test
{
    [TestClass]
    public class WalletsTests
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
                throw new Exception($"Missing BfApiKey in config.");

            if (string.IsNullOrEmpty(SecretKey))
                throw new Exception("Missing BfApiSecretin config.");
        }
        
        [TestMethod]
        public void TestMethod()
        {
            // arrange
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When($"*{Endpoints.Wallets}")
                .Respond(MediaTypes.ApplicationJson, "[1]");

            // act
            var bfClient = new BitfinexApiClient(ApiKey, SecretKey, new HttpClient());
            var wallets = bfClient.GetWalletsAsync().Result;

            // assert
            Assert.AreEqual(wallets.Count, 6);
        }
    }
}
