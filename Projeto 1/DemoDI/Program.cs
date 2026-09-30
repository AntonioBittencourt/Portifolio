using Microsoft.Extensions.DependencyInjection;

namespace DemoDI
{

    public interface IAtivoFinanceiro
    {
        string Nome { get; }
        decimal ValorInvestido { get; }
        decimal ValorAtual { get; }

        decimal CalcularRentabilidade();
    }

    // Interface específica: ativos que possuem data de vencimento
    public interface IAtivoComVencimento
    {
        DateTime DataVencimento { get; }
        int DiasParaVencimento();
    }

    // Interface específica: ativos que geram renda periódica
    // como dividendos, distribuições etc.
    public interface IGeradorDeRenda
    {
        decimal CalcularRendaPeriodica();
        string Periodicidade { get; } // Ex.: "Mensal", "Trimestral", "Anual"
    }

    // Interface específica: ativos negociáveis em mercado
    public interface IAtivoNegociavel
    {
        decimal PrecoMercado { get; }
        decimal VariacaoDiaria { get; } // percentual de variação do dia
    }


    public interface ICarteiraInvestimentos
    {
        IReadOnlyCollection<IAtivoFinanceiro> Ativos { get; }
        void AdicionarAtivo(IAtivoFinanceiro ativo);
        decimal ValorTotalPortfolio();
        decimal RentabilidadeMediaPonderada();
    }

    // --- Interfaces e Classes de Log / DI ---
    /*  public interface IDateTimeProvider
      {
          DateTime Now { get; }
      } */

    public class SystemDateTimeProvider : IDateTimeProvider
    {
        public DateTime Now => DateTime.Now;
    }

  /*  public interface ILogService
    {
        void LogError(string mensagem);
        void LogInfo(string mensagem);
    }
  
 */

  /*  public class ConsoleLogService : ILogService
    {
        private readonly IDateTimeProvider dateTimeProvider;
        public ConsoleLogService(IDateTimeProvider dateTimeProvider) => this.dateTimeProvider = dateTimeProvider;

        public void LogError(string mensagem) => Console.WriteLine($"{dateTimeProvider.Now} - ERROR: {mensagem}");
        public void LogInfo(string mensagem) => Console.WriteLine($"{dateTimeProvider.Now} - Info: {mensagem}");
    } */




    public interface ILogServiceBase
    {
        public Guid Id { get; }
        public ServiceLifetime LifeTime { get; }
    }
    public interface ILogServiceTransient : ILogServiceBase { }
    public class ExampleTransientService : ILogServiceTransient
    {
        public Guid Id { get; }
        public ExampleTransientService() => Id = Guid.NewGuid();
        public ServiceLifetime LifeTime => ServiceLifetime.Transient;
    }

    public interface ILogServiceScoped : ILogServiceBase { }
    public class ExampleScopedService : ILogServiceScoped
    {
        public Guid Id { get; }
        public ExampleScopedService() => Id = Guid.NewGuid();
        public ServiceLifetime LifeTime => ServiceLifetime.Scoped;
    }

    public interface ILogServiceSingleton : ILogServiceBase { }
    public class ExampleSingletonService : ILogServiceSingleton
    {
        public Guid Id { get; }
        public ExampleSingletonService() => Id = Guid.NewGuid();
        public ServiceLifetime LifeTime => ServiceLifetime.Singleton;

    }

    public class ProcessarPagamento(ILogServiceTransient transient,
                                    ILogServiceScoped scoped, 
                                    ILogServiceSingleton singleton)
    {
        public void ImprimeIds(string detalhes)
        {
            LogService(transient, $"{detalhes} - Serviço Transient\t\t- Sempre Diferente\t\t\t");
            LogService(scoped, $"{detalhes} - Serviço Scoped\t\t\t- Muda a cada Escopo\t\t\t");
            LogService(singleton, $"{detalhes} - Serviço Singleton\t\t- Sempre Igual/Nunca Muda\t\t");
            Console.WriteLine();
        }

        private void LogService(ILogServiceBase service, string escopo) => Console.WriteLine($"{escopo} - {service.Id}");
    }


    internal class Program
    {
        /* static void Main(string[] args)
         {
             var collection = new ServiceCollection();

             collection.AddSingleton<ILogServiceSingleton, ExampleSingletonService>();
             collection.AddScoped<ILogServiceScoped, ExampleScopedService>();
             collection.AddTransient<ILogServiceTransient, ExampleTransientService>();

             collection.AddTransient<ProcessarPagamento>();

             var provider = collection.BuildServiceProvider();

             ExecutaTeste(provider, "Requisição do Paulo");
             ExecutaTeste(provider, "Requisição do João");
         } */


        static void Main(string[] args)
        {
            var collection = new ServiceCollection();

            // 1. Registro de Infraestrutura
            collection.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();
            collection.AddTransient<ILogService, ConsoleLogService>();

            // 2. Registro dos Exceções de Tempo de Vida
            collection.AddSingleton<ILogServiceSingleton, ExampleSingletonService>();
            collection.AddScoped<ILogServiceScoped, ExampleScopedService>();
            collection.AddTransient<ILogServiceTransient, ExampleTransientService>();
            collection.AddTransient<ProcessarPagamento>();

            // 3. Registro dos Ativos Financeiros
            collection.AddTransient<Acao>();
            collection.AddTransient<TituloRendaFixa>();
            collection.AddTransient<FundoInvestimento>();
            collection.AddTransient<ICarteiraInvestimentos, CarteiraInvestimentos>();
            collection.AddTransient<RelatorioPortfolioService>();
            collection.AddScoped(typeof(Portfolio<>));
           // collection.AddTransient(typeof(Portfolio<>));

            var provider = collection.BuildServiceProvider();

            // Executa testes dos Escopos do DI
            // ExecutaTeste(provider, "Requisição do Paulo");
            // ExecutaTeste(provider, "Requisição do João");

            // Executa simulação da Carteira de Ativos
            TestarCarteiraDeAtivos(provider);

            Console.WriteLine("\nPressione ENTER para encerrar...");
            Console.ReadLine();
        }

        private static void ExecutaTeste(ServiceProvider container, string nomeEscopo)
        {
            using var escopo = container.CreateScope();
            var provider = escopo.ServiceProvider;

            var instancia1 = provider.GetRequiredService<ProcessarPagamento>();
            //var instancia2 = provider.GetRequiredService<ProcessarPagamento>();
            //var instancia3 = provider.GetRequiredService<ProcessarPagamento>();

            instancia1.ImprimeIds($"{nomeEscopo} - Primeira Instância de ProcessarPagamento");
            //instancia2.ImprimeIds($"{nomeEscopo} - Segunda Instância de ProcessarPagamento");
            //instancia3.ImprimeIds($"{nomeEscopo} - Terceira Instância de ProcessarPagamento");



            Console.WriteLine("\nPressione ENTER para encerrar...");
            Console.WriteLine("\nNova mensagem...");
            //Console.WriteLine("\nQualquer Merda");
            Console.ReadLine();

        }

        private static void TestarCarteiraDeAtivos(IServiceProvider provider)
        {
            Console.WriteLine("\n=================== Portifolio de Investimentos ===================");


            //var carteira = provider.GetRequiredService<ICarteiraInvestimentos>();


            var portfolio = provider.GetRequiredService<Portfolio<IAtivoFinanceiro>>();

            // Criando as instâncias resolvidas pelo DI
            var acao = provider.GetRequiredService<Acao>();
            acao.Nome = "PETR4";
            acao.Quantidade = 100;
            acao.PrecoMedioCompra = 30.00m;
            acao.PrecoMercado = 35.00m;
            acao.DividendosRecebidos = 150.00m;

            var titulo = provider.GetRequiredService<TituloRendaFixa>();
            titulo.Nome = "CDB Banco X";
            titulo.ValorInvestido = 5000.00m;
            titulo.TaxaAnual = 12.0m;
            titulo.DataAplicacao = DateTime.Now.AddDays(-180);
            titulo.DataVencimento = DateTime.Now.AddDays(185);

            var fundo = provider.GetRequiredService<FundoInvestimento>();
            fundo.Nome = "FII HGLG11";
            fundo.QuantidadeCotas = 50;
            fundo.ValorCotaCompra = 160.00m;
            fundo.ValorCotaAtual = 165.00m;
            fundo.RendimentoPorCota = 1.10m;

            // Lista polimórfica usando a interface comum IAtivoFinanceiro
            //var carteira = new List<IAtivoFinanceiro> { acao, titulo, fundo };

            /*
            carteira.AdicionarAtivo(acao);
            carteira.AdicionarAtivo(titulo);
            carteira.AdicionarAtivo(fundo);
            */

            // Adicionando os ativos no portifolio
            portfolio.AdicionarAtivo(acao);
            portfolio.AdicionarAtivo(titulo);
            portfolio.AdicionarAtivo(fundo);

            
            Console.WriteLine("\n---------------------------------------------------------");
            Console.WriteLine($"\nValor Total: R$ {portfolio.ValorTotalPortfolio():N2}");
            // Console.WriteLine($"\nValor Total da Carteira: R$ {carteira.PesoDoAtivo():N2}");
            Console.WriteLine($"Rentabilidade Média Ponderada: {portfolio.RentabilidadeMediaPonderada():N2}%");
            Console.WriteLine("---------------------------------------------------------");





            foreach (var ativo in portfolio.Ativos)
            {
                Console.WriteLine($"\nAtivo: {ativo.Nome}");
                Console.WriteLine($"  Valor Investido: R$ {ativo.ValorInvestido:N2}");
                Console.WriteLine($"  Valor Atual: R$ {ativo.ValorAtual:N2}");
                Console.WriteLine($"  Rentabilidade: {ativo.CalcularRentabilidade():N2}%");

            }


            // Exemplo de uso do método FiltrarPor (Ex: Ativos com rentabilidade positiva > 10%)
            Console.WriteLine("\n--- Ativos com Rentabilidade Maior que 10% ---");
            var ativosRentaveis = portfolio.FiltrarPor(a => a.CalcularRentabilidade() > 10.0m);
            foreach (var a in ativosRentaveis)
            {
                Console.WriteLine($" - {a.Nome}: {a.CalcularRentabilidade():N2}%");
            }


            var relatorioService = provider.GetRequiredService<RelatorioPortfolioService>();
            relatorioService.GerarRelatorioAtivos();




        }


    }
}
