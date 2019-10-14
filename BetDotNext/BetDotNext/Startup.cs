using BetDotNext.Data;
using BetDotNext.Services;
using BetDotNext.Setup;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Telegram.Bot;

namespace BetDotNext
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var connection = string.Empty;
            var database = string.Empty;
            var adminPass = string.Empty;
            var userPass = string.Empty;

            var telegramToken = string.Empty;

            services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(telegramToken));
            services.AddSingleton(_ => new MongoClient(connection).GetDatabase(database).MongoDbInit(adminPass, userPass));
            services.AddSingleton<BetService>();
            services.AddSingleton<QueueMessagesService>();

            services.AddHostedService<BetToTelegramService>();

            services.AddSingleton<UserRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, BetService betService)
        {
            betService.Start();
        }
    }
}