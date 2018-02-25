using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("BitfinexApi.Test")] // applies for the whole assembly
namespace BitfinexApi
{
    internal static class Endpoints
    {
        internal const string PlatformStatus = "v2/platform/status";

        internal const string Alerts = "v2/auth/r/alerts";
        internal const string Wallets = "v2/auth/r/wallets";
    }

    internal static class MediaTypes
    {
        public const string ApplicationJson = "application/json";
    }
}
