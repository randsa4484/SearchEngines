using System;
using System.Reflection;
using Bds.TechTest.Filters;
using Bds.TechTest.Services.BingSearchEngine;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SearchAggregator;
using Swashbuckle.AspNetCore.Swagger;

namespace Bds.TechTest
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                // api keys for search engines are NOT to be 
                // committed to source control
                builder.AddUserSecrets<Startup>();
            }

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration);

            // register a provided search engine
            services.RegisterGoogleEngine(google =>
            {
                google.Key = Configuration["Google:apiKey"];
                google.Cx = Configuration["Google:cx"];
            });

            // register our own search engine
            services.AddSingleton<IBingConfiguration, BingConfiguration>();
            services.RegisterEngine<BingSearchEngine>();

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(ExceptionFilterAttribute));
            });

            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("doc", new Info
                {
                    Version = Assembly.GetEntryAssembly().GetName().Version.ToString(),
                    Title = "Technical Test",
                    Description = "Allow user to amalgamate search queries across engines",
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            var path = "TechTest";
            var helpRoot = "help";
            var swaggerRoot = "swagger/doc/swagger.json";

            if (!string.IsNullOrWhiteSpace(path))
            {
                path = path.StartsWith("/") ? path : "/" + path;
                app.UsePathBase(path);

                swaggerRoot = $"{path}/{swaggerRoot}";
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = helpRoot;
                c.SwaggerEndpoint(swaggerRoot, "Technical Test");
            });

            // make sure Bing search engine is configured correctly
            var bingConfiguration = app.ApplicationServices.GetService<IBingConfiguration>();

            if (string.IsNullOrEmpty(bingConfiguration.ApiKey))
            {
                var msg = "Bing API key must be specified";
                var e = new Exception(msg);
                loggerFactory.CreateLogger<Startup>().LogError("Configuration Error", e);
                // this is a no go
                throw e;
            }

            app.UseMvc();
        }
    }
}
