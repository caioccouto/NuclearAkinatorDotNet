using NuclearAkinatorDotNet.Enums;
using NuclearAkinatorDotNet.Models;

namespace NuclearAkinatorDotNet.Validators;

public static class PerguntaValidator
{
    public static List<string> Validar(PerguntaEducativa pergunta, IEnumerable<PerguntaEducativa> existentes)
    {
        var erros = new List<string>();

        if (string.IsNullOrWhiteSpace(pergunta.Enunciado))
            erros.Add("O enunciado é obrigatório.");

        if (string.IsNullOrWhiteSpace(pergunta.AlternativaA))
            erros.Add("A alternativa A é obrigatória.");

        if (string.IsNullOrWhiteSpace(pergunta.AlternativaB))
            erros.Add("A alternativa B é obrigatória.");

        if (string.IsNullOrWhiteSpace(pergunta.AlternativaC))
            erros.Add("A alternativa C é obrigatória.");

        if (string.IsNullOrWhiteSpace(pergunta.AlternativaD))
            erros.Add("A alternativa D é obrigatória.");

        if (!RespostaValida(pergunta.RespostaCorreta))
            erros.Add("A resposta correta deve ser A, B, C ou D.");

        if (string.IsNullOrWhiteSpace(pergunta.ExplicacaoCientifica))
            erros.Add("A explicação científica é obrigatória.");

        if (!Enum.IsDefined(pergunta.Categoria))
            erros.Add("A categoria informada é inválida.");

        if (!Enum.IsDefined(pergunta.NivelDificuldade))
            erros.Add("O nível de dificuldade informado é inválido.");

        if (!string.IsNullOrWhiteSpace(pergunta.Enunciado) && EnunciadoDuplicado(pergunta.Enunciado, existentes))
            erros.Add("Já existe uma pergunta cadastrada com este enunciado.");

        return erros;
    }

    public static bool RespostaValida(string? resposta)
    {
        string valor = (resposta ?? string.Empty).Trim().ToUpperInvariant();
        return valor is "A" or "B" or "C" or "D";
    }

    public static bool EnunciadoDuplicado(string enunciado, IEnumerable<PerguntaEducativa> existentes)
    {
        string alvo = enunciado.Trim();
        return existentes.Any(p => string.Equals(p.Enunciado.Trim(), alvo, StringComparison.OrdinalIgnoreCase));
    }
}
