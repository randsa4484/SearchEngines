using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SearchAggregator;
using SearchAggregator.Models;

namespace SearchAggregatorTests
{
    [TestClass]
    public class RegisterConfigsTests
    {
        [TestMethod, ExpectedException(typeof(NullReferenceException))]
        public void Test_RegisterGoogleEngine_Exception_ApiKey_NotSet()
        {
            var serviceCollection = new Mock<IServiceCollection>();

            serviceCollection.Object.RegisterGoogleEngine(builder => { });
        }

        [TestMethod, ExpectedException(typeof(NullReferenceException))]
        public void Test_RegisterGoogleEngine_Exception_Cx_NotSet()
        {
            var serviceCollection = new Mock<IServiceCollection>();

            serviceCollection.Object.RegisterGoogleEngine(builder =>
            {
                builder.Key = "a_key";
            });
        }

        [TestMethod]
        public void Test_RegisterGoogleEngine_Sets_SearcherRegistered()
        {
            var serviceCollection = new Mock<IServiceCollection>();

            serviceCollection.Object.RegisterGoogleEngine(builder =>
            {
                builder.Key = "a_key";
                builder.Cx = "cx";
            });

            Assert.IsTrue(RegisterConfigs.SearcherRegistered);
        }

        [TestMethod]
        public void Test_RegisterEngine_Sets_SearcherRegistered()
        {
            var serviceCollection = new Mock<IServiceCollection>();
 
            serviceCollection.Object.RegisterEngine<MockEngine>();

            Assert.IsTrue(RegisterConfigs.SearcherRegistered);
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private class MockEngine : ISearchEngine
        {
            public string Name { get; set; }
            public Task<IEnumerable<SearchResult>> Search(string term)
            {
                throw new NotImplementedException();
            }
        }
    }
}