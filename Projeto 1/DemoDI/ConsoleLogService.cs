namespace DemoDI
{
    public class ConsoleLogService : ILogService
    {
        private readonly IDateTimeProvider dateTimeProvider;
        public ConsoleLogService(IDateTimeProvider dateTimeProvider) => this.dateTimeProvider = dateTimeProvider;
        public void LogError(string mensagem) => Console.WriteLine($"{dateTimeProvider.Now} - ERROR: {mensagem}");
        public void LogInfo(string mensagem) => Console.WriteLine($"{dateTimeProvider.Now} - Info: {mensagem}");
    }
}
