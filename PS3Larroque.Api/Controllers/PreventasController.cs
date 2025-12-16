using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PS3Larroque.Application.Dtos;
using PS3Larroque.Application.Interfaces;

namespace PS3Larroque.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PreventasController : ControllerBase
{
    private readonly IPreventaService _preventaService;
    private readonly ILogger<PreventasController> _logger;

    public PreventasController(IPreventaService preventaService, ILogger<PreventasController> logger)
    {
        _preventaService = preventaService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<int>> Crear([FromBody] PreventaCreateDto dto)
    {
        if (dto.Items == null || dto.Items.Count == 0)
            return BadRequest("La preventa debe tener al menos un item.");

        var id = await _preventaService.CrearPreventaAsync(dto);
        return Ok(id);
    }

    // ✅ AHORA RECIBE SUCURSALID Y (OPCIONAL) SOLOPENDIENTES
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PreventaListadoDto>>> Listar(
        [FromQuery] int sucursalId,
        [FromQuery] int max = 100,
        [FromQuery] bool soloPendientes = true)
    {
        _logger.LogInformation("[GET /api/preventas] sucursalId={SucursalId} max={Max} soloPendientes={SoloPendientes}",
            sucursalId, max, soloPendientes);

        var lista = await _preventaService.ListarAsync(sucursalId, max, soloPendientes);

        _logger.LogInformation("[GET /api/preventas] Devueltas={Count}", lista.Count);

        return Ok(lista);
    }

    [HttpPut("{id}/procesar")]
    public async Task<ActionResult> Procesar(int id)
    {
        await _preventaService.MarcarProcesadaAsync(id);
        return NoContent();
    }
}
