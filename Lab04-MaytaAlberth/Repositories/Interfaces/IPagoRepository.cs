using Lab04_MaytaAlberth.Models;
using Lab04_MaytaAlberth.Repositories.Interfaces;

namespace Lab04_MaytaAlberth.Repositories.Interfaces
{
    public interface IPagoRepository : IRepository<Pago>
    {
        Task<IEnumerable<Pago>> GetByOrdenAsync(int ordenId);
        Task<IEnumerable<Pago>> GetByMetodoPagoAsync(string metodoPago);
        Task<IEnumerable<Pago>> GetByFechaRangoAsync(DateTime fechaInicio, DateTime fechaFin);
        Task<IEnumerable<Pago>> GetAllWithDetailsAsync();
        Task<Pago?> GetByIdWithDetailsAsync(int id);
    }
}