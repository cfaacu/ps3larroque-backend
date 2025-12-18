using Microsoft.EntityFrameworkCore;
using PS3Larroque.Application.Dtos;
using PS3Larroque.Application.Interfaces;

namespace PS3Larroque.Infrastructure.Services;

public class StockService : IStockService
{
    private readonly ApplicationDbContext _db;

    private static readonly Dictionary<int, string> Sucursales = new()
    {
        { 1, "Banfield" },
        { 2, "Adrogue" },
        { 3, "Lomas" },
        { 4, "Avellaneda Stand" },
        { 5, "Avellaneda Local" },
        { 6, "Brown" },
        { 7, "Plaza Oeste" },
        { 8, "Martinez Stand" },
        { 9, "Martinez Local" },
        { 10, "Abasto" }
    };

    public StockService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<StockSearchResultDto>> BuscarAsync(string term, int sucursalId, int limit = 30)
    {
        if (string.IsNullOrWhiteSpace(term))
            return new List<StockSearchResultDto>();

        if (!Sucursales.ContainsKey(sucursalId))
            throw new ArgumentException($"Sucursal inválida: {sucursalId}", nameof(sucursalId));

        var sucursalText = sucursalId.ToString();
        term = term.Trim();

        bool esCodigoBarras = term.All(char.IsDigit) && term.Length >= 8;

        var query = _db.StocksSucursal
            .AsNoTracking()
            // ✅ trim en sucursal y código para evitar espacios invisibles
            .Where(s => (s.Sucursal ?? "").Trim() == sucursalText && s.Stock > 0);

        if (esCodigoBarras)
        {
            // Código de barras (numérico): exacto pero trim + case no aplica
            query = query.Where(s => ((s.Codigo ?? "").Trim()) == term);
        }
        else
        {
            var likeDesc = $"%{term}%";
            query = query.Where(s =>
                EF.Functions.ILike((s.Descripcion ?? ""), likeDesc) ||
                EF.Functions.ILike(((s.Codigo ?? "").Trim()), term) // ✅ exacto, case-insensitive, y sin espacios
            );
        }

        var list = await query
            .OrderByDescending(s => s.Stock)
            .ThenBy(s => s.Descripcion)
            .Take(limit)
            .Select(s => new StockSearchResultDto
            {
                Codigo         = s.Codigo,
                Consola        = s.Consola,
                Descripcion    = s.Descripcion,
                Tipo           = s.Tipo,
                Categoria      = s.Categoria,
                SucursalId     = sucursalId,
                SucursalNombre = Sucursales[sucursalId],
                Stock          = s.Stock,
                Precio         = s.Precio
            })
            .ToListAsync();

        return list;
    }

    public async Task<List<StockSearchResultDto>> GetByCodigoAsync(string codigo, int sucursalId)
    {
        if (string.IsNullOrWhiteSpace(codigo))
            return new List<StockSearchResultDto>();

        if (!Sucursales.ContainsKey(sucursalId))
            throw new ArgumentException($"Sucursal inválida: {sucursalId}", nameof(sucursalId));

        var sucursalText = sucursalId.ToString();
        codigo = codigo.Trim();

        var list = await _db.StocksSucursal
            .AsNoTracking()
            .Where(s => (s.Sucursal ?? "").Trim() == sucursalText)
            .Where(s => EF.Functions.ILike(((s.Codigo ?? "").Trim()), codigo))
            .Select(s => new StockSearchResultDto
            {
                Codigo         = s.Codigo,
                Consola        = s.Consola,
                Descripcion    = s.Descripcion,
                Tipo           = s.Tipo,
                Categoria      = s.Categoria,
                SucursalId     = sucursalId,
                SucursalNombre = Sucursales[sucursalId],
                Stock          = s.Stock,
                Precio         = s.Precio
            })
            .ToListAsync();

        return list;
    }
}
