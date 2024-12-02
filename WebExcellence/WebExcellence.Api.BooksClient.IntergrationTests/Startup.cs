using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebExcellence.Api.BooksClient;

namespace WebExcellence.Api.BooksClient.IntergrationTests
{
    public class Startup
    {
        public void ConfigureHost(IHostBuilder hostBuilder)
        {
            // Arrange
            var config = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            hostBuilder.ConfigureHostConfiguration(builder =>
            {
                builder.AddConfiguration(config);
            });

            hostBuilder.ConfigureServices((context, services) =>
            {
                var url = config["API_BASE_URL"];
                if (string.IsNullOrEmpty(url))
                {
                    throw new InvalidOperationException("API_BASE_URL is required");
                }
                services.AddReliableBooksClient(url); // Simulate production injection
            });
        }
    }
}
