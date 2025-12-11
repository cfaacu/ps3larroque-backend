namespace PS3Larroque.Application.Dtos;

public class StockSearchResultDto
{
    public string Codigo { get; set; } = null!;
    public string Consola { get; set; } = null!;
    public string Descripcion { get; set; } = null!;
    public string Tipo { get; set; } = null!;
    public string Categoria { get; set; } = null!;

    // En BD se guarda el número de sucursal como texto ("1", "2"...),
    // pero al front le mandamos ID + Nombre amigable:
    public int SucursalId { get; set; }
    public string SucursalNombre { get; set; } = null!;

    public int Stock { get; set; }
    public decimal Precio { get; set; }
}
