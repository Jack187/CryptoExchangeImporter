using BitfinexApi;
using BitfinexApi.Configuration;
using System;
using System.Net.Http;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {            
            BitfinexApiClient bfRestClient = new BitfinexApiClient(Config.ApiKey, Config.SecretKey, new HttpClient());

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
    }
}
