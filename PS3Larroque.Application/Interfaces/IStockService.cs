using PS3Larroque.Application.Dtos;

namespace PS3Larroque.Application.Interfaces;

public interface IStockService
{
    Task<List<StockSearchResultDto>> BuscarAsync(string term, int limit = 20);
    Task<List<StockSearchResultDto>> GetByCodigoAsync(string codigo);
}
