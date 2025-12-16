namespace PS3Larroque.Application.Dtos;

public class PreventaItemCreateDto
{
    public string Codigo { get; set; } = null!;
    public int Cantidad { get; set; }
    public decimal PrecioUnit { get; set; }
}

public class PreventaCreateDto
{
    public int SucursalId { get; set; }          // 👈 int
    public string Vendedor { get; set; } = null!;
    public List<PreventaItemCreateDto> Items { get; set; } = new();
}

// Si querés mantener un DTO “resumen” para una tabla, hacelo así:
public class PreventaListItemDto
{
    public int Id { get; set; }
    public int SucursalId { get; set; }          // 👈 int
    public string Vendedor { get; set; } = null!;
    public DateTime FechaCreacion { get; set; }
    public string Estado { get; set; } = null!;
    public decimal Total { get; set; }
}

public class PreventaItemDto
{
    public string Codigo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    public decimal PrecioUnit { get; set; }
    public decimal Subtotal => Cantidad * PrecioUnit;
}

public class PreventaListadoDto
{
    public int Id { get; set; }
    public int SucursalId { get; set; }
    public string Vendedor { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
    public decimal Total { get; set; }
    public List<PreventaItemDto> Items { get; set; } = new();
}
