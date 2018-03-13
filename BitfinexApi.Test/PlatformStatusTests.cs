using BitfinexApi.Configuration;
using BitfinexApi.Test.Mocking;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RichardSzalay.MockHttp;
using System;
using System.Net.Http;

namespace BitfinexApi.Test
{
    [TestClass]
    public class PlatformStatusTests
    {
        [TestMethod]
        public void When_ApiReturnsPlatformWorking_OperativeEqualsOne()
        {            
            // arrange
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When($"*{Endpoints.PlatformStatus}")
                .Respond(MediaTypes.ApplicationJson, "[1]");

            // act
            var bfClient = new BitfinexApiClient(Config.ApiKey, Config.SecretKey, new HttpClient(mockHttp));
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
            var bfClient = new BitfinexApiClient(Config.ApiKey, Config.SecretKey, new HttpClient(mockHttp));
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
            var bfClient = new BitfinexApiClient(Config.ApiKey, Config.SecretKey, new HttpClient(mockHttp));
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
            var bfClient = new BitfinexApiClient(Config.ApiKey, Config.SecretKey, new HttpClient(mockHttp));
            var platformStatus = bfClient.GetPlatformStatusAsync().Result;
        }
    }
}