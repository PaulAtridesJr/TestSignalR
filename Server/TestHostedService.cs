using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace xemtest
{
    internal class TestHostedService : IHostedService
    {
        private readonly ILogger<TestHostedService> logger;
        private readonly IUI ui;
        private readonly IHubContext<TestHub, ITestClient> hubContext;

        public TestHostedService(
            IHostApplicationLifetime appLifetime,
            ILogger<TestHostedService> logger,
            IUI ui,
            IHubContext<TestHub, ITestClient> hubContext)
        {
            this.logger = logger;
            this.ui = ui;
            this.hubContext = hubContext;

            appLifetime.ApplicationStarted.Register(OnStarted);
            appLifetime.ApplicationStopping.Register(OnStopping);
            appLifetime.ApplicationStopped.Register(OnStopped);

            this.ui.SendToClient += async (m) =>
            {
                try
                {
                    await this.hubContext.Clients.All.SendToClient(m);
                }
                catch (System.Exception ex)
                {

                    this.logger.LogError($"Failed to send message to client - {ex.Message}");
                }
            };
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting ...");

            this.ui.Show();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Stop called");

            return Task.CompletedTask;
        }

        private void OnStarted()
        {
            logger.LogInformation("Started");
        }

        private void OnStopping()
        {
            logger.LogInformation("Stopping ...");
            if (this.ui != null)
            {
                this.ui.Dispose();
            }
        }

        private void OnStopped()
        {
            logger.LogInformation("Stopped");
        }
    }
}
