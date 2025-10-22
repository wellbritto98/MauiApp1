namespace MauiApp1.Web.Models;

public class Periodo
{
    public int PeriNrCodigo { get; set; }
    public string PeriTxDescricao { get; set; } = string.Empty;
    public DateTime PeriDtInicio { get; set; }
    public DateTime PeriDtFim { get; set; }
    public char PeriInAtivo { get; set; }
    public DateTime PeriDtCadastro { get; set; }
}
