using BitexTradingBot.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace BitexTradingBot.Core.DataAccess.DataBase.Contexts
{
    class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<BitexTradingBotContext>
    {

        public BitexTradingBotContext CreateDbContext(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(@Directory.GetCurrentDirectory() + "/../BitexTradingBot/appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile(@Directory.GetCurrentDirectory() + "/../BitexTradingBot/" + $"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var builder = new DbContextOptionsBuilder<BitexTradingBotContext>();
            var connectionString = configuration.GetSection("WebJobConfiguration").Get<WebJobConfiguration>().DatabaseConnectionString;
            builder.UseSqlServer(connectionString);
            return new BitexTradingBotContext(builder.Options);
        }
    }
}
