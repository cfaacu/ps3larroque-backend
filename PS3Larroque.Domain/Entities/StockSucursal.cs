public class StockSucursal
{
    public string Codigo { get; set; } = null!;
    public string Consola { get; set; } = null!;
    public string Descripcion { get; set; } = null!;
    public string Tipo { get; set; } = null!;
    public string Categoria { get; set; } = null!;
    
    // acá guardamos el ID numérico de sucursal como texto ("1", "2", etc.)
    public string Sucursal { get; set; } = null!;

    public int Stock { get; set; }
    public decimal Precio { get; set; }
    public DateTime ActualizadoEn { get; set; }
}