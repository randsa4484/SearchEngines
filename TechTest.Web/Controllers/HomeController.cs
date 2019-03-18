using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TechTest.Web.Models;
using TechTest.Web.SearchService;

namespace TechTest.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISearchService _searchService;

        public HomeController(ISearchService searchService)
        {
            _searchService = searchService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetData(string searchTerm)
        {
            IEnumerable<SearchResult> results = null;
            try
            {
                results = await _searchService.Search(searchTerm);
            }
            catch (SearchException)
            {
                return PartialView("_NoResultsPartialView");
            }

            var sr = new SearchResults
            {
                Results = results.ToList(),
                SearchTerm = searchTerm
            };

            return PartialView("_ResultsPartialView", sr);
        }
        
        public IActionResult About()
        {
            ViewData["Message"] = "Demonstration of Fetching results from Search Engines.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Andrew Rands";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
