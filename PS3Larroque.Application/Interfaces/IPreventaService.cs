using PS3Larroque.Application.Dtos;

namespace PS3Larroque.Application.Interfaces;

public interface IPreventaService
{
    Task<int> CrearPreventaAsync(PreventaCreateDto dto);

    // 👇 obligamos a filtrar por sucursal
    Task<List<PreventaListadoDto>> ListarAsync(int sucursalId, int max = 100);

    Task MarcarProcesadaAsync(int id);
}
