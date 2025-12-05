namespace PS3Larroque.Domain.Entities;

public class ProductoLegacy
{
    public string Codigo { get; set; } = null!;
    public string Consola { get; set; } = null!;
    public string Descripcion { get; set; } = null!;
    public decimal PrecioVentaNuevo { get; set; }
    public decimal PrecioCompraUsado { get; set; }

    public ICollection<StockSucursal> Stocks { get; set; } = new List<StockSucursal>();
}
