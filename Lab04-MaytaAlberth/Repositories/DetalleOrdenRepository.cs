using Lab04_MaytaAlberth.Models;
using Lab04_MaytaAlberth.Repositories;
using Lab04_MaytaAlberth.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Lab04_MaytaAlberth.Repositories
{
    public class DetalleOrdenRepository : Repository<Detallesorden>, IDetalleOrdenRepository
    {
        public DetalleOrdenRepository(TiendaContext context) : base(context) { }

        public async Task<IEnumerable<Detallesorden>> GetByOrdenAsync(int ordenId)
        {
            return await _context.Set<Detallesorden>()
                .Include(d => d.Producto)
                .Include(d => d.Orden)
                .Where(d => d.OrdenId == ordenId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Detallesorden>> GetByProductoAsync(int productoId)
        {
            return await _context.Set<Detallesorden>()
                .Include(d => d.Producto)
                .Include(d => d.Orden)
                .Where(d => d.ProductoId == productoId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Detallesorden>> GetAllWithDetailsAsync()
        {
            return await _context.Set<Detallesorden>()
                .Include(d => d.Producto)
                .Include(d => d.Orden)
                .ToListAsync();
        }

        public async Task<Detallesorden?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Set<Detallesorden>()
                .Include(d => d.Producto)
                .Include(d => d.Orden)
                .FirstOrDefaultAsync(d => d.DetalleId == id);
        }
    }
}