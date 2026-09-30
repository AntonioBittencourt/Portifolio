namespace DemoDI
{
    public class FundoInvestimento : IAtivoFinanceiro, IGeradorDeRenda
    {

        private readonly ILogService logService;

        public string Nome { get; set; }
        public decimal QuantidadeCotas{ get; set; }
        public decimal ValorCotaCompra { get; set; }
        public decimal ValorCotaAtual { get; set; }
        public decimal TaxaAdministracao { get; set; }
        public decimal RendimentoPorCota { get; set; }
        public string Periodicidade { get; set; }



        //construtor de IlogService
        public FundoInvestimento(ILogService logService)
        {
            this.logService = logService;
        }


        public decimal ValorInvestido => QuantidadeCotas * ValorCotaCompra;
        public decimal ValorAtual => QuantidadeCotas * ValorCotaAtual;


        // Implementação do método CalcularRentabilidade da interface IAtivoFinanceiro
        public decimal CalcularRentabilidade()
        {
            logService.LogInfo($"Calculando rentabilidade do fundo de investimento {Nome}");


            if (ValorCotaCompra == 0)
            {
                logService.LogError($"Não é possível calcular a rentabilidade de {Nome}: Valor de cota de compra é zero.");
                return 0m;
            }

            return (((ValorCotaAtual - ValorCotaCompra)
        / ValorCotaCompra) * 100m);
        }

        public decimal CalcularRendaPeriodica()
        {
            logService.LogInfo($"Calculando renda periódica do fundo de investimento {Nome}");
            return (QuantidadeCotas * RendimentoPorCota);
        }



        // private readonly IDateTimeProvider dateTimeProvider;
        //public ConsoleLogService(IDateTimeProvider dateTimeProvider) => this.dateTimeProvider = dateTimeProvider;
        //public void LogError(string mensagem) => Console.WriteLine($"{dateTimeProvider.Now} - ERROR: {mensagem}");
        //public void LogInfo(string mensagem) => Console.WriteLine($"{dateTimeProvider.Now} - Info: {mensagem}");
    }
}