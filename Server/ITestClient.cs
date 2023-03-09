namespace xemtest
{
    public interface ITestClient
    {
        Task SendToClient(MessageToClient message);
    }
}