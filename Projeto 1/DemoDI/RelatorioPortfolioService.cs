using System;
using System.Reflection;

namespace DemoDI
{
	public class RelatorioPortfolioService
	{
		private readonly Portfolio<IAtivoFinanceiro> _portfolio;
		private readonly ILogService _logService;
		public RelatorioPortfolioService(Portfolio<IAtivoFinanceiro> portfolio, ILogService logService)
		{
			_portfolio = portfolio ?? throw new ArgumentNullException(nameof(portfolio));
			_logService = logService ?? throw new ArgumentNullException(nameof(logService));
		}
		public void GerarRelatorioAtivos()
		{
			if (_portfolio.Ativos.Count == 0)
			{
				_logService.LogInfo("O portfólio está vazio. Nenhum relatório será gerado.");
				return;
			}
			Console.WriteLine("\n--- Relatório do Portfólio ---");
			foreach (var ativo in _portfolio.Ativos)
			{

				Console.WriteLine($"Valor Total do Portfólio: R$ {_portfolio.ValorTotalPortfolio():N2}");
				Console.WriteLine($"Rentabilidade Média Ponderada: {_portfolio.RentabilidadeMediaPonderada():N2}%");
				Console.WriteLine("-------------------------------\n");

				Type tipoAtivo = ativo.GetType();


				Console.WriteLine($"Ativo: {ativo.Nome}");
				Console.WriteLine($"  Nome da Classe (Type.Name):      {tipoAtivo.Name}");
				Console.WriteLine($"  Namespace Completo (FullName):   {tipoAtivo.FullName}");
				Console.WriteLine($"  Nome do Ativo (Propriedade):     {ativo.Nome}");
				Console.WriteLine($"  É uma Classe? (IsClass):         {tipoAtivo.IsClass}");
				Console.WriteLine($"  Implementa IAtivoFinanceiro?     {typeof(IAtivoFinanceiro).IsAssignableFrom(tipoAtivo)}");



				//propriedades básicas da Interface
				Console.WriteLine($"  Valor Investido: R$ {ativo.ValorInvestido:N2}");
				Console.WriteLine($"  Valor Atual: R$ {ativo.ValorAtual:N2}");
				Console.WriteLine($"  Peso no Portfólio: {_portfolio.PesoDoAtivo(ativo):P2}");

				//Invocando o método CalcularRentabilidade() da interface IAtivoFinanceiro
				Console.WriteLine($"  Rentabilidade: {ativo.CalcularRentabilidade():N2}%");


				// Extraindo Propriedades Específicas via Reflection (Dynamic Member Inspection)
				ExibirPropriedadeOpcional(tipoAtivo, ativo, "DataVencimento", "Data de Vencimento");
				ExibirPropriedadeOpcional(tipoAtivo, ativo, "DiasParaVencimento", "Dias Para Vencimento");
				ExibirPropriedadeOpcional(tipoAtivo, ativo, "Periodicidade", "Periodicidade");
				ExibirPropriedadeOpcional(tipoAtivo, ativo, "PrecoMercado", "Preço de Mercado");
				ExibirPropriedadeOpcional(tipoAtivo, ativo, "VariacaoDiaria", "Variação Diária");

				Console.WriteLine();



			}


		}
			// Método auxiliar para obter e formatar propriedades via Reflection
		private void ExibirPropriedadeOpcional(Type tipo, object instancia, string nomePropriedade, string rótulo)
		{
			PropertyInfo prop = tipo.GetProperty(nomePropriedade, BindingFlags.Public | BindingFlags.Instance);
			if (prop != null)
			{
				object valor = prop.GetValue(instancia);
				if (valor != null)
				{
					// Formatação amigável por tipo de dado (observe que não foi usado o switch baseado no tipo concereto do ativo, mas sim no tipo do valor retornado)
					string valorFormatado = valor switch
					{
						DateTime dt => dt.ToString("dd/MM/yyyy"),
						decimal dec => dec.ToString("N2"),
						double dbl => dbl.ToString("N2"),
						_ => valor.ToString()
					};

					Console.WriteLine($"  {rótulo}: {valorFormatado}");
				}
			}
		}


	}
	
}