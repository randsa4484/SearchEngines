using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Search.WebSearch;
using SearchAggregator;
using SearchAggregator.Models;

namespace Bds.TechTest.Services.BingSearchEngine
{
    internal class BingSearchEngine : ISearchEngine
    {
        private readonly IBingConfiguration _bingConfiguration;

        public BingSearchEngine(IBingConfiguration bingConfiguration)
        {
            // validity of configuration properties checked in startup
            _bingConfiguration = bingConfiguration;
        }

        public string Name => "Bing";

        public async Task<IEnumerable<SearchResult>> Search(string term)
        {
            var client = new WebSearchClient(new ApiKeyServiceClientCredentials(_bingConfiguration.ApiKey));

            var webData = await client.Web.SearchWithHttpMessagesAsync(term);

            var items = webData.Body.WebPages.Value;

            return items.Select(i => new SearchResult
            {
                Link = i.DisplayUrl,
                Snippet = i.Snippet,
                Title = i.Name
            });
        }
    }
}
