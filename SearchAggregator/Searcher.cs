using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SearchAggregator.Models;

namespace SearchAggregator
{
    internal class Searcher : ISearcher
    {
        public Searcher(IEnumerable<ISearchEngine> searchEngines)
        {
            _searchEngines.AddRange(searchEngines);
        }

        private readonly List<ISearchEngine> _searchEngines = new List<ISearchEngine>();

        public IEnumerable<ISearchEngine> SearchEngines => _searchEngines;

        public async Task<IEnumerable<SearchResult>> Search(string searchTerm)
        {
            var tasks = new List<Task<IEnumerable<SearchResult>>>();

            foreach (var searchEngine in SearchEngines)
            {
                tasks.Add(Search(searchEngine, searchTerm));
            }

            await Task.WhenAll(tasks.ToArray());

            return tasks.SelectMany(t => t.Result);
        }

        public async Task<IEnumerable<SearchResult>> Search(ISearchEngine searchEngine, string searchTerm)
        {
            var results = (await searchEngine.Search(searchTerm)).ToList();

            foreach (var searchResult in results)
            {
                searchResult.SearchEngine = searchEngine.Name;
            }

            return results;
        }
    }
}