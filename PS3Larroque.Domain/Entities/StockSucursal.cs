namespace PS3Larroque.Domain.Entities;

public class StockSucursal
{
    public string Codigo { get; set; } = null!;
    public string Consola { get; set; } = null!;
    public string Descripcion { get; set; } = null!;
    public string? Tipo { get; set; }
    public string? Categoria { get; set; }
    public string Sucursal { get; set; } = null!;
    public int Stock { get; set; }
    public decimal Precio { get; set; }
    public DateTime ActualizadoEn { get; set; }

    public ProductoLegacy Producto { get; set; } = null!;
}
