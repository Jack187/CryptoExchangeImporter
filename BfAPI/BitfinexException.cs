using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BfAPI
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