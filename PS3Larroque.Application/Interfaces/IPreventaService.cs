using PS3Larroque.Application.Dtos;

namespace PS3Larroque.Application.Interfaces;

public interface IPreventaService
{
    Task<int> CrearPreventaAsync(PreventaCreateDto dto);

    Task<List<PreventaListadoDto>> ListarAsync(int sucursalId, int max = 100, bool soloPendientes = true);

    Task MarcarProcesadaAsync(int id);
}
