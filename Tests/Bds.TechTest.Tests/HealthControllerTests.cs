using System.Collections.Generic;
using Bds.TechTest.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SearchAggregator;

namespace Bds.TechTest.Tests
{
    [TestClass]
    public class HealthControllerTests
    {
        [TestMethod]
        public void Test_Get_Shows_Engine_Names()
        {
            var se1 = new Moq.Mock<ISearchEngine>();
            se1.Setup(x => x.Name).Returns("Google");
            var se2 = new Moq.Mock<ISearchEngine>();
            se2.Setup(x => x.Name).Returns("Yahoo");

            var searcher = new Moq.Mock<ISearcher>();
            searcher.Setup(x => x.SearchEngines).Returns(new List<ISearchEngine> {se1.Object, se2.Object});

            var controller = new HealthController(searcher.Object);

            var ret = controller.Get();
            var engines = ret.SearchEngines;
            Assert.AreEqual("Google,Yahoo", engines);
        }
    }
}