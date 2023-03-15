namespace xemtest
{
    public interface ITestClient
    {
        Task SendToClient(MessageToClient message);
        Task ReceiveMessage(string text);
    }
}