using Microsoft.VisualStudio.TestTools.UnitTesting;
using RichardSzalay.MockHttp;
using System;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net.Http;

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
        [DataRow("[]", 0)]
        [DataRow("[[\"exchange\",\"BTC\",0.0583,0,null]]", 1)]
        [DataRow("[[\"exchange\",\"BCH\",9.9e-7,0,null]," +
            "[\"exchange\",\"IOT\",52.94757829,0,null]," +
            "[\"exchange\",\"USD\",8.2e-7,0,null]," +
            "[\"exchange\",\"XMR\",1.99399999,0,null]," +
            "[\"exchange\",\"XRP\",364.5789779,0,null]," +
            "[\"exchange\",\"OMG\",0.2058,0,null]]", 6)]
        public void When_ApiReturnsNWallets_WalletsListContainsNWallets(string responseJson, int walletCount)
        {
            // arrange
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When($"*{Endpoints.Wallets}")
                .Respond(MediaTypes.ApplicationJson, responseJson);

            // act
            var bfClient = new BitfinexApiClient(ApiKey, SecretKey, new HttpClient(mockHttp));
            var wallets = bfClient.GetWalletsAsync().Result;

            // assert
            Assert.AreEqual(wallets.Count, walletCount);
        }

        [TestMethod]
        [DataRow("exchange", "BTC", 0.0583, 0, null)]
        [DataRow("exchange", "IOT", 52.94757829, 0.2, 0.5)]
        public void When_ApiReturnsWalletData_Expect_WalletWithSameData(
            string walletType, string currency, double balance, double unsettledInterest, double? balanceAvailable)
        {
            // arrange
            var responseJson = string.Format("[[\"{0}\",\"{1}\",{2},{3},{4}]]",
                walletType, 
                currency,
                balance.ToString(CultureInfo.InvariantCulture),
                unsettledInterest.ToString(CultureInfo.InvariantCulture),
                balanceAvailable?.ToString(CultureInfo.InvariantCulture) ?? "null");

            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When($"*{Endpoints.Wallets}")
                .Respond(MediaTypes.ApplicationJson, responseJson);

            // act
            var bfClient = new BitfinexApiClient(ApiKey, SecretKey, new HttpClient(mockHttp));
            var wallets = bfClient.GetWalletsAsync().Result;
            var wallet = wallets.FirstOrDefault(w => w.Currency.Equals(currency));

            // assert
            Assert.IsNotNull(wallet);
            Assert.AreEqual(wallet.WalletType, walletType);
            Assert.AreEqual(wallet.Currency, currency);
            Assert.AreEqual(wallet.Balance, balance);
            Assert.AreEqual(wallet.UnsettledInterest, unsettledInterest);
            Assert.AreEqual(wallet.BalanceAvailable, balanceAvailable);
        }
    }
}