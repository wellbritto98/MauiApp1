using MauiApp1.Web.Models;

namespace MauiApp1.Web.Services;

public interface IInsumoService
{
    Task<PagedResult<Insumo>> GetPagedAsync(FilterRequest filter);
    Task<Insumo?> GetByIdAsync(int id);
    Task<Insumo> CreateAsync(Insumo insumo);
    Task<Insumo> UpdateAsync(Insumo insumo);
    Task<bool> DeleteAsync(int id);
}