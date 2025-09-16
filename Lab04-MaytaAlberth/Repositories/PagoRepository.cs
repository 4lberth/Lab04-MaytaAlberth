using Lab04_MaytaAlberth.Models;
using Lab04_MaytaAlberth.Repositories;
using Lab04_MaytaAlberth.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Lab04_MaytaAlberth.Repositories
{
    public class PagoRepository : Repository<Pago>, IPagoRepository
    {
        public PagoRepository(TiendaContext context) : base(context) { }

        public async Task<IEnumerable<Pago>> GetByOrdenAsync(int ordenId)
        {
            return await _context.Set<Pago>()
                .Include(p => p.Orden)
                .ThenInclude(o => o.Cliente)
                .Where(p => p.OrdenId == ordenId)
                .OrderByDescending(p => p.FechaPago)
                .ToListAsync();
        }

        public async Task<IEnumerable<Pago>> GetByMetodoPagoAsync(string metodoPago)
        {
            return await _context.Set<Pago>()
                .Include(p => p.Orden)
                .ThenInclude(o => o.Cliente)
                .Where(p => p.MetodoPago == metodoPago)
                .OrderByDescending(p => p.FechaPago)
                .ToListAsync();
        }

        public async Task<IEnumerable<Pago>> GetByFechaRangoAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            return await _context.Set<Pago>()
                .Include(p => p.Orden)
                .ThenInclude(o => o.Cliente)
                .Where(p => p.FechaPago >= fechaInicio && p.FechaPago <= fechaFin)
                .OrderByDescending(p => p.FechaPago)
                .ToListAsync();
        }

        public async Task<IEnumerable<Pago>> GetAllWithDetailsAsync()
        {
            return await _context.Set<Pago>()
                .Include(p => p.Orden)
                .ThenInclude(o => o.Cliente)
                .OrderByDescending(p => p.FechaPago)
                .ToListAsync();
        }

        public async Task<Pago?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Set<Pago>()
                .Include(p => p.Orden)
                .ThenInclude(o => o.Cliente)
                .FirstOrDefaultAsync(p => p.PagoId == id);
        }
    }
}