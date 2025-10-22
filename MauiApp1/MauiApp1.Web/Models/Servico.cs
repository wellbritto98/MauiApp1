namespace MauiApp1.Web.Models;

public class Servico
{
    public string FontSgFonte { get; set; } = string.Empty;
    public int ServNrCodigo { get; set; }
    public string ServTxDescricao { get; set; } = string.Empty;
    public string ServTxDescricaoColeta { get; set; } = string.Empty;
    public string ServSgUnidade { get; set; } = string.Empty;
    public string ServSgUnidadeColeta { get; set; } = string.Empty;
    public decimal ServVlFatorConversao { get; set; }
    public int GpsrNrCodigo { get; set; }
    public char ServInCestaBasica { get; set; }
    public string? ServTxResponsavel { get; set; }
    public DateTime ServDtUltRevisao { get; set; }
    public DateTime ServDtCadastro { get; set; }
    public char ServInDesativado { get; set; }
    public string? FontSgFonteAnt { get; set; }
    public int? ServNrCodigoAnt { get; set; }
    public DateTime? ServDtUltColeta { get; set; }
    public byte[]? ServTxDescricaoComplementar { get; set; }
}