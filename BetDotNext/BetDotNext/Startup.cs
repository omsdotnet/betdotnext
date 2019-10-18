using BetDotNext.Data;
using BetDotNext.Services;
using BetDotNext.Setup;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Telegram.Bot;

namespace BetDotNext
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            _configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var connection = _configuration["Mongo"];
            var database = _configuration["DB"];

            var telegramToken = _configuration["TelegramToken"];

            services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(telegramToken));
            services.AddSingleton(_ => new MongoClient(connection).GetDatabase(database).MongoDbInit());

            services.AddSingleton<BetService>();
            services.AddSingleton<QueueMessagesService>();
            services.AddSingleton<ActiveCommandService>();

            services.AddHostedService<BetToTelegramService>();

            services.AddSingleton<UserRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, BetService betService)
        {
            betService.Start();
        }
    }
}