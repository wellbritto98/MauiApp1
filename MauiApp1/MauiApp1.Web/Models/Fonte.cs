namespace MauiApp1.Web.Models;

public class Fonte
{
    public string FontSgFonte { get; set; } = string.Empty;
    public string FontTxDescricao { get; set; } = string.Empty;
    public string? FontTxEndereco { get; set; }
    public string? FontTxTelefone { get; set; }
    public string? FontTxEmail { get; set; }
    public char FontInAtivo { get; set; }
    public DateTime FontDtCadastro { get; set; }
}
