using BitfinexApi.Test.Mocking;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RichardSzalay.MockHttp;
using System;
using System.Configuration;
using System.Net.Http;

namespace BitfinexApi.Test
{
    [TestClass]
    public class PlatformStatusTests
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
        public void When_ApiReturnsPlatformWorking_OperativeEqualsOne()
        {            
            // arrange
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When($"*{Endpoints.PlatformStatus}")
                .Respond(MediaTypes.ApplicationJson, "[1]");

            // act
            var bfClient = new BitfinexApiClient(ApiKey, SecretKey, new HttpClient(mockHttp));
            var platformStatus = bfClient.GetPlatformStatusAsync().Result;

            // assert
            Assert.AreEqual(platformStatus.Operative, 1);
        }

        [TestMethod]
        public void When_ApiReturnsPlatformNotWorking_OperativeEqualsZero()
        {
            // arrange
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When($"*{Endpoints.PlatformStatus}")
                .Respond(MediaTypes.ApplicationJson, "[0]");

            // act
            var bfClient = new BitfinexApiClient(ApiKey, SecretKey, new HttpClient(mockHttp));
            var platformStatus = bfClient.GetPlatformStatusAsync().Result;

            // assert
            Assert.AreEqual(platformStatus.Operative, 0);
        }

        [TestMethod]
        public void When_ApiReturnsNoValue_PlatformStatusIsNull()
        {
            // arrange
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When($"*{Endpoints.PlatformStatus}")
                .Respond(MediaTypes.ApplicationJson, "[]");

            // act
            var bfClient = new BitfinexApiClient(ApiKey, SecretKey, new HttpClient(mockHttp));
            var platformStatus = bfClient.GetPlatformStatusAsync().Result;

            // assert
            Assert.IsNull(platformStatus);
        }

        // TODO: check further
        [TestMethod, ExpectedException(typeof(AggregateException))]
        public void When_ApiNotReachable_Expect_AggregateException()
        {
            // arrange
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When($"*{Endpoints.PlatformStatus}")
                .Respond(new ExceptionThrowingContent(
                    new HttpRequestException("An error occured while sending the request.")));

            // act
            var bfClient = new BitfinexApiClient(ApiKey, SecretKey, new HttpClient(mockHttp));
            var platformStatus = bfClient.GetPlatformStatusAsync().Result;
        }
    }
}