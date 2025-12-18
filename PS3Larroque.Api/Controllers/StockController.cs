using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PS3Larroque.Application.Dtos;
using PS3Larroque.Application.Interfaces;

namespace PS3Larroque.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StockController : ControllerBase
{
    private readonly IStockService _stockService;
    private readonly ILogger<StockController> _logger;

    public StockController(IStockService stockService, ILogger<StockController> logger)
    {
        _stockService = stockService;
        _logger = logger;
    }

    // ✅ /api/stock/buscar?term=algo&sucursalId=2&limit=30
    [HttpGet("buscar")]
    public async Task<ActionResult<IEnumerable<StockSearchResultDto>>> Buscar(
        [FromQuery] string term,
        [FromQuery] int sucursalId,   // 👈 ESTE NOMBRE DEBE COINCIDIR CON LO QUE USA EL FRONT
        [FromQuery] int limit = 30)
    {
        var termRaw = term ?? "";
        var termTrim = termRaw.Trim();

        _logger.LogInformation(
            "[STOCK BUSCAR] sucursalId={SucursalId} limit={Limit} termRaw='{TermRaw}' termTrim='{TermTrim}'",
            sucursalId, limit, termRaw, termTrim);

        IEnumerable<StockSearchResultDto> result;

        try
        {
            result = await _stockService.BuscarAsync(termTrim, sucursalId, limit);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "[STOCK BUSCAR] ERROR sucursalId={SucursalId} termTrim='{TermTrim}'",
                sucursalId, termTrim);

            return StatusCode(500, "Error interno en búsqueda de stock.");
        }

        var count = result?.Count() ?? 0;
        _logger.LogInformation(
            "[STOCK BUSCAR] OK sucursalId={SucursalId} termTrim='{TermTrim}' results={Count}",
            sucursalId, termTrim, count);

        // opcional para debug desde el front / devtools
        Response.Headers["X-Results-Count"] = count.ToString();

        return Ok(result);
    }

    // /api/stock/por-codigo?codigo=8713...&sucursal=2
    [HttpGet("por-codigo")]
    public async Task<ActionResult<IEnumerable<StockSearchResultDto>>> PorCodigo(
        [FromQuery] string codigo,
        [FromQuery] int sucursal)
    {
        var codigoRaw = codigo ?? "";
        var codigoTrim = codigoRaw.Trim();

        _logger.LogInformation(
            "[STOCK POR-CODIGO] sucursal={Sucursal} codigoRaw='{CodigoRaw}' codigoTrim='{CodigoTrim}'",
            sucursal, codigoRaw, codigoTrim);

        IEnumerable<StockSearchResultDto> result;

        try
        {
            result = await _stockService.GetByCodigoAsync(codigoTrim, sucursal);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "[STOCK POR-CODIGO] ERROR sucursal={Sucursal} codigoTrim='{CodigoTrim}'",
                sucursal, codigoTrim);

            return StatusCode(500, "Error interno en búsqueda por código.");
        }

        var count = result?.Count() ?? 0;
        _logger.LogInformation(
            "[STOCK POR-CODIGO] OK sucursal={Sucursal} codigoTrim='{CodigoTrim}' results={Count}",
            sucursal, codigoTrim, count);

        Response.Headers["X-Results-Count"] = count.ToString();

        return Ok(result);
    }
}
