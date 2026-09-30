using System;


namespace DemoDI
{
    public class TituloRendaFixa : IAtivoFinanceiro, IAtivoComVencimento
    {

		private readonly ILogService logService;
		private readonly IDateTimeProvider dateTimeProvider;

		public string Nome { get; set; }
		public decimal ValorInvestido { get; set; }
		public decimal TaxaAnual { get; set; }
		public DateTime DataAplicacao { get; set; }
		public DateTime DataVencimento { get; set; }

		//construtor de IlogService
		public TituloRendaFixa(ILogService logService, IDateTimeProvider dateTimeProvider)
		{
			this.logService = logService;
			this.dateTimeProvider = dateTimeProvider;
		}



		//funções lambda
		public int DiasDecorridos => (dateTimeProvider.Now - DataAplicacao).Days;
		public decimal ValorAtual => ValorInvestido + (ValorInvestido * (CalcularRentabilidade() / 100m));

		public decimal CalcularRentabilidade()
		{
			logService.LogInfo($"Calculando rentabilidade da Título de renda fixa {Nome}");


			if (DiasDecorridos <= 0)
			{
				logService.LogError($"Não é possível calcular a rentabilidade de {Nome}: Número de dias decorridos é zero.");
				return 0m;
			}

			return ((TaxaAnual * DiasDecorridos) / 365m);
		}

		public int DiasParaVencimento()
		{
			logService.LogInfo($"Calculando dias para vencimento do Título de renda fixa {Nome}");
			return (DataVencimento - dateTimeProvider.Now).Days;
		}

		// private readonly IDateTimeProvider dateTimeProvider;
		//public ConsoleLogService(IDateTimeProvider dateTimeProvider) => this.dateTimeProvider = dateTimeProvider;
		//public void LogError(string mensagem) => Console.WriteLine($"{dateTimeProvider.Now} - ERROR: {mensagem}");
		//public void LogInfo(string mensagem) => Console.WriteLine($"{dateTimeProvider.Now} - Info: {mensagem}");
	}
}