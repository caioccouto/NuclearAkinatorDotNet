namespace NuclearAkinatorDotNet.Enums;

public static class EnumExtensoes
{
    public static string Descricao(this CategoriaEducativa categoria) => categoria switch
    {
        CategoriaEducativa.RadioatividadeNatural => "Radioatividade natural",
        CategoriaEducativa.RadiacaoIonizante => "Radiação ionizante",
        CategoriaEducativa.RadiacaoNaoIonizante => "Radiação não ionizante",
        CategoriaEducativa.MedicinaNuclear => "Medicina nuclear",
        CategoriaEducativa.AplicacoesTecnologiaNuclear => "Aplicações da tecnologia nuclear",
         => categoria.ToString()
    };

    public static string Descricao(this NivelDificuldade nivel) => nivel switch
    {
        NivelDificuldade.Facil => "Fácil",
        NivelDificuldade.Medio => "Médio",
        NivelDificuldade.Dificil => "Difícil",
         => nivel.ToString()
    };
}