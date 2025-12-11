using Microsoft.EntityFrameworkCore;
using PS3Larroque.Application.Dtos;
using PS3Larroque.Application.Interfaces;

namespace PS3Larroque.Infrastructure.Services;

public class StockService : IStockService
{
    private readonly ApplicationDbContext _db;

    // Mapeo ID → nombre que querés mostrar
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

    // Busca por término (código o descripción) dentro de UNA sucursal
    public async Task<IEnumerable<StockSearchResultDto>> BuscarAsync(
        string term,
        int sucursalId,
        int limit = 30)
    {
        if (string.IsNullOrWhiteSpace(term))
            return new List<StockSearchResultDto>();

        if (!Sucursales.ContainsKey(sucursalId))
            throw new ArgumentException($"Sucursal inválida: {sucursalId}", nameof(sucursalId));

        string sucursalText = sucursalId.ToString();
        term = term.Trim();

        // Si es todo números y larguito, asumimos código de barras
        bool esCodigo = term.All(char.IsDigit) && term.Length >= 8;

        var query = _db.StocksSucursal
            .AsNoTracking()
            .Where(s => s.Sucursal == sucursalText && s.Stock > 0);

        if (esCodigo)
        {
            query = query.Where(s => s.Codigo == term);
        }
        else
        {
            string like = $"%{term.ToLower()}%";
            query = query.Where(s =>
                EF.Functions.ILike(s.Descripcion, like) ||
                s.Codigo == term);
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

    // Busca por código exacto dentro de UNA sucursal
    public async Task<List<StockSearchResultDto>> GetByCodigoAsync(
        string codigo,
        int sucursalId)
    {
        if (string.IsNullOrWhiteSpace(codigo))
            return new List<StockSearchResultDto>();

        if (!Sucursales.ContainsKey(sucursalId))
            throw new ArgumentException($"Sucursal inválida: {sucursalId}", nameof(sucursalId));

        string sucursalText = sucursalId.ToString();
        codigo = codigo.Trim();

        var list = await _db.StocksSucursal
            .AsNoTracking()
            .Where(s => s.Codigo == codigo && s.Sucursal == sucursalText)
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
