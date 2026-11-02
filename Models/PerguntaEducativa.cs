using NuclearAkinatorDotNet.Enums;

namespace NuclearAkinatorDotNet.Models;

public class PerguntaEducativa
{
    public int Id { get; set; }
    public string Enunciado { get; set; } = string.Empty;
    public string AlternativaA { get; set; } = string.Empty;
    public string AlternativaB { get; set; } = string.Empty;
    public string AlternativaC { get; set; } = string.Empty;
    public string AlternativaD { get; set; } = string.Empty;
    public string RespostaCorreta { get; set; } = string.Empty;
    public string ExplicacaoCientifica { get; set; } = string.Empty;
    public CategoriaEducativa Categoria { get; set; }
    public NivelDificuldade NivelDificuldade { get; set; }
}
