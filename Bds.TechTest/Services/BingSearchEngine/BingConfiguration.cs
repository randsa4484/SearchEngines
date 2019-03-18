using System;
using Microsoft.Extensions.Configuration;

namespace Bds.TechTest.Services.BingSearchEngine
{
    internal interface IBingConfiguration
    {
        string ApiKey { get; }
    }

    internal class BingConfiguration: IBingConfiguration
    {
        public BingConfiguration(IConfiguration configuration)
        {
            ApiKey = configuration["Bing:apiKey"];

            if(string.IsNullOrEmpty(ApiKey))
                throw new NullReferenceException("Bing Api Key must be specified");
        }

        public string ApiKey { get; }
    }
}