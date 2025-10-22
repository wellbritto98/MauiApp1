namespace MauiApp1.Web.Models;

public class ServicoPreco
{
    public string FontSgFonte { get; set; } = string.Empty;
    public int ServNrCodigo { get; set; }
    public int PeriNrCodigo { get; set; }
    public decimal SerpVlPreco { get; set; }
    public DateTime SerpDtCadastro { get; set; }
    public string? SerpTxObservacao { get; set; }
}
