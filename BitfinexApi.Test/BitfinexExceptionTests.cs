using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BitfinexApi.Test
{
    [TestClass]
    public class BitfinexExceptionTests
    {
        [TestMethod, ExpectedException(typeof(BitfinexException))]
        public void When_JsonStringPlatformWorking_OperativeEqualsOne()
        {
            //// arrange
            //string jsonString = "[1]";

            //// act
            //var platformStatus = JsonConvert.DeserializeObject<PlatformStatus>(jsonString, new PlatformStatusConverter());

            //// assert
            //Assert.AreEqual(1, platformStatus.Operative);
            throw new BitfinexException(0,"");
        }
    }
}
