namespace PS3Larroque.Application.Dtos;

public class StockSearchResultDto
{
    public string Codigo { get; set; } = null!;
    public string Consola { get; set; } = null!;
    public string Descripcion { get; set; } = null!;
    public string? Tipo { get; set; }
    public string? Categoria { get; set; }
    public string Sucursal { get; set; } = null!;
    public int Stock { get; set; }
    public decimal Precio { get; set; }
}
