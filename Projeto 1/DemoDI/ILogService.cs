namespace DemoDI
{
    public interface ILogService
    {
        void LogError(string mensagem);
        void LogInfo(string mensagem);
    }
}
