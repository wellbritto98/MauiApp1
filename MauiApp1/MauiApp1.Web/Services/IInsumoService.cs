using MauiApp1.Web.Models;

namespace MauiApp1.Web.Services;

public interface IInsumoService
{
    Task<PagedResult<Insumo>> GetPagedAsync(FilterRequest filter);
    Task<Insumo?> GetByIdAsync(string fontSgFonte, int insuNrCodigo);
    Task<Insumo> CreateAsync(Insumo insumo);
    Task<Insumo> UpdateAsync(Insumo insumo);
    Task<bool> DeleteAsync(string fontSgFonte, int insuNrCodigo);
    Task<IEnumerable<Fonte>> GetFontesAsync();
    Task<IEnumerable<GrupoInsumo>> GetGruposInsumoAsync();
}
