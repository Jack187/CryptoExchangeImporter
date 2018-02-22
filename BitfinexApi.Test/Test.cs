using Microsoft.VisualStudio.TestTools.UnitTesting;
using RichardSzalay.MockHttp;
using System.Net.Http;

namespace BitfinexApi.Test
{
    [TestClass]
    public class Test
    {
        [TestMethod]
        public void TestMethod1()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("*v2/platform/status")
                .Respond("application/json", "[1]"); // Respond with JSON

            var bfClient = new BitfinexApiClient("", "", new HttpClient(mockHttp));
            var platformStatus = bfClient.GetPlatformStatusAsync().Result;

            Assert.AreEqual(platformStatus.Operative, 1);
        }

        [TestMethod]
        public void TestMethod2()
        {
            //var mockHttp = new MockHttpMessageHandler();
            //mockHttp.When("*v2/platform/status")
            //    .Respond("application/json", "[1]"); // Respond with JSON

            var bfClient = new BitfinexApiClient(
                "",
                "",
                new HttpClient());

            var wallets = bfClient.GetWalletsAsync().Result;

            Assert.AreEqual(wallets.Count, 6);
        }
    }
}
