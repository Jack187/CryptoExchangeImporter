namespace BitfinexApi.Resources
{
    public class Wallet
    {
        public string WalletType { get; set; }
        public string Currency { get; set; }
        public double Balance { get; set; }
        public double UnsettledInterest { get; set; }
        public double? BalanceAvailable { get; set; }
        
        public override string ToString()
        {
            return $"WalletType: {WalletType}, " +
                $"Currency: {Currency}," +
                $" Balance: {Balance}," +
                $" UnsettledInterest: {UnsettledInterest}," +
                $" BalanceAvailable: {BalanceAvailable}";
        }
    }
}