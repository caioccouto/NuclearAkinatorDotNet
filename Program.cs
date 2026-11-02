using System.Text;
using NuclearAkinatorDotNet.Enums;
using NuclearAkinatorDotNet.Models;
using NuclearAkinatorDotNet.Repositories;
using NuclearAkinatorDotNet.Services;
using NuclearAkinatorDotNet.Validators;

namespace NuclearAkinatorDotNet;

public class Program
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        var repositorio = new PerguntaRepository();

        try
        {
            string? aviso = repositorio.Carregar();
            if (aviso != null)
                Console.WriteLine($"\nAviso: {aviso}");
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            Console.WriteLine($"\nNão foi possível acessar o arquivo de dados ({repositorio.CaminhoArquivo}).");
            Console.WriteLine($"Detalhe: {ex.Message}");
            Console.WriteLine("Verifique as permissões da pasta e execute o programa novamente.");
            return;
        }

        var servico = new PerguntaService(repositorio);
        bool executando = true;

        while (executando)
        {
            ExibirMenu();
            string opcao = LerLinha("Escolha uma opção: ").Trim();

            try
            {
                switch (opcao)
                {
                    case "1": CadastrarPergunta(servico); break;
                    case "2": ListarPerguntas(servico); break;
                    case "3": BuscarPorId(servico); break;
                    case "4": BuscarPorCategoria(servico); break;
                    case "5": BuscarPorDificuldade(servico); break;
                    case "6": ExcluirPergunta(servico); break;
                    case "7":
                        executando = false;
                        Console.WriteLine("\nEncerrando o gerenciador. Até logo!");
                        break;
                    default:
                        Console.WriteLine("\nOpção inválida. Digite um número de 1 a 7.");
                        break;
                }
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
            {
                Console.WriteLine("\nErro ao gravar os dados. A operação NÃO foi concluída.");
                Console.WriteLine($"Detalhe: {ex.Message}");
            }
        }
    }

    private static void ExibirMenu()
    {
        Console.WriteLine();
        Console.WriteLine("=== NUCLEAR AKINATOR ===");
        Console.WriteLine("GERENCIADOR DE PERGUNTAS EDUCATIVAS");
        Console.WriteLine();
        Console.WriteLine("1. Cadastrar pergunta");
        Console.WriteLine("2. Listar perguntas");
        Console.WriteLine("3. Buscar por ID");
        Console.WriteLine("4. Buscar por categoria");
        Console.WriteLine("5. Buscar por dificuldade");
        Console.WriteLine("6. Excluir pergunta");
        Console.WriteLine("7. Sair");
        Console.WriteLine();
    }

    private static void CadastrarPergunta(PerguntaService servico)
    {
        Console.WriteLine("\n--- Cadastro de pergunta ---");

        var pergunta = new PerguntaEducativa
        {
            Enunciado = LerLinha("Enunciado: "),
            AlternativaA = LerLinha("Alternativa A: "),
            AlternativaB = LerLinha("Alternativa B: "),
            AlternativaC = LerLinha("Alternativa C: "),
            AlternativaD = LerLinha("Alternativa D: "),
            RespostaCorreta = LerResposta(),
            ExplicacaoCientifica = LerLinha("Explicação científica: "),
            Categoria = LerCategoria(),
            NivelDificuldade = LerDificuldade()
        };

        if (servico.TentarCadastrar(pergunta, out var erros))
        {
            Console.WriteLine($"\nPergunta cadastrada com sucesso! ID gerado: {pergunta.Id}");
            return;
        }

        Console.WriteLine("\nA pergunta não foi cadastrada. Corrija os problemas abaixo:");
        foreach (var erro in erros)
            Console.WriteLine($" - {erro}");
    }

    private static void ListarPerguntas(PerguntaService servico)
    {
        var perguntas = servico.Listar();

        if (perguntas.Count == 0)
        {
            Console.WriteLine("\nNenhuma pergunta cadastrada.");
            return;
        }

        Console.WriteLine($"\n--- Perguntas cadastradas ({perguntas.Count}) ---");
        foreach (var pergunta in perguntas)
            ExibirResumo(pergunta);
    }

    private static void BuscarPorId(PerguntaService servico)
    {
        if (!LerInteiro("ID da pergunta: ", out int id))
            return;

        var pergunta = servico.BuscarPorId(id);

        if (pergunta == null)
        {
            Console.WriteLine($"\nNenhuma pergunta encontrada com o ID {id}.");
            return;
        }

        ExibirDetalhes(pergunta);
    }

    private static void BuscarPorCategoria(PerguntaService servico)
    {
        var categoria = LerCategoria();
        var resultado = servico.BuscarPorCategoria(categoria);

        ExibirResultadoBusca(resultado, $"na categoria \"{categoria.Descricao()}\"");
    }

    private static void BuscarPorDificuldade(PerguntaService servico)
    {
        var nivel = LerDificuldade();
        var resultado = servico.BuscarPorNivel(nivel);

        ExibirResultadoBusca(resultado, $"com dificuldade \"{nivel.Descricao()}\"");
    }

    private static void ExcluirPergunta(PerguntaService servico)
    {
        if (!LerInteiro("ID da pergunta a excluir: ", out int id))
            return;

        var pergunta = servico.BuscarPorId(id);

        if (pergunta == null)
        {
            Console.WriteLine($"\nNão existe pergunta com o ID {id}.");
            return;
        }

        ExibirDetalhes(pergunta);

        string confirmacao = LerLinha("\nConfirma a exclusão desta pergunta? (S/N): ").Trim().ToUpperInvariant();
        if (confirmacao != "S")
        {
            Console.WriteLine("\nExclusão cancelada.");
            return;
        }

        if (servico.TentarExcluir(id, out string erro))
            Console.WriteLine($"\nPergunta {id} excluída com sucesso.");
        else
            Console.WriteLine($"\n{erro}");
    }

    private static void ExibirResultadoBusca(List<PerguntaEducativa> resultado, string filtro)
    {
        if (resultado.Count == 0)
        {
            Console.WriteLine($"\nNenhuma pergunta encontrada {filtro}.");
            return;
        }

        Console.WriteLine($"\n--- {resultado.Count} pergunta(s) encontrada(s) {filtro} ---");
        foreach (var pergunta in resultado)
            ExibirResumo(pergunta);
    }

    private static void ExibirResumo(PerguntaEducativa pergunta)
    {
        Console.WriteLine();
        Console.WriteLine($"[ID {pergunta.Id}] {pergunta.Enunciado}");
        Console.WriteLine($"    Categoria: {pergunta.Categoria.Descricao()} | Dificuldade: {pergunta.NivelDificuldade.Descricao()}");
    }

    private static void ExibirDetalhes(PerguntaEducativa pergunta)
    {
        Console.WriteLine();
        Console.WriteLine($"ID: {pergunta.Id}");
        Console.WriteLine($"Enunciado: {pergunta.Enunciado}");
        Console.WriteLine($"  A) {pergunta.AlternativaA}");
        Console.WriteLine($"  B) {pergunta.AlternativaB}");
        Console.WriteLine($"  C) {pergunta.AlternativaC}");
        Console.WriteLine($"  D) {pergunta.AlternativaD}");
        Console.WriteLine($"Resposta correta: {pergunta.RespostaCorreta}");
        Console.WriteLine($"Explicação científica: {pergunta.ExplicacaoCientifica}");
        Console.WriteLine($"Categoria: {pergunta.Categoria.Descricao()}");
        Console.WriteLine($"Dificuldade: {pergunta.NivelDificuldade.Descricao()}");
    }

    private static string LerLinha(string mensagem)
    {
        Console.Write(mensagem);
        string? linha = Console.ReadLine();

        if (linha == null)
        {
            Console.WriteLine("\nEntrada encerrada. Finalizando o programa.");
            Environment.Exit(0);
        }

        return linha;
    }

    private static bool LerInteiro(string mensagem, out int valor)
    {
        if (int.TryParse(LerLinha(mensagem).Trim(), out valor))
            return true;

        Console.WriteLine("\nValor inválido. Informe um número inteiro.");
        return false;
    }

    private static string LerResposta()
    {
        while (true)
        {
            string resposta = LerLinha("Resposta correta (A, B, C ou D): ").Trim();

            if (PerguntaValidator.RespostaValida(resposta))
                return resposta;

            Console.WriteLine("Resposta inválida. Digite A, B, C ou D.");
        }
    }

    private static CategoriaEducativa LerCategoria()
    {
        Console.WriteLine("\nCategorias:");
        foreach (var categoria in Enum.GetValues<CategoriaEducativa>())
            Console.WriteLine($"  {(int)categoria}. {categoria.Descricao()}");

        while (true)
        {
            string entrada = LerLinha("Escolha a categoria: ").Trim();

            if (int.TryParse(entrada, out int numero) && Enum.IsDefined(typeof(CategoriaEducativa), numero))
                return (CategoriaEducativa)numero;

            Console.WriteLine("Categoria inválida. Escolha um dos números listados.");
        }
    }

    private static NivelDificuldade LerDificuldade()
    {
        Console.WriteLine("\nNíveis de dificuldade:");
        foreach (var nivel in Enum.GetValues<NivelDificuldade>())
            Console.WriteLine($"  {(int)nivel}. {nivel.Descricao()}");

        while (true)
        {
            string entrada = LerLinha("Escolha o nível: ").Trim();

            if (int.TryParse(entrada, out int numero) && Enum.IsDefined(typeof(NivelDificuldade), numero))
                return (NivelDificuldade)numero;

            Console.WriteLine("Nível inválido. Escolha um dos números listados.");
        }
    }
}
