using Lab04_MaytaAlberth.Models;
using Lab04_MaytaAlberth.Repositories;
using Lab04_MaytaAlberth.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Lab04_MaytaAlberth.Repositories
{
    public class OrdenRepository : Repository<Ordene>, IOrdenRepository
    {
        public OrdenRepository(TiendaContext context) : base(context) { }

        public async Task<IEnumerable<Ordene>> GetByClienteAsync(int clienteId)
        {
            return await _context.Set<Ordene>()
                .Include(o => o.Cliente)
                .Include(o => o.Detallesordens)
                .ThenInclude(d => d.Producto)
                .Include(o => o.Pagos)
                .Where(o => o.ClienteId == clienteId)
                .OrderByDescending(o => o.FechaOrden)
                .ToListAsync();
        }

        public async Task<IEnumerable<Ordene>> GetByFechaRangoAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            return await _context.Set<Ordene>()
                .Include(o => o.Cliente)
                .Include(o => o.Detallesordens)
                .Include(o => o.Pagos)
                .Where(o => o.FechaOrden >= fechaInicio && o.FechaOrden <= fechaFin)
                .OrderByDescending(o => o.FechaOrden)
                .ToListAsync();
        }

        public async Task<IEnumerable<Ordene>> GetAllWithDetailsAsync()
        {
            return await _context.Set<Ordene>()
                .Include(o => o.Cliente)
                .Include(o => o.Detallesordens)
                .Include(o => o.Pagos)
                .OrderByDescending(o => o.FechaOrden)
                .ToListAsync();
        }

        public async Task<Ordene?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Set<Ordene>()
                .Include(o => o.Cliente)
                .Include(o => o.Detallesordens)
                .ThenInclude(d => d.Producto)
                .Include(o => o.Pagos)
                .FirstOrDefaultAsync(o => o.OrdenId == id);
        }
    }
}