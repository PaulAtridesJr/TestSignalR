namespace xemtest
{
    public interface IWorker {
        ExecuteResult Execute();
        ExecuteResult ExecuteWithParams(ExecuteData data);
    }
}