using Microsoft.AspNetCore.Mvc;
using MauiApp1.Web.Models;
using MauiApp1.Web.Services;

namespace MauiApp1.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InsumosController : ControllerBase
{
    private readonly IInsumoService _insumoService;

    public InsumosController(IInsumoService insumoService)
    {
        _insumoService = insumoService;
    }

    [HttpGet("paged")]
    public async Task<ActionResult<PagedResult<Insumo>>> GetPaged([FromQuery] FilterRequest filter)
    {
        try
        {
            var result = await _insumoService.GetPagedAsync(filter);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro interno: {ex.Message}");
        }
    }

    [HttpGet("{fontSgFonte}/{insuNrCodigo}")]
    public async Task<ActionResult<Insumo>> GetById(string fontSgFonte, int insuNrCodigo)
    {
        try
        {
            var insumo = await _insumoService.GetByIdAsync(fontSgFonte, insuNrCodigo);
            if (insumo == null)
                return NotFound();

            return Ok(insumo);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro interno: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<ActionResult<Insumo>> Create([FromBody] Insumo insumo)
    {
        try
        {
            insumo.InsuDtCadastro = DateTime.UtcNow;
            insumo.InsuDtUltRevisao = DateTime.UtcNow;
            var createdInsumo = await _insumoService.CreateAsync(insumo);
            return CreatedAtAction(nameof(GetById), new { fontSgFonte = createdInsumo.FontSgFonte, insuNrCodigo = createdInsumo.InsuNrCodigo }, createdInsumo);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro interno: {ex.Message}");
        }
    }

    [HttpPut("{fontSgFonte}/{insuNrCodigo}")]
    public async Task<ActionResult<Insumo>> Update(string fontSgFonte, int insuNrCodigo, [FromBody] Insumo insumo)
    {
        try
        {
            if (fontSgFonte != insumo.FontSgFonte || insuNrCodigo != insumo.InsuNrCodigo)
                return BadRequest("Parâmetros de identificação não coincidem");

            insumo.InsuDtUltRevisao = DateTime.UtcNow;
            var updatedInsumo = await _insumoService.UpdateAsync(insumo);
            return Ok(updatedInsumo);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro interno: {ex.Message}");
        }
    }

    [HttpDelete("{fontSgFonte}/{insuNrCodigo}")]
    public async Task<ActionResult> Delete(string fontSgFonte, int insuNrCodigo)
    {
        try
        {
            var deleted = await _insumoService.DeleteAsync(fontSgFonte, insuNrCodigo);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro interno: {ex.Message}");
        }
    }

    [HttpGet("fontes")]
    public async Task<ActionResult<IEnumerable<Fonte>>> GetFontes()
    {
        try
        {
            var fontes = await _insumoService.GetFontesAsync();
            return Ok(fontes);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro interno: {ex.Message}");
        }
    }

    [HttpGet("grupos")]
    public async Task<ActionResult<IEnumerable<GrupoInsumo>>> GetGrupos()
    {
        try
        {
            var grupos = await _insumoService.GetGruposInsumoAsync();
            return Ok(grupos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro interno: {ex.Message}");
        }
    }
}
