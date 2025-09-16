using Lab04_MaytaAlberth;
using Lab04_MaytaAlberth.Models;
using Lab04_MaytaAlberth.Repositories.Interfaces;

namespace Lab04_MaytaAlberth.Repositories.Interfaces
{
    public interface IDetalleOrdenRepository : IRepository<Detallesorden>
    {
        Task<IEnumerable<Detallesorden>> GetByOrdenAsync(int ordenId);
        Task<IEnumerable<Detallesorden>> GetByProductoAsync(int productoId);
        Task<IEnumerable<Detallesorden>> GetAllWithDetailsAsync();
        Task<Detallesorden?> GetByIdWithDetailsAsync(int id);
    }
}