using Microsoft.AspNetCore.Mvc;
using PS3Larroque.Application.Dtos;
using PS3Larroque.Application.Interfaces;

namespace PS3Larroque.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StockController : ControllerBase
{
    private readonly IStockService _stockService;

    public StockController(IStockService stockService)
    {
        _stockService = stockService;
    }

    [HttpGet("buscar")]
    public async Task<ActionResult<IEnumerable<StockSearchResultDto>>> Buscar(
        [FromQuery] string term,
        [FromQuery] int limit = 20)
    {
        if (string.IsNullOrWhiteSpace(term))
            return BadRequest("term es requerido.");

        var resultados = await _stockService.BuscarAsync(term, limit);
        return Ok(resultados);
    }

    [HttpGet("{codigo}")]
    public async Task<ActionResult<IEnumerable<StockSearchResultDto>>> PorCodigo(string codigo)
    {
        var resultados = await _stockService.GetByCodigoAsync(codigo);
        if (resultados.Count == 0)
            return NotFound();

        return Ok(resultados);
    }
}
