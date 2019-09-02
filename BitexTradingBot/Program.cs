using BitexTradingBot.Core.DataAccess.DataInvoke;
using BitexTradingBot.Core.Implementations;
using BitexTradingBot.Core.Interfaces;
using BitexTradingBot.Core.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BitexTradingBot
{
    class Program
    {
        private static IServiceProvider _serviceProvider;
        private static IConfigurationRoot _appConfig;

        static async Task Main(string[] args)
        {
            BuildConfiguration();
            RegisterServices();

            await _serviceProvider.GetService<WebJobEntryPoint>().Run();
        }

        private static void BuildConfiguration()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            Console.WriteLine($"Current Environment : {(string.IsNullOrEmpty(environment) ? "Development" : environment)}");

            _appConfig = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
               .AddEnvironmentVariables()
               .Build();
        }

        private static void RegisterServices()
        {
            IServiceCollection services = new ServiceCollection();

            var WebJobConfiguration = _appConfig.GetSection("WebJobConfiguration").Get<WebJobConfiguration>();

            services.AddTransient<WebJobEntryPoint>();
            services.AddTransient<IHttpClientApi, HttpClientApi>();
            services.AddTransient<ITradingApi, TradingApi>();
            services.AddSingleton<IWebJobConfiguration>(WebJobConfiguration);

            services.AddHttpClient("bitex", c =>
            {
                c.BaseAddress = new Uri(WebJobConfiguration.BitexApiUrl);
                c.DefaultRequestHeaders.Add("Authorization", WebJobConfiguration.BitexApiKey);
            });

            _serviceProvider = services.BuildServiceProvider();

        }
    }
}
