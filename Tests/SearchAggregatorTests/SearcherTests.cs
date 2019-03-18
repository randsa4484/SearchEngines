using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SearchAggregator;
using SearchAggregator.Models;

namespace SearchAggregatorTests
{
    [TestClass]
    public class SearcherTests
    {
        [TestMethod]
        public void Search()
        {
            var sr1 = new SearchResult
            {
                Title = "BBC",
                Link = "https://www.bbc.co.uk",
                Snippet = "Blah Blah Blah"
            };

            var sr2 = new SearchResult
            {
                Title = "BBC News",
                Link = "https://www.bbc.co.uk/news",
                Snippet = "News Blah Blah Blah"
            };

            var searchEngine1 = new Moq.Mock<ISearchEngine>();
            searchEngine1.Setup(x => x.Search("jim")).ReturnsAsync(new List<SearchResult>() {sr1, sr2});
            var searchEngine2 = new Moq.Mock<ISearchEngine>();
            searchEngine2.Setup(x => x.Search("jim")).ReturnsAsync(new List<SearchResult>() {sr1, sr2});
            var searcher = new Searcher(new List<ISearchEngine> {searchEngine1.Object, searchEngine2.Object});

            Assert.AreEqual(2, searcher.SearchEngines.Count());

            var res = searcher.Search(searchEngine1.Object, "jim").Result;

            Assert.AreEqual(2, res.Count());

            res = searcher.Search("jim").Result;

            Assert.AreEqual(4, res.Count());
        }

        [TestMethod]
        public void Search_Adds_Engine_Name()
        {
            var sr1 = new SearchResult
            {
                Title = "BBC",
                Link = "https://www.bbc.co.uk",
                Snippet = "Blah Blah Blah"
            };

            var searchEngine = new Moq.Mock<ISearchEngine>();
            searchEngine.Setup(x => x.Name).Returns("TestEngine");
            searchEngine.Setup(x => x.Search("jim")).ReturnsAsync(new List<SearchResult>() { sr1 });
            var searcher = new Searcher(new List<ISearchEngine> { searchEngine.Object });
            
            var res = searcher.Search(searchEngine.Object, "jim").Result;

            Assert.AreEqual("TestEngine", res.First().SearchEngine);

        }
    }
}