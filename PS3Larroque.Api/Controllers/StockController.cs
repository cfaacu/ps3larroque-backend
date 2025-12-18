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

    // ✅ /api/stock/buscar?term=algo&sucursalId=2&limit=30
    [HttpGet("buscar")]
    public async Task<ActionResult<IEnumerable<StockSearchResultDto>>> Buscar(
        string term,
        int sucursalId,   // 👈 ESTE NOMBRE DEBE COINCIDIR CON LO QUE USA EL FRONT
        int limit = 30)
    {
        var result = await _stockService.BuscarAsync(term, sucursalId, limit);
        return Ok(result);
    }

    // /api/stock/por-codigo?codigo=8713...&sucursal=2
    [HttpGet("por-codigo")]
    public async Task<ActionResult<IEnumerable<StockSearchResultDto>>> PorCodigo(
        [FromQuery] string codigo,
        [FromQuery] int sucursal)
    {
        var result = await _stockService.GetByCodigoAsync(codigo, sucursal);
        return Ok(result);
    }
}
