using System.IO;
using System.Reflection;
using BetDotNext.Data;
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
            var connection = _configuration["Mongo"];
            var database = _configuration["DB"];
            var telegramToken = _configuration["TelegramToken"];

           services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(telegramToken));
            //services.AddSingleton(_ => new MongoClient(connection).GetDatabase(database).MongoDbInit());

            services.AddSingleton<BetService>();
            //services.AddSingleton<QueueMessagesService>();
            services.AddSingleton<ActiveCommandService>();
            //services.AddSingleton<UserRepository>();

            //services.AddHostedService<BetToTelegramService>();

            services.AddMediatR(Assembly.GetExecutingAssembly());
          }).Configure(app =>
          {
            app.ApplicationServices.GetRequiredService<BetService>().Start();
          });
        });
  }
}