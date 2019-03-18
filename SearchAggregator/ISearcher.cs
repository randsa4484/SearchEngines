using System.Collections.Generic;
using System.Threading.Tasks;
using SearchAggregator.Models;

namespace SearchAggregator
{
    public interface ISearcher
    {
        IEnumerable<ISearchEngine> SearchEngines { get; }

        Task<IEnumerable<SearchResult>> Search(string searchTerm);
    }
}