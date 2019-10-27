using System;
using System.IO;
using BetDotNext.Activity;
using BetDotNext.Activity.Bet;
using BetDotNext.BotPlatform;
using BetDotNext.BotPlatform.Impl;
using BetDotNext.ExternalServices;
using BetDotNext.Services;
using BetDotNext.Setup;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using Telegram.Bot;

namespace BetDotNext
{
  public static class Program
  {
    private static IConfiguration _configuration;

    public static void Main(string[] args)
    {
      CreateHostBuilder(args).Build().Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
      Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((hostingContext, builder) =>
        {
          builder.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .AddEnvironmentVariables();

          _configuration = builder.Build();
        }).ConfigureWebHostDefaults(webBuilder =>
        {
          webBuilder.ConfigureServices(services =>
          {
            var connection = _configuration["Mongo"] ?? "mongodb://192.168.168.131:28017";
            var database = _configuration["DB"] ?? "zx";
            var telegramToken = _configuration["TelegramToken"] ?? "606619300:AAFdo1oRuERSg-CEtoik5D198BrRV2gPrtM";

            services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(telegramToken));
            services.AddSingleton(_ => new MongoClient(connection).GetDatabase(database).MongoDbInit());

            services.AddSingleton<BetService>();
            services.AddSingleton<QueueMessagesService>();
            services.AddSingleton<ConversationService>();

            services.AddHostedService<BetToTelegramService>();

            services.AddHttpClient<BetPlatformService>(client =>
            {
              client.BaseAddress = new Uri("http://bookmakerboard.azurewebsites.net/");
            });

            services.AddSingleton<IBotStorage, BotStorageInMemory>();
            services.AddSingleton<IBot, Bot>();
            services.AddSingleton<BetActivity>();
            services.AddSingleton<StartActivity>();
            services.AddSingleton<RemoveBetActivity>();

          }).Configure(app =>
          {
            app.ApplicationServices.GetRequiredService<IBot>()
              .AddActivity("/start", typeof(StartActivity))
              .AddActivity("/bet", typeof(BetActivity))
              .AddActivity("/removebet", typeof(RemoveBetActivity));

            app.ApplicationServices.GetRequiredService<BetService>().Start();
          });
        });
  }
}