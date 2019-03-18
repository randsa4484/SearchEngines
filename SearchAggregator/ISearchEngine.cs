using System.Collections.Generic;
using System.Threading.Tasks;
using SearchAggregator.Models;

namespace SearchAggregator
{
    public interface ISearchEngine
    {
        string Name { get; }

        Task<IEnumerable<SearchResult>> Search(string term);
    }
}
