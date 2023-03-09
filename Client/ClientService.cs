using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace xemtestclient
{
    public sealed class ClientService : IHostedService
{
    private readonly ILogger _logger;
    private readonly ITestClient client;
    public ClientService(
        ILogger<ClientService> logger,
        IHostApplicationLifetime appLifetime,
        ITestClient client)
    {
        _logger = logger;
        this.client = client;
        appLifetime.ApplicationStarted.Register(OnStarted);
        appLifetime.ApplicationStopping.Register(OnStopping);
        appLifetime.ApplicationStopped.Register(OnStopped);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("1. StartAsync has been called.");

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("4. StopAsync has been called.");

        return Task.CompletedTask;
    }

    private async void OnStarted()
    {
        _logger.LogInformation("2. OnStarted has been called.");
      
            await client.Connect();
            await client.SendCommandToServer();

            await Task.Delay(3000);

            await client.SendCommandToServerWithParams();
           
    }

    private async void OnStopping()
    {
        _logger.LogInformation("3. OnStopping has been called.");
         await client.StopAsync();
    }

    private void OnStopped()
    {
        _logger.LogInformation("5. OnStopped has been called.");
    }
}
}