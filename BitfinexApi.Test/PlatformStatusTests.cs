using BitfinexApi.JsonConverters;
using BitfinexApi.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace BitfinexApi.Test
{
    [TestClass]
    public class PlatformStatusTests
    {
        // Should_ReturnPlatformStatus1_When_CorrectJsonStringContains1
        // When_DeserializeJsonStringThatIndicatesPlatformWorking_Expect_PlatformStatusWithOperative1

        [TestMethod]
        public void When_JsonStringPlatformWorking_OperativeEqualsOne()
        {
            // arrange
            string jsonString = "[1]";

            // act
            var platformStatus = JsonConvert.DeserializeObject<PlatformStatus>(jsonString, new PlatformStatusConverter());

            // assert
            Assert.AreEqual(1, platformStatus.Operative);
        }

        [TestMethod]
        public void When_JsonStringPlatformNotWorking_OperativeEqualsZero()
        {
        }
    }
}
