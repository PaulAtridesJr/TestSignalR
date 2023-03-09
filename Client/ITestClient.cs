namespace xemtestclient
{
    public interface ITestClient
    {
        Task Connect();
        Task SendCommandToServer();
        Task SendCommandToServerWithParams();
        Task StopAsync();
    }
}