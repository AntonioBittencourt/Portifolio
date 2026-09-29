namespace DemoDI
{
    public class Acao : IAtivoFinanceiro, IGeradorDeRenda, IAtivoNegociavel
    {

        private readonly ILogService logService;


        // Propriedades da classe Acao
        public int Quantidade { get; set; }
        public decimal PrecoMedioCompra { get; set; }
        public decimal DividendosRecebidos { get; set; }
        public decimal PrecoMercado { get; set; }
        public string Periodicidade { get; set; }
        public decimal VariacaoDiaria { get; set; }


        public string Nome { get; set; }


        //construtor de IlogService
        public Acao(ILogService logService)
        {
            this.logService = logService;
        }


        // Funções lambda para calcular ValorInvestido e ValorAtual
        public decimal ValorInvestido => Quantidade * PrecoMedioCompra;
        public decimal ValorAtual => Quantidade * PrecoMercado;

        // Implementação do método CalcularRentabilidade da interface IAtivoFinanceiro
        public decimal CalcularRentabilidade()
        {
            logService.LogInfo($"Calculando rentabilidade da ação {Nome}");


            if (ValorInvestido == 0)
            {
                logService.LogError($"Não é possível calcular a rentabilidade de {Nome}: Valor Investido é zero.");
                return 0m;
            }

            return (((ValorAtual + DividendosRecebidos - ValorInvestido)
        / ValorInvestido) * 100m);
        }



        // --- Método da interface IGeradorDeRenda ---
        public decimal CalcularRendaPeriodica()
        {
            logService.LogInfo($"Calculando renda periódica da ação {Nome}");
            return DividendosRecebidos;
        }
















    }
}


/*

public interface IAtivoFinanceiro
{
    string Nome { get; }
    decimal ValorInvestido { get; }
    decimal ValorAtual { get; }

    decimal CalcularRentabilidade();
}*/