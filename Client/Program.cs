using System;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Hosting;
using Serilog.AspNetCore;
using Microsoft.Extensions.Logging;

namespace xemtestclient
{
    class Programm
    {
        static async Task Main(string[] args)
        {            
            
            using IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                    services
                        .AddTransient<ITestClient, TestClient>()
                        .AddHostedService<ClientService>())
                        .UseSerilog((hostContext, services, configuration) => 
                        {
                            configuration.WriteTo.File("client.log").WriteTo.Console();
                        })
              
                .Build();
            
            AppDomain.CurrentDomain.UnhandledException += (a, e) => 
            {
                var logger = host.Services.GetRequiredService<Microsoft.Extensions.Logging.ILogger<Programm>>();
                logger.LogError($"Crash - {e.ExceptionObject}");
            };


            await host.RunAsync();
            Console.ReadLine();
        }
    }
}
