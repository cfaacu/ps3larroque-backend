using Microsoft.EntityFrameworkCore;
using PS3Larroque.Application.Dtos;
using PS3Larroque.Application.Interfaces;
using PS3Larroque.Domain.Entities;

namespace PS3Larroque.Infrastructure.Services;

public class PreventaService : IPreventaService
{
    private readonly ApplicationDbContext _db;

    public PreventaService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<int> CrearPreventaAsync(PreventaCreateDto dto)
    {
        if (dto.SucursalId <= 0)
            throw new InvalidOperationException("Sucursal inválida.");

        if (dto.Items == null || dto.Items.Count == 0)
            throw new InvalidOperationException("La preventa debe tener al menos un ítem.");

        var preventa = new Preventa
        {
            SucursalId = dto.SucursalId,
            Vendedor = dto.Vendedor,
            Estado = "pendiente",
            FechaCreacion = DateTime.UtcNow
        };

        var codigos = dto.Items
            .Select(i => i.Codigo)
            .Where(c => !string.IsNullOrWhiteSpace(c))
            .Distinct()
            .ToList();

        var productos = await _db.ProductosLegacy
            .AsNoTracking()
            .Where(p => codigos.Contains(p.Codigo))
            .ToDictionaryAsync(p => p.Codigo, p => p.Descripcion);

        foreach (var it in dto.Items)
        {
            var desc = productos.TryGetValue(it.Codigo, out var d) ? d : it.Codigo;

            preventa.Items.Add(new PreventaItem
            {
                Codigo = it.Codigo,
                Descripcion = desc,
                Cantidad = it.Cantidad,

                PrecioUnit = it.PrecioUnit,

                MetodoPagoKey = string.IsNullOrWhiteSpace(it.MetodoPagoKey) ? "base" : it.MetodoPagoKey.Trim()
            });
        }

        _db.Preventas.Add(preventa);
        await _db.SaveChangesAsync();
        return preventa.Id;
    }

    public async Task<List<PreventaListadoDto>> ListarAsync(int sucursalId, int max = 100, bool soloPendientes = true)
    {
        if (sucursalId <= 0)
            throw new ArgumentException($"Sucursal inválida: {sucursalId}", nameof(sucursalId));

        var productosDict = await _db.ProductosLegacy
            .AsNoTracking()
            .ToDictionaryAsync(p => p.Codigo, p => p.Descripcion);

        IQueryable<Preventa> q = _db.Preventas
            .AsNoTracking()
            .Where(p => p.SucursalId == sucursalId);

        if (soloPendientes)
            q = q.Where(p => p.Estado == "pendiente");

        var preventas = await q
            .Include(p => p.Items)
            .OrderByDescending(p => p.Id)
            .Take(max)
            .ToListAsync();

        var resultado = preventas.Select(p => new PreventaListadoDto
        {
            Id = p.Id,
            SucursalId = p.SucursalId,
            Vendedor = p.Vendedor,
            Estado = p.Estado,
            Fecha = p.FechaCreacion,
            Items = p.Items.Select(it =>
            {
                productosDict.TryGetValue(it.Codigo, out var desc);
                return new PreventaItemDto
                {
                    Codigo = it.Codigo,
                    Descripcion = desc ?? it.Descripcion ?? string.Empty,
                    Cantidad = it.Cantidad,
                    PrecioUnit = it.PrecioUnit,

                    // ✅ NUEVO
                    MetodoPagoKey = it.MetodoPagoKey
                };
            }).ToList()
        }).ToList();

        foreach (var pr in resultado)
            pr.Total = pr.Items.Sum(i => i.Subtotal);

        return resultado;
    }

    public async Task MarcarProcesadaAsync(int id)
    {
        var prev = await _db.Preventas.FindAsync(id);
        if (prev == null)
            throw new KeyNotFoundException("Preventa no encontrada.");

        prev.Estado = "procesada";
        await _db.SaveChangesAsync();
    }
}
