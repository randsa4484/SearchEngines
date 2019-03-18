using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TechTest.Web.Models;

namespace TechTest.Web.SearchService
{
    public class SearchService : ISearchService
    {
        private readonly ILogger<SearchResult> _logger;

        public SearchService(ILogger<SearchResult> logger, IConfiguration configuration)
        {
            _logger = logger;
            SearchServiceBaseUrl = configuration["SearchService:BaseUrl"];
            SearchEndPoint = configuration["SearchService:SearchEndpoint"];
            PingEndPoint = configuration["SearchService:PingEndpoint"];
        }

        public string SearchServiceBaseUrl { get; }
        public string SearchEndPoint { get; }
        public string PingEndPoint { get; }

        public async Task<IEnumerable<SearchResult>> Search(string searchTerm)
        {
            try
            {
                var result = await CallWebAPI($"{SearchEndPoint}?searchTerm={searchTerm}");

                return await result.Content.ReadAsAsync<IEnumerable<SearchResult>>();
            }
            catch(Exception)
            {
                // exception will have been logged
                throw new SearchException();
            }
        }

        public void Ping()
        {
            var res = CallWebAPI(PingEndPoint).Result;

            dynamic data = res.Content.ReadAsAsync<dynamic>().Result;

            _logger.LogInformation("Web API Running - {0}", (object)data);
        }

        private async Task<HttpResponseMessage> CallWebAPI(string relativeUri)
        {
            if(string.IsNullOrEmpty(SearchServiceBaseUrl))
                throw new NullReferenceException("Configuration setting SearchServiceBaseUrl has not been set");

            var uri = new Uri(new Uri(SearchServiceBaseUrl), relativeUri).AbsoluteUri;

            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage urlResult = null;
                try
                {
                    urlResult = await httpClient.GetAsync(uri);

                    if (!urlResult.IsSuccessStatusCode)
                    {
                        throw new HttpRequestException("call to SearchService WebAPI failed: " + urlResult);
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError($"Search Service WebAPI call has failed. Exception {e.GetType()} thrown. Message = {e.Message}");
                        throw;
                }

                return urlResult;
            }
        }
    }
}
