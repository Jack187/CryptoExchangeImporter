using System;
using System.Configuration;
using BfAPI;

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

                BitfinexRestClient bfRestClient = new BitfinexRestClient(apiKey, apiSecret);

                //var platformStatus = bfRestClient.GetPlatformStatusAsync().Result;
                //var alerts = bfRestClient.GetAlertsAsync().Result;

                //Console.WriteLine($"API State (1=up / 0=down): {platformStatus.Operative}");

                //Console.WriteLine("Alerts:");
                //alerts.ForEach(alert => Console.WriteLine(alert.ToString()));

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
