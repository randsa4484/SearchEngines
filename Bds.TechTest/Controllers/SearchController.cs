using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SearchAggregator;

namespace Bds.TechTest.Controllers
{
    [Route("[controller]")]
    public class SearchController : Controller
    {
        private readonly ISearcher _searcher;

        public SearchController(ISearcher searcher)
        {
            _searcher = searcher;
        }

        [HttpGet]
        public async Task<IActionResult> WebSearch(string searchTerm)
        {
            if(string.IsNullOrEmpty(searchTerm))
                return new BadRequestResult();

            var results = await _searcher.Search(searchTerm);

            return new JsonResult(results);
        }

        [HttpGet("Example_To_Show_NotImplemented_Handled_By_Exception_Filter")]
        public IActionResult Example()
        {
            throw new NotImplementedException();
        }
    }
}
