using System.Collections.Generic;
using Bds.TechTest.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SearchAggregator;
using SearchAggregator.Models;

namespace Bds.TechTest.Tests
{
    [TestClass]
    public class SearchControllerTests
    {
        [TestMethod]
        public void WebSearch_Null_String_Bad_Request()
        {
            WebSearch_NorOrEmpty_String_Bad_Request(null);
        }

        [TestMethod]
        public void WebSearch_Empty_String_Bad_Request()
        {
            WebSearch_NorOrEmpty_String_Bad_Request("");
        }

        private static void WebSearch_NorOrEmpty_String_Bad_Request(string term)
        {
            var searcher = new Moq.Mock<ISearcher>();

            var controller = new SearchController(searcher.Object);

            var result = controller.WebSearch(term).Result;

            Assert.IsTrue(result is BadRequestResult);
        }

        [TestMethod]
        public void WebSearch()
        {
            var searcher = new Moq.Mock<ISearcher>();

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

            var res = new List<SearchResult> {sr1, sr2};

            searcher.Setup(x => x.Search("BBC")).ReturnsAsync(res);

            var controller = new SearchController(searcher.Object);

            var result = (JsonResult)controller.WebSearch("BBC").Result;

            var searchItems = (List<SearchResult>)result.Value;

            Assert.AreEqual(2, searchItems.Count);
        }
    }
}