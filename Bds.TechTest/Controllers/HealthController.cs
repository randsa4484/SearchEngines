using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using SearchAggregator;

namespace Bds.TechTest.Controllers
{
    [Route("[controller]")]
    public class HealthController : Controller
    {
        private readonly ISearcher _searcher;

        public HealthController(ISearcher searcher)
        {
            _searcher = searcher;
        }

        [HttpGet]
        public dynamic Get()
        {
            var SearchEngines = string.Join(",", _searcher.SearchEngines.Select(se => se.Name));

            var Version = Assembly.GetEntryAssembly().GetName().Version.ToString();

            return new
            {
                SearchEngines,
                Version
            };
        }

    }
}
