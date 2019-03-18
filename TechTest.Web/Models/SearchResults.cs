using System.Collections.Generic;

namespace TechTest.Web.Models
{
    public class SearchResults
    {
        public List<SearchResult> Results { get; set; }
        public string SearchTerm { get; set; }
    }
}