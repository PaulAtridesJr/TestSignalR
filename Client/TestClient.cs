using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace xemtestclient
{
    public class TestClient : ITestClient
    {
        private readonly ILogger<TestClient> logger;
        HubConnection connection;

        public TestClient(ILogger<TestClient> logger)
        {
            this.logger = logger;
        }

        public async Task Connect()
        {
            try
            {
                connection = new HubConnectionBuilder()
                    .WithUrl("http://localhost:9123/Test")
                    .WithAutomaticReconnect()
                    .ConfigureLogging(logging =>
                    {
                        logging.AddConsole();
                        logging.SetMinimumLevel(LogLevel.Debug);
                        logging.AddSerilog();
                    })
                    .AddJsonProtocol(options =>
                    {
                        options.PayloadSerializerOptions.Converters.Add(new IntPtrConverter());
                    })
                    .Build();

                connection.Closed += async (error) =>
                {
                    await Task.Delay(new Random().Next(0, 5) * 1000);
                    await connection.StartAsync();
                };
                connection.Reconnecting += error =>
                {
                    logger.LogWarning($"Reconnecting - {error}");
                    return Task.CompletedTask;
                };
                connection.Reconnected += connectionId =>
                {
                    logger.LogInformation($"Reconnected - {connectionId}");
                    return Task.CompletedTask;
                };

                connection.On<MessageToClient>("SendToClient", (msg) =>
                {
                    logger.LogInformation($"From server - {msg?.ID} {msg?.Name}");
                });

                await connection.StartAsync();
                logger.LogInformation("Connected");
            }
            catch (Exception ex)
            {
                logger.LogError($"Failed to connect - {ex.Message}");
            }
        }


        public async Task SendCommandToServer()
        {
            ExecuteResult result = null;
            try
            {
                result = await connection.InvokeAsync<ExecuteResult>("ExecuteOnServer");
            }
            catch (Exception ex)
            {
                logger.LogError($"Failed to execute - {ex.Message}");
            }

            if (result != null)
            {
                logger.LogInformation($"Answer from server - {result.Address}");

                // throws uncatchable AccessViolation
                //try
                //{
                //    IntPtr ptr = result.Address;
                //    byte[] arr = new byte[10];
                //    for (int i = 0; i < 10; i++)
                //   {
                //        arr[i] = Marshal.ReadByte(ptr, i);
                //    }
                //    logger.LogInformation($"Data read - '{string.Join(" ", arr.Select(s => $"{s:X2}"))}'");
                //}
                //catch (System.Exception ex)
                //{
                //    logger.LogError($"Failed to read from memory - {ex.Message}");

                //}

            }
        }

        public async Task SendCommandToServerWithParams()
        {
            ExecuteResult result = null;
            try
            {
                result = await connection.InvokeAsync<ExecuteResult>("ExecuteWithParamsOnServer", new ExecuteData() { ID = 22, Data = "clnt" });
            }
            catch (Exception ex)
            {
                logger.LogError($"Failed to execute - {ex.Message}");
            }

            if (result != null)
            {
                logger.LogInformation($"Answer from server - {result.Address}");
            }
        }

        public Task StopAsync()
        {
            logger.LogInformation("Stopping client ...");

            var res = connection.StopAsync().WaitAsync(new TimeSpan(0, 0, 10));
            logger.LogInformation("Client stopped");

            return res;
        }
    }
}