using MauiApp1.Web.Models;

namespace MauiApp1.Web.Services;

public interface IServicoService
{
    Task<PagedResult<Servico>> GetPagedAsync(FilterRequest filter);
    Task<Servico?> GetByIdAsync(string fontSgFonte, int servNrCodigo);
    Task<Servico> CreateAsync(Servico servico);
    Task<Servico> UpdateAsync(Servico servico);
    Task<bool> DeleteAsync(string fontSgFonte, int servNrCodigo);
    Task<IEnumerable<Fonte>> GetFontesAsync();
    Task<IEnumerable<GrupoServico>> GetGruposServicoAsync();
}
