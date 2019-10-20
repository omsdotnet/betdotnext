using System;
using System.IO;
using System.Reflection;
using BetDotNext.Commands;
using BetDotNext.ExternalServices;
using BetDotNext.Services;
using BetDotNext.Setup;
using MediatR;
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
            var database = _configuration["DB"] ?? "p";
            var telegramToken = _configuration["TelegramToken"] ?? "606619300:AAEluJ1V_SbMuUwzyqUBqa5wzoVgTbka4_g";

            services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(telegramToken));
            services.AddSingleton(_ => new MongoClient(connection).GetDatabase(database).MongoDbInit());

            services.AddSingleton<BetService>();
            services.AddSingleton<QueueMessagesService>();
            services.AddSingleton<ConversationService>();

            services.AddHostedService<BetToTelegramService>();

            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddSingleton<BotCommandService>();
            services.AddSingleton<StartCommand>();
            services.AddSingleton<BetCommand>();
            services.AddSingleton<RemoveBetCommand>();

            services.AddHttpClient<BetPlatformService>(client =>
            {
              client.BaseAddress = new Uri("http://bookmakerboard.azurewebsites.net/");
            });

          }).Configure(app =>
          {
            app.ApplicationServices.GetRequiredService<BetService>().Start();
          });
        });
  }
}