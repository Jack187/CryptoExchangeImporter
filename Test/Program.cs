using System;
using System.Threading.Tasks;
using BitfinexAPI;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            BitfinexRestClient t = new BitfinexRestClient(string.Empty, String.Empty);
            var test = t.GetPlatformStatusAsync();

            string ttt = "";
            Console.WriteLine(test.Result.Operative);
            Console.ReadLine();

            // Test für die Exception
            //Test PlatformStatusAsync
        }
    }
}
