using NuclearAkinatorDotNet.Enums;
using NuclearAkinatorDotNet.Models;
using NuclearAkinatorDotNet.Repositories;
using NuclearAkinatorDotNet.Validators;

namespace NuclearAkinatorDotNet.Services;

public class PerguntaService
{
    private readonly PerguntaRepository _repositorio;

    public PerguntaService(PerguntaRepository repositorio)
    {
        _repositorio = repositorio;
    }

    public bool TentarCadastrar(PerguntaEducativa pergunta, out List<string> erros)
    {
        Normalizar(pergunta);

        erros = PerguntaValidator.Validar(pergunta, _repositorio.Listar());
        if (erros.Count > 0)
            return false;

        pergunta.Id = _repositorio.GerarProximoId();
        _repositorio.Incluir(pergunta);
        return true;
    }

    public bool TentarExcluir(int id, out string erro)
    {
        if (_repositorio.BuscarPorId(id) == null)
        {
            erro = $"Não existe pergunta com o ID {id}.";
            return false;
        }

        _repositorio.Excluir(id);
        erro = string.Empty;
        return true;
    }

    public IReadOnlyList<PerguntaEducativa> Listar() => _repositorio.Listar();

    public PerguntaEducativa? BuscarPorId(int id) => _repositorio.BuscarPorId(id);

    public List<PerguntaEducativa> BuscarPorCategoria(CategoriaEducativa categoria) =>
        _repositorio.BuscarPorCategoria(categoria);

    public List<PerguntaEducativa> BuscarPorNivel(NivelDificuldade nivel) =>
        _repositorio.BuscarPorNivel(nivel);

    private static void Normalizar(PerguntaEducativa pergunta)
    {
        pergunta.Enunciado = (pergunta.Enunciado ?? string.Empty).Trim();
        pergunta.AlternativaA = (pergunta.AlternativaA ?? string.Empty).Trim();
        pergunta.AlternativaB = (pergunta.AlternativaB ?? string.Empty).Trim();
        pergunta.AlternativaC = (pergunta.AlternativaC ?? string.Empty).Trim();
        pergunta.AlternativaD = (pergunta.AlternativaD ?? string.Empty).Trim();
        pergunta.RespostaCorreta = (pergunta.RespostaCorreta ?? string.Empty).Trim().ToUpperInvariant();
        pergunta.ExplicacaoCientifica = (pergunta.ExplicacaoCientifica ?? string.Empty).Trim();
    }
}
