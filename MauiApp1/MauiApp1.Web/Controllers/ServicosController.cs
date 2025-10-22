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

    [HttpGet("{id}")]
    public async Task<ActionResult<Servico>> GetById(int id)
    {
        try
        {
            var servico = await _servicoService.GetByIdAsync(id);
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
            servico.DataCriacao = DateTime.UtcNow;
            var createdServico = await _servicoService.CreateAsync(servico);
            return CreatedAtAction(nameof(GetById), new { id = createdServico.Id }, createdServico);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro interno: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Servico>> Update(int id, [FromBody] Servico servico)
    {
        try
        {
            if (id != servico.Id)
                return BadRequest("ID mismatch");

            servico.DataAtualizacao = DateTime.UtcNow;
            var updatedServico = await _servicoService.UpdateAsync(servico);
            return Ok(updatedServico);
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
            var deleted = await _servicoService.DeleteAsync(id);
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