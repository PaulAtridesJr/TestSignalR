using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace xemtest
{
	internal class TestHub : Hub<ITestClient>
	{
		private readonly ILogger<TestHub> logger;
        private readonly IWorker worker;

        public TestHub(ILogger<TestHub> logger, IWorker worker)
		{
			this.logger = logger;
            this.worker = worker;
        }

        public ExecuteResult ExecuteOnServer() 
        {
            this.logger.LogInformation($"Server received command 'ExecuteOnServer'");
			var result = this.worker.Execute();
			this.logger.LogInformation($"Result: {result?.Address}");
            return result;
        }

		public ExecuteResult ExecuteWithParamsOnServer(ExecuteData data) 
        {
			this.logger.LogInformation($"Server received command 'ExecuteWithParamsOnServer'. Params: {data?.Data} - {data?.ID}");
			var result = this.worker.ExecuteWithParams(data);
			this.logger.LogInformation($"Result: {result?.Address}");
            return result;
        }

		public override Task OnConnectedAsync()
		{
			this.logger.LogInformation($"User connected - {this.Context.ConnectionId}");
			return base.OnConnectedAsync();
		}

		public override Task OnDisconnectedAsync(Exception exception)
		{
			this.logger.LogInformation($"User disconnected - {this.Context.ConnectionId}. {(exception == null ? "" : exception.Message)}");
			return base.OnDisconnectedAsync(exception);
		}
	}
}
