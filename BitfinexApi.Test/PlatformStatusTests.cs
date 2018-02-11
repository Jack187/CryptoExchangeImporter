using BitfinexApi.JsonConverters;
using BitfinexApi.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace BitfinexApi.Test
{
    [TestClass]
    public class PlatformStatusTests
    {
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
            // arrange
            string jsonString = "[0]";

            // act
            var platformStatus = JsonConvert.DeserializeObject<PlatformStatus>(jsonString, new PlatformStatusConverter());

            // assert
            Assert.AreEqual(0, platformStatus.Operative);
        }

        [TestMethod]
        public void When_JsonStringHasNoValue_PlatformStatusIsNull()
        {
            // arrange
            string jsonString = "[]";

            // act
            var platformStatus = JsonConvert.DeserializeObject<PlatformStatus>(jsonString, new PlatformStatusConverter());

            // assert
            Assert.IsNull(platformStatus);
        }

        [TestMethod]
        public void When_JsonStringIsEmpty_PlatformStatusIsNull()
        {
            // arrange
            string jsonString = string.Empty;

            // act
            var platformStatus = JsonConvert.DeserializeObject<PlatformStatus>(jsonString, new PlatformStatusConverter());

            // assert
            Assert.IsNull(platformStatus);
        }

        [TestMethod]
        public void When_JsonStringIsNonIntegerValue_PlatformStatusIsNull()
        {
            // arrange
            string jsonString = "[\"a\"]";

            // act
            var platformStatus = JsonConvert.DeserializeObject<PlatformStatus>(jsonString, new PlatformStatusConverter());

            // assert
            Assert.IsNull(platformStatus);
        }
    }
}