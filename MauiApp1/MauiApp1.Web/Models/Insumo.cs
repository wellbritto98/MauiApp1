namespace MauiApp1.Web.Models;

public class Insumo
{
    public string FontSgFonte { get; set; } = string.Empty;
    public int InsuNrCodigo { get; set; }
    public string InsuTxDescricao { get; set; } = string.Empty;
    public string InsuTxDescricaoColeta { get; set; } = string.Empty;
    public string InsuSgUnidade { get; set; } = string.Empty;
    public string InsuSgUnidadeColeta { get; set; } = string.Empty;
    public decimal InsuVlFatorConversao { get; set; }
    public int GpinNrCodigo { get; set; }
    public char InsuInCestaBasica { get; set; }
    public string? InsuTxResponsavel { get; set; }
    public DateTime InsuDtUltRevisao { get; set; }
    public DateTime InsuDtCadastro { get; set; }
    public char InsuInDesativado { get; set; }
    public string? FontSgFonteAnt { get; set; }
    public int? InsuNrCodigoAnt { get; set; }
    public DateTime? InsuDtUltColeta { get; set; }
    public byte[]? InsuTxDescricaoComplementar { get; set; }
}