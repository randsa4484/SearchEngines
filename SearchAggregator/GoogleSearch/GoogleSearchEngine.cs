using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Services;
using Microsoft.Extensions.Logging;
using SearchAggregator.Models;

namespace SearchAggregator.GoogleSearch
{
    internal class GoogleSearchEngine : ISearchEngine
    {
        private readonly ILogger<GoogleSearchEngine> _logger;
        private readonly IGoogleConfiguration _googleConfiguration;

        public GoogleSearchEngine(ILogger<GoogleSearchEngine> logger, IGoogleConfiguration googleConfiguration)
        {
            _logger = logger;
            // builder will check validity of configuration properties
            _googleConfiguration = googleConfiguration;
        }

        public string Name => "Google";

        public async Task<IEnumerable<SearchResult>> Search(string term)
        {
            var customsearchService = new Google.Apis.Customsearch.v1.CustomsearchService(new BaseClientService.Initializer
            {
                ApiKey = _googleConfiguration.Key
            });

            var listRequest = customsearchService.Cse.List(term);
            listRequest.Cx = _googleConfiguration.Cx;

            var search = await listRequest.ExecuteAsync();

            return search.Items.Select(i => new SearchResult
            {
                Link = i.Link,
                Snippet = i.HtmlSnippet,
                Title = i.Title
            });
        }
    }
}