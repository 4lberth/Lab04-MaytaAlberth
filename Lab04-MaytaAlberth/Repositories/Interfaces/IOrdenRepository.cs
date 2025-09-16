using Lab04_MaytaAlberth.Models;
using Lab04_MaytaAlberth.Repositories.Interfaces;

namespace Lab04_MaytaAlberth.Repositories.Interfaces
{
    public interface IOrdenRepository : IRepository<Ordene>
    {
        Task<IEnumerable<Ordene>> GetByClienteAsync(int clienteId);
        Task<IEnumerable<Ordene>> GetByFechaRangoAsync(DateTime fechaInicio, DateTime fechaFin);
        Task<IEnumerable<Ordene>> GetAllWithDetailsAsync();
        Task<Ordene?> GetByIdWithDetailsAsync(int id);
    }
}