using Lab04_MaytaAlberth.Models;
using Lab04_MaytaAlberth.Repositories.Interfaces;

namespace Lab04_MaytaAlberth.Repositories.Interfaces
{
    public interface IProductoRepository : IRepository<Producto>
    {
        Task<IEnumerable<Producto>> GetByCategoriaAsync(int categoriaId);
        Task<IEnumerable<Producto>> GetProductosConStockAsync();
        Task<IEnumerable<Producto>> BuscarProductosAsync(string termino);
        Task<IEnumerable<Producto>> GetAllWithDetailsAsync();
        Task<Producto?> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<Producto>> GetProductosPorRangoPrecioAsync(decimal precioMin, decimal precioMax);
    }
}