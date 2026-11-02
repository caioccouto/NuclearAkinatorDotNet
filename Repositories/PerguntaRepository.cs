using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using NuclearAkinatorDotNet.Enums;
using NuclearAkinatorDotNet.Models;

namespace NuclearAkinatorDotNet.Repositories;

public class PerguntaRepository
{
    private const string NomeProjeto = "NuclearAkinatorDotNet";

    private static readonly JsonSerializerOptions OpcoesJson = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        Converters = { new JsonStringEnumConverter() }
    };

    private readonly string _caminhoArquivo;
    private List<PerguntaEducativa> _perguntas = new();

    public PerguntaRepository(string? caminhoArquivo = null)
    {
        _caminhoArquivo = caminhoArquivo ?? ResolverCaminhoPadrao();
    }

    public string CaminhoArquivo => _caminhoArquivo;

    public static string ResolverCaminhoPadrao()
    {
        var diretorio = new DirectoryInfo(AppContext.BaseDirectory);

        while (diretorio != null)
        {
            if (File.Exists(Path.Combine(diretorio.FullName, NomeProjeto + ".csproj")))
                return Path.Combine(diretorio.FullName, "dados", "perguntas.json");

            diretorio = diretorio.Parent;
        }

        string dadosUsuario = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        return Path.Combine(dadosUsuario, NomeProjeto, "perguntas.json");
    }

    public string? Carregar()
    {
        try
        {
            string? diretorio = Path.GetDirectoryName(_caminhoArquivo);
            if (!string.IsNullOrEmpty(diretorio))
                Directory.CreateDirectory(diretorio);

            if (!File.Exists(_caminhoArquivo))
            {
                _perguntas = new List<PerguntaEducativa>();
                Salvar();
                return null;
            }

            string conteudo = File.ReadAllText(_caminhoArquivo);

            if (string.IsNullOrWhiteSpace(conteudo))
            {
                _perguntas = new List<PerguntaEducativa>();
                return null;
            }

            var lidas = JsonSerializer.Deserialize<List<PerguntaEducativa>>(conteudo, OpcoesJson);

            if (lidas == null)
                throw new JsonException("O arquivo não contém uma lista de perguntas.");

            _perguntas = lidas.Where(p => p != null).ToList();
            return null;
        }
        catch (JsonException)
        {
            string backup = $"{_caminhoArquivo}.invalido-{DateTime.Now:yyyyMMddHHmmss}";
            File.Move(_caminhoArquivo, backup);
            _perguntas = new List<PerguntaEducativa>();
            Salvar();
            return $"O arquivo de dados estava com conteúdo inválido. Uma cópia foi guardada em: {backup}";
        }
    }

    public IReadOnlyList<PerguntaEducativa> Listar() => _perguntas.AsReadOnly();

    public PerguntaEducativa? BuscarPorId(int id) => _perguntas.FirstOrDefault(p => p.Id == id);

    public List<PerguntaEducativa> BuscarPorCategoria(CategoriaEducativa categoria) =>
        _perguntas.Where(p => p.Categoria == categoria).ToList();

    public List<PerguntaEducativa> BuscarPorNivel(NivelDificuldade nivel) =>
        _perguntas.Where(p => p.NivelDificuldade == nivel).ToList();

    public int GerarProximoId() => _perguntas.Count == 0 ? 1 : _perguntas.Max(p => p.Id) + 1;

    public void Incluir(PerguntaEducativa pergunta)
    {
        _perguntas.Add(pergunta);

        try
        {
            Salvar();
        }
        catch
        {
            _perguntas.Remove(pergunta);
            throw;
        }
    }

    public bool Excluir(int id)
    {
        var pergunta = BuscarPorId(id);
        if (pergunta == null)
            return false;

        int posicao = _perguntas.IndexOf(pergunta);
        _perguntas.RemoveAt(posicao);

        try
        {
            Salvar();
        }
        catch
        {
            _perguntas.Insert(posicao, pergunta);
            throw;
        }

        return true;
    }

    private void Salvar()
    {
        string json = JsonSerializer.Serialize(_perguntas, OpcoesJson);
        string temporario = _caminhoArquivo + ".tmp";

        File.WriteAllText(temporario, json);
        File.Move(temporario, _caminhoArquivo, true);
    }
}
