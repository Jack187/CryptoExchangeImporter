using System;

namespace BitfinexApi
{
    // TODO: see if a common CryptoExchangeException is suitable
    public class BitfinexException : Exception
    {
        public int ErrorCode { get; }

        public BitfinexException(int errorCode, string message)
            : base(message)

        {
            ErrorCode = errorCode;
        }
    }
}