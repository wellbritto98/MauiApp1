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

    [HttpGet("{id}")]
    public async Task<ActionResult<Insumo>> GetById(int id)
    {
        try
        {
            var insumo = await _insumoService.GetByIdAsync(id);
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
            insumo.DataCriacao = DateTime.UtcNow;
            var createdInsumo = await _insumoService.CreateAsync(insumo);
            return CreatedAtAction(nameof(GetById), new { id = createdInsumo.Id }, createdInsumo);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro interno: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Insumo>> Update(int id, [FromBody] Insumo insumo)
    {
        try
        {
            if (id != insumo.Id)
                return BadRequest("ID mismatch");

            insumo.DataAtualizacao = DateTime.UtcNow;
            var updatedInsumo = await _insumoService.UpdateAsync(insumo);
            return Ok(updatedInsumo);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro interno: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            var deleted = await _insumoService.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro interno: {ex.Message}");
        }
    }
}