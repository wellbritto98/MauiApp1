namespace MauiApp1.Web.Models;

public class GrupoInsumo
{
    public int GpinNrCodigo { get; set; }
    public string GpinTxDescricao { get; set; } = string.Empty;
    public char GpinInAtivo { get; set; }
    public DateTime GpinDtCadastro { get; set; }
}
