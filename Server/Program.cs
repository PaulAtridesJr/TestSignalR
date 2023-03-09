using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Formatting.Compact;

namespace xemtest;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddTransient<IWorker, SimpleWorker>();
        builder.Services.AddTransient<IUI, Form1>();
        builder.Services.AddHostedService<TestHostedService>();
        builder.Services.AddSignalR().AddJsonProtocol(opt => opt.PayloadSerializerOptions.Converters.Add(new IntPtrConverter()));
        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog();
        builder.Host.UseSerilog((context, services, configuration) => configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext()
            .WriteTo.File(new CompactJsonFormatter(), "server.log")
            .WriteTo.Console()); // redirect all log events through Serilog pipeline

        var app = builder.Build();
        app.UseSerilogRequestLogging(); // HTTP request logging 

        app.MapHub<TestHub>("/Test");

        var tsk = app.RunAsync(@"http://localhost:9123");
        while (tsk.IsCompleted == false)
        {
            Application.DoEvents();
        }
    }
}