using System;
using System.Reflection.Metadata.Ecma335;
using Microsoft.Extensions.DependencyInjection;
using SearchAggregator.GoogleSearch;

namespace SearchAggregator
{
    public static class RegisterConfigs
    {
        public static void RegisterGoogleEngine(this IServiceCollection serviceCollection, Action<GoogleBuilder> builder)
        {
            var config = new GoogleBuilder();
            builder(config);

            if (string.IsNullOrEmpty(config.Key))
                throw new NullReferenceException("Google API Key must be specified");

            if (string.IsNullOrEmpty(config.Cx))
                throw new NullReferenceException("Google Search Engine Id (cx) must be specified");

            serviceCollection.AddSingleton<IGoogleConfiguration>(config);
            serviceCollection.AddTransient<ISearchEngine, GoogleSearchEngine>();

            if (SearcherRegistered) return;

            serviceCollection.AddTransient<ISearcher, Searcher>();
            SearcherRegistered = true;
        }

        public static bool SearcherRegistered { get; private set; }

        public static void RegisterEngine<TEngine>(this IServiceCollection serviceCollection) where TEngine : class, ISearchEngine 
        {
            serviceCollection.AddTransient<ISearchEngine, TEngine>();

            if (SearcherRegistered) return;

            serviceCollection.AddTransient<ISearcher, Searcher>();
            SearcherRegistered = true;
        }
    }
}