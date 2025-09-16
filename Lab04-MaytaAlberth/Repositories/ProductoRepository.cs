using Lab04_MaytaAlberth.Models;
using Lab04_MaytaAlberth.Repositories;
using Lab04_MaytaAlberth.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Lab04_MaytaAlberth.Repositories
{
    public class ProductoRepository : Repository<Producto>, IProductoRepository
    {
        public ProductoRepository(TiendaContext context) : base(context) { }

        public async Task<IEnumerable<Producto>> GetByCategoriaAsync(int categoriaId)
        {
            return await _context.Set<Producto>()
                .Include(p => p.Categoria)
                .Include(p => p.Detallesordens)
                .Where(p => p.CategoriaId == categoriaId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Producto>> GetProductosConStockAsync()
        {
            return await _context.Set<Producto>()
                .Include(p => p.Categoria)
                .Include(p => p.Detallesordens)
                .Where(p => p.Stock > 0)
                .ToListAsync();
        }

        public async Task<IEnumerable<Producto>> BuscarProductosAsync(string termino)
        {
            return await _context.Set<Producto>()
                .Include(p => p.Categoria)
                .Include(p => p.Detallesordens)
                .Where(p => p.Nombre.Contains(termino) || 
                           (p.Descripcion != null && p.Descripcion.Contains(termino)))
                .ToListAsync();
        }

        public async Task<IEnumerable<Producto>> GetAllWithDetailsAsync()
        {
            return await _context.Set<Producto>()
                .Include(p => p.Categoria)
                .Include(p => p.Detallesordens)
                .ToListAsync();
        }

        public async Task<Producto?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Set<Producto>()
                .Include(p => p.Categoria)
                .Include(p => p.Detallesordens)
                .FirstOrDefaultAsync(p => p.ProductoId == id);
        }

        public async Task<IEnumerable<Producto>> GetProductosPorRangoPrecioAsync(decimal precioMin, decimal precioMax)
        {
            return await _context.Set<Producto>()
                .Include(p => p.Categoria)
                .Include(p => p.Detallesordens)
                .Where(p => p.Precio >= precioMin && p.Precio <= precioMax)
                .OrderBy(p => p.Precio)
                .ToListAsync();
        }
    }
}