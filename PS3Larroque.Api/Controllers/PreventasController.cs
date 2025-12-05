using Microsoft.AspNetCore.Mvc;
using PS3Larroque.Application.Dtos;
using PS3Larroque.Application.Interfaces;

namespace PS3Larroque.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PreventasController : ControllerBase
{
    private readonly IPreventaService _preventaService;

    public PreventasController(IPreventaService preventaService)
    {
        _preventaService = preventaService;
    }

    [HttpPost]
    public async Task<ActionResult<int>> Crear([FromBody] PreventaCreateDto dto)
    {
        if (dto.Items == null || dto.Items.Count == 0)
            return BadRequest("La preventa debe tener al menos un item.");

        var id = await _preventaService.CrearPreventaAsync(dto);
        return Ok(id);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PreventaListadoDto>>> Listar([FromQuery] int max = 100)
    {
        var lista = await _preventaService.ListarAsync(max);
        return Ok(lista);
    }

    [HttpPut("{id}/procesar")]
    public async Task<ActionResult> Procesar(int id)
    {
        await _preventaService.MarcarProcesadaAsync(id);
        return NoContent();
    }
}
