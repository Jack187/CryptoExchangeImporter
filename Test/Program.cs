using BitfinexApi;
using System;
using System.Configuration;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var apiKey = ConfigurationManager.AppSettings["BfApiKey"];
                var apiSecret = ConfigurationManager.AppSettings["BfApiSecret"];

                if (string.IsNullOrEmpty(apiKey))
                    throw new Exception($"Missing BfApiKey in App.config");

                if (string.IsNullOrEmpty(apiSecret))
                    throw new Exception("Missing BfApiSecret in App.config");

                BitfinexApiClient bfRestClient = new BitfinexApiClient(apiKey, apiSecret);

                var platformStatus = bfRestClient.GetPlatformStatusAsync().Result;
                Console.WriteLine($"API State (1=up / 0=down): {platformStatus.Operative}");

                var alerts = bfRestClient.GetAlertsAsync().Result;
                Console.WriteLine("Alerts:");
                alerts.ForEach(alert => Console.WriteLine(alert.ToString()));

                var wallets = bfRestClient.GetWalletsAsync().Result;
                Console.WriteLine("Wallets:");
                wallets.ForEach(wallet => Console.WriteLine(wallet.ToString()));

                Console.ReadLine();

            }
            catch (Exception ex)
            {
                //TODO: add logging
            }

            // Test für die Exception
            //Test PlatformStatusAsync
        }
    }
}
