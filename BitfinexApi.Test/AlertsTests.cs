using BitfinexApi.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RichardSzalay.MockHttp;
using System.Linq;
using System.Net.Http;

namespace BitfinexApi.Test
{
    [TestClass]
    public class AlertsTests
    {
        [TestMethod]
        [DataRow("[]", 0)]
        [DataRow("[[\"price:tBTCUSD:8300\",\"price\",\"tBTCUSD\",8300,9]]", 1)]
        [DataRow("[[\"price:tBTCUSD:8300\",\"price\",\"tBTCUSD\",8300,9]," +
            "[\"price:tIOTBTC:0.00000081\",\"price\",\"tIOTBTC\",8.1e-7,100]," +
            "[\"price:tETHBTC:0.012\",\"price\",\"tETHBTC\",0.012,100]]", 3)]
        public void When_ApiReturnsNAlertss_AlertsListContainsNAlerts(string responseJson, int alertsCount)
        {
            // arrange
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When($"*{Endpoints.Alerts}")
                .Respond(MediaTypes.ApplicationJson, responseJson);

            // act
            var bfClient = new BitfinexApiClient(Config.ApiKey, Config.SecretKey, new HttpClient(mockHttp));
            var alerts = bfClient.GetAlertsAsync().Result;

            // assert
            Assert.AreEqual(alerts.Count, alertsCount);
        }

        [TestMethod]
        [DataRow("[[\"price:tBTCUSD:8300\",\"price\",\"tBTCUSD\",8300,9]]", "price:tBTCUSD:8300", "price", "tBTCUSD", 8300, 9)]
        public void When_ApiReturnsAlertAsJson_Expect_AlertWithSameData(string responseJson, string id, string type, string symbol, double price, int unknown)
        {
            // arrange
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When($"*{Endpoints.Alerts}")
                .Respond(MediaTypes.ApplicationJson, responseJson);

            // act
            var bfClient = new BitfinexApiClient(Config.ApiKey, Config.SecretKey, new HttpClient(mockHttp));
            var alerts = bfClient.GetAlertsAsync().Result;
            var alert = alerts.FirstOrDefault();

            // assert
            Assert.IsNotNull(alert);
            Assert.AreEqual(alert.Id, id);
            Assert.AreEqual(alert.Type, type);
            Assert.AreEqual(alert.Symbol, symbol);
            Assert.AreEqual(alert.Price, price);
            Assert.AreEqual(alert.Unknown, unknown);
        }
    }
}