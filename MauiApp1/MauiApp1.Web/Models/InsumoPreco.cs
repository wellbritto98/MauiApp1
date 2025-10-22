namespace MauiApp1.Web.Models;

public class InsumoPreco
{
    public string FontSgFonte { get; set; } = string.Empty;
    public int InsuNrCodigo { get; set; }
    public int PeriNrCodigo { get; set; }
    public decimal InprVlPreco { get; set; }
    public DateTime InprDtCadastro { get; set; }
    public string? InprTxObservacao { get; set; }
}
