namespace MauiApp1.Web.Models;

public class GrupoServico
{
    public int GpsrNrCodigo { get; set; }
    public string GpsrTxDescricao { get; set; } = string.Empty;
    public char GpsrInAtivo { get; set; }
    public DateTime GpsrDtCadastro { get; set; }
}
