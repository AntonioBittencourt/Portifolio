using System;
using System.Collections.Generic;
using System.Linq;



namespace DemoDI

{/*
	public interface ICarteiraInvestimentos
	{
		IReadOnlyCollection<IAtivoFinanceiro> Ativos { get; }
		void AdicionarAtivo(IAtivoFinanceiro ativo);
		decimal ValorTotalPortfolio();
		decimal RentabilidadeMediaPonderada();
	}
	*/


	public class Portfolio<T> where T : IAtivoFinanceiro
	{
		private readonly List<T> _ativos = new();
		private readonly ILogService _logService;
		public IReadOnlyCollection<T> Ativos => _ativos.AsReadOnly();
		public Portfolio(ILogService logService)
		{
			_logService = logService ?? throw new ArgumentNullException(nameof(logService));
		}
		public void AdicionarAtivo(T ativo)
		{
			if (ativo == null) return;
			_ativos.Add(ativo);
			_logService.LogInfo($"Ativo {ativo.Nome} adicionado ao portfólio.");
		}
		public decimal ValorTotalPortfolio() => _ativos.Sum(a => a.ValorAtual);



		public decimal PesoDoAtivo(T ativo)
		{
			var valorTotal = ValorTotalPortfolio();
			if (valorTotal == 0) return 0m;
			return ativo.ValorAtual / valorTotal;
		}


		public decimal RentabilidadeMediaPonderada()
		{
			var valorTotal = ValorTotalPortfolio();
			if (valorTotal == 0) return 0m;
			return _ativos.Sum(a => a.CalcularRentabilidade() * a.ValorAtual) / valorTotal;
		}


		public IEnumerable<T> FiltrarPor(Func<T, bool> predicado)
		{
			if (predicado == null) return _ativos;
			return _ativos.Where(predicado);
		}

	}

}