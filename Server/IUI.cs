namespace xemtest
{
    public interface IUI : IDisposable
    {
        event Action<MessageToClient> SendToClient;
        void Show();
    }
}