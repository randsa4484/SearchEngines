using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SearchAggregator.GoogleSearch;

namespace SearchAggregatorTests
{
    [TestClass]
    public class GoogleSearchEngineTests
    {
        [TestMethod]
        public void Test_GoogleSearchEngine_Initialisation()
        {
            var logger = new Mock<ILogger<GoogleSearchEngine>>();
            var configuration = new Mock<IGoogleConfiguration>();
            var google = new GoogleSearchEngine(logger.Object, configuration.Object);

            Assert.AreEqual("Google", google.Name);
        }
    }
}
