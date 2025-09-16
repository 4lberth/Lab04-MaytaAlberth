using Lab04_MaytaAlberth.Models;
using Lab04_MaytaAlberth.Repositories.Interfaces;

namespace Lab04_MaytaAlberth.Repositories.Interfaces
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        Task<Categoria?> GetByNombreAsync(string nombre);
        Task<IEnumerable<Categoria>> GetAllWithProductsAsync();
        Task<Categoria?> GetByIdWithProductsAsync(int id);
    }
}