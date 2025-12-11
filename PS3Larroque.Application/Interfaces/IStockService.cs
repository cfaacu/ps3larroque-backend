using PS3Larroque.Application.Dtos;

namespace PS3Larroque.Application.Interfaces;

public interface IStockService
{
    Task<IEnumerable<StockSearchResultDto>> BuscarAsync(
        string term,
        int sucursalId,
        int limit = 30);

    // También buscar por código, pero **solo dentro de una sucursal**
    Task<List<StockSearchResultDto>> GetByCodigoAsync(
        string codigo,
        int sucursalId);
}
