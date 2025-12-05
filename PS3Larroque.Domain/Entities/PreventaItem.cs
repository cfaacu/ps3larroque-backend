namespace PS3Larroque.Domain.Entities;

public class PreventaItem
{
    public int Id { get; set; }
    public int PreventaId { get; set; }
    public string Codigo { get; set; } = null!;
    public string Descripcion { get; set; } = null!;
    public int Cantidad { get; set; }
    public decimal PrecioUnit { get; set; }

    public Preventa Preventa { get; set; } = null!;
}
