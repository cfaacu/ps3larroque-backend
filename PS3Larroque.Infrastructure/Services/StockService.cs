using Microsoft.EntityFrameworkCore;
using PS3Larroque.Application.Dtos;
using PS3Larroque.Application.Interfaces;

namespace PS3Larroque.Infrastructure.Services;

public class StockService : IStockService
{
    private readonly ApplicationDbContext _db;

    public StockService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<List<StockSearchResultDto>> BuscarAsync(string term, int limit = 20)
    {
        if (string.IsNullOrWhiteSpace(term))
            return new List<StockSearchResultDto>();

        term = term.Trim().ToLower();

        return await _db.StocksSucursal
            .AsNoTracking()
            .Where(s =>
                s.Descripcion.ToLower().Contains(term) ||
                s.Codigo.ToLower().Contains(term))
            .OrderBy(s => s.Descripcion)
            .Take(limit)
            .Select(s => new StockSearchResultDto
            {
                Codigo = s.Codigo,
                Consola = s.Consola,
                Descripcion = s.Descripcion,
                Tipo = s.Tipo,
                Categoria = s.Categoria,
                Sucursal = s.Sucursal,
                Stock = s.Stock,
                Precio = s.Precio
            })
            .ToListAsync();
    }

    public async Task<List<StockSearchResultDto>> GetByCodigoAsync(string codigo)
    {
        if (string.IsNullOrWhiteSpace(codigo))
            return new List<StockSearchResultDto>();

        return await _db.StocksSucursal
            .AsNoTracking()
            .Where(s => s.Codigo == codigo)
            .Select(s => new StockSearchResultDto
            {
                Codigo = s.Codigo,
                Consola = s.Consola,
                Descripcion = s.Descripcion,
                Tipo = s.Tipo,
                Categoria = s.Categoria,
                Sucursal = s.Sucursal,
                Stock = s.Stock,
                Precio = s.Precio
            })
            .ToListAsync();
    }
}
