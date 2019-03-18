using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TechTest.Web.Controllers;
using TechTest.Web.Models;
using TechTest.Web.SearchService;

namespace TechTest.Web.Tests
{
    [TestClass]
    public class HomeControllerTests
    {
        [TestMethod]
        public void Test_Search_Throws_SearchException()
        {
            var searchService = new Moq.Mock<ISearchService>();
            searchService.Setup(x => x.Search("xyz")).ThrowsAsync(new SearchException());

            var controller = new HomeController(searchService.Object);

            var result = controller.GetData("xyz").Result as PartialViewResult;

            Assert.AreEqual("_NoResultsPartialView", result.ViewName);
            Assert.IsNull(result.Model);
        }

        [TestMethod]
        public void Test_Search()
        {
            var sr = new SearchResult {Title = "BBC"};

            var searchService = new Moq.Mock<ISearchService>();
            searchService.Setup(x => x.Search("xyz")).ReturnsAsync(new List<SearchResult>{sr});

            var controller = new HomeController(searchService.Object);

            var result = controller.GetData("xyz").Result as PartialViewResult;

            Assert.AreEqual("_ResultsPartialView", result.ViewName);
            var searchRes = (SearchResults) result.Model;

            Assert.AreEqual("xyz", searchRes.SearchTerm);
            Assert.AreEqual(1, searchRes.Results.Count);

        }
    }
}
