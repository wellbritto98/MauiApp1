using Microsoft.AspNetCore.Mvc;
using MauiApp1.Web.Models;
using MauiApp1.Web.Services;

namespace MauiApp1.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServicosController : ControllerBase
{
    private readonly IServicoService _servicoService;

    public ServicosController(IServicoService servicoService)
    {
        _servicoService = servicoService;
    }

    [HttpGet("paged")]
    public async Task<ActionResult<PagedResult<Servico>>> GetPaged([FromQuery] FilterRequest filter)
    {
        try
        {
            var result = await _servicoService.GetPagedAsync(filter);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro interno: {ex.Message}");
        }
    }

    [HttpGet("{fontSgFonte}/{servNrCodigo}")]
    public async Task<ActionResult<Servico>> GetById(string fontSgFonte, int servNrCodigo)
    {
        try
        {
            var servico = await _servicoService.GetByIdAsync(fontSgFonte, servNrCodigo);
            if (servico == null)
                return NotFound();

            return Ok(servico);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro interno: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<ActionResult<Servico>> Create([FromBody] Servico servico)
    {
        try
        {
            servico.ServDtCadastro = DateTime.UtcNow;
            servico.ServDtUltRevisao = DateTime.UtcNow;
            var createdServico = await _servicoService.CreateAsync(servico);
            return CreatedAtAction(nameof(GetById), new { fontSgFonte = createdServico.FontSgFonte, servNrCodigo = createdServico.ServNrCodigo }, createdServico);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro interno: {ex.Message}");
        }
    }

    [HttpPut("{fontSgFonte}/{servNrCodigo}")]
    public async Task<ActionResult<Servico>> Update(string fontSgFonte, int servNrCodigo, [FromBody] Servico servico)
    {
        try
        {
            if (fontSgFonte != servico.FontSgFonte || servNrCodigo != servico.ServNrCodigo)
                return BadRequest("Parâmetros de identificação não coincidem");

            servico.ServDtUltRevisao = DateTime.UtcNow;
            var updatedServico = await _servicoService.UpdateAsync(servico);
            return Ok(updatedServico);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro interno: {ex.Message}");
        }
    }

    [HttpDelete("{fontSgFonte}/{servNrCodigo}")]
    public async Task<ActionResult> Delete(string fontSgFonte, int servNrCodigo)
    {
        try
        {
            var deleted = await _servicoService.DeleteAsync(fontSgFonte, servNrCodigo);
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
            var fontes = await _servicoService.GetFontesAsync();
            return Ok(fontes);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro interno: {ex.Message}");
        }
    }

    [HttpGet("grupos")]
    public async Task<ActionResult<IEnumerable<GrupoServico>>> GetGrupos()
    {
        try
        {
            var grupos = await _servicoService.GetGruposServicoAsync();
            return Ok(grupos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro interno: {ex.Message}");
        }
    }
}
