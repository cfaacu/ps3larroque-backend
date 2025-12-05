namespace PS3Larroque.Application.Dtos;

public class PreventaItemCreateDto
{
    public string Codigo { get; set; } = null!;
    public int Cantidad { get; set; }
    public decimal PrecioUnit { get; set; }
}

public class PreventaCreateDto
{
    public string Sucursal { get; set; } = null!;
    public string Vendedor { get; set; } = null!;
    public List<PreventaItemCreateDto> Items { get; set; } = new();
}

public class PreventaListItemDto
{
    public int Id { get; set; }
    public string Sucursal { get; set; } = null!;
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
        public string Sucursal { get; set; } = string.Empty;
        public string Vendedor { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public List<PreventaItemDto> Items { get; set; } = new();
    }