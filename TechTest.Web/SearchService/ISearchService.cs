using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechTest.Web.Models;

namespace TechTest.Web.SearchService
{
    public interface ISearchService
    {
        Task<IEnumerable<SearchResult>> Search(string searchTerm);
        void Ping();
    }

    public class SearchException : Exception
    { }
}