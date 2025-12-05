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
        if (dto.Items == null || dto.Items.Count == 0)
            throw new InvalidOperationException("La preventa debe tener al menos un ítem.");

        var preventa = new Preventa
        {
            Sucursal = dto.Sucursal,
            Vendedor = dto.Vendedor,
            Estado = "pendiente",
            FechaCreacion = DateTime.UtcNow
        };

        var codigos = dto.Items.Select(i => i.Codigo).Distinct().ToList();
        var productos = await _db.ProductosLegacy
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
                PrecioUnit = it.PrecioUnit
            });
        }

        _db.Preventas.Add(preventa);
        await _db.SaveChangesAsync();
        return preventa.Id;
    }

            public async Task<List<PreventaListadoDto>> ListarAsync(int max = 100)
        {
            // Traemos todos los productos para poder mapear códigos -> descripción
            var productosDict = await _db.ProductosLegacy
                .AsNoTracking()
                .ToDictionaryAsync(p => p.Codigo, p => p.Descripcion);

            var preventas = await _db.Preventas
                .AsNoTracking()
                .Include(p => p.Items)
                .OrderByDescending(p => p.Id)
                .Take(max)
                .ToListAsync();

            var resultado = preventas.Select(p => new PreventaListadoDto
            {
                Id = p.Id,
                Sucursal = p.Sucursal,
                Vendedor = p.Vendedor,
                Fecha = p.FechaCreacion,
                Items = p.Items.Select(it =>
                {
                    productosDict.TryGetValue(it.Codigo, out var desc);
                    return new PreventaItemDto
                    {
                        Codigo = it.Codigo,
                        Descripcion = desc ?? string.Empty,
                        Cantidad = it.Cantidad,
                        PrecioUnit = it.PrecioUnit
                    };
                }).ToList()
            }).ToList();

            // Calcular total por preventa
            foreach (var pr in resultado)
            {
                pr.Total = pr.Items.Sum(i => i.Subtotal);
            }

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
