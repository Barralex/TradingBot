﻿using BitexTradingBot.Core.Constants;
using BitexTradingBot.BitexTradingBot.Impl;
using BitexTradingBot.Core.DataAccess.DataBase.Contexts;
using BitexTradingBot.Core.DataAccess.DataInvoke;
using BitexTradingBot.Core.Implementations;
using BitexTradingBot.Core.Interfaces;
using BitexTradingBot.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace BitexTradingBot
{
    public class Program
    {
        private static IServiceProvider _serviceProvider;
        private static IConfigurationRoot _appConfig;

        public delegate ITradingApi ServiceResolver(ServiceKeyEnum key);

        private static async Task Main(string[] args)
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
            services.AddTransient<IStrategy, LateralStrategy>();

            services.AddTransient<BitexTradingApi>();

            services.AddTransient<ServiceResolver>(serviceProvider => key =>
            {
                switch (key)
                {
                    case ServiceKeyEnum.Bitex:
                        return serviceProvider.GetService<BitexTradingApi>();

                    default:
                        throw new KeyNotFoundException();
                }
            });

            services.AddSingleton<IWebJobConfiguration>(WebJobConfiguration);

            services.AddHttpClient("bitex", c =>
            {
                c.BaseAddress = new Uri(WebJobConfiguration.BitexApiUrl);
                c.DefaultRequestHeaders.Add("Authorization", WebJobConfiguration.BitexApiKey);
            });

            services.AddHttpClient("coinmarketcap", c =>
            {
                c.BaseAddress = new Uri(WebJobConfiguration.CoinmarketcapUrl);
                c.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", WebJobConfiguration.CoinmarketcapApi);
            });

            services.AddDbContext<BitexTradingBotContext>(options => options.UseSqlServer(WebJobConfiguration.DatabaseConnectionString));

            _serviceProvider = services.BuildServiceProvider();

        }
    }
}