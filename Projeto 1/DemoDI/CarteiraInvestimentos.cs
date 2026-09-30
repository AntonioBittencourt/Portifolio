namespace DemoDI
{
    public class CarteiraInvestimentos : ICarteiraInvestimentos
    {
        private readonly List<IAtivoFinanceiro> _ativos = new();
        private readonly ILogService _logService;


        public IReadOnlyCollection<IAtivoFinanceiro> Ativos => _ativos.AsReadOnly();

        public CarteiraInvestimentos(ILogService logService)
        {
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));
        }

        public void AdicionarAtivo(IAtivoFinanceiro ativo)
        {
            if (ativo == null) return;
            _ativos.Add(ativo);
            _logService.LogInfo($"Ativo {ativo.Nome} adicionado à carteira.");
        }

        public decimal ValorTotalPortfolio() => _ativos.Sum(a => a.ValorAtual);
        public decimal PesoDoAtivo(IAtivoFinanceiro ativo)
        {
            var valorTotal = ValorTotalPortfolio();
            if (valorTotal == 0) return 0m;
            return ativo.ValorAtual / valorTotal;
        }



        public decimal RentabilidadeMediaPonderada()
        {
            _logService.LogInfo("Calculando rentabilidade média ponderada da carteira.");

            var valorTotal = ValorTotalPortfolio();

            if (valorTotal == 0)
            {
                _logService.LogError("Não foi possível calcular a rentabilidade: Valor total da carteira é zero.");
                return 0m;
            }

            return _ativos.Sum(a => a.CalcularRentabilidade() * a.ValorAtual) / valorTotal;
        }

    }
}