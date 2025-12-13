namespace PS3Larroque.Domain.Entities;

public class Preventa
{
    public int Id { get; set; }

    public int SucursalId { get; set; }          // 👈 antes string Sucursal

    public string Vendedor { get; set; } = null!;
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    public string Estado { get; set; } = "pendiente";

    public ICollection<PreventaItem> Items { get; set; } = new List<PreventaItem>();
}
