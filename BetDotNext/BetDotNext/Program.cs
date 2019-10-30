using System;
using System.IO;
using System.Net.Http;
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
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Serilog;
using Serilog.Events;
using Telegram.Bot;

namespace BetDotNext
{
  public static class Program
  {
    private static IConfiguration _configuration;

    public static int Main(string[] args)
    {
      try
      {
        CreateHostBuilder(args).Build().Run();
      }
      catch (Exception ex)
      {
        Log.Fatal(ex, "Host terminated unexpectedly");
        return 1;
      }
      finally
      {
        Log.CloseAndFlush();
      }

      return 0;
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
      Host.CreateDefaultBuilder(args)
        .ConfigureLogging(ConfigureLogging)
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
            var telegramToken = _configuration["TelegramToken"] ?? "606619300:AAErpzzU1A1LtArae57jrvYJbtXWAKR087M";

            var handler = new HttpClientHandler
            {
              ServerCertificateCustomValidationCallback = (message, certificate2, arg3, arg4) => true
            };

            services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(telegramToken, new HttpClient(handler)));
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
          }).UseSerilog();
        });

    private static void ConfigureLogging(HostBuilderContext builder, ILoggingBuilder loggingBuilder)
    {
      var seqHost = builder.Configuration["SEQ_PORT"];
      Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Debug()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.Seq(seqHost)
        .CreateLogger();
    }
  }
}