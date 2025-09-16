using Lab04_MaytaAlberth;
using Lab04_MaytaAlberth.Models;
using Lab04_MaytaAlberth.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Lab04_MaytaAlberth.Repositories;

public class ClienteRepository : Repository<Cliente>, IClienteRepository
{
    public ClienteRepository(TiendaContext context) : base(context) { }

    public async Task<Cliente?> GetByEmailAsync(string correo)
    {
        return await _context.Set<Cliente>()
            .Include(c => c.Ordenes)
            .FirstOrDefaultAsync(c => c.Correo == correo);
    }
    
    public async Task<IEnumerable<Cliente>> GetAllWithOrdersAsync()
    {
        return await _context.Set<Cliente>()
            .Include(c => c.Ordenes)
            .ToListAsync();
    }
    
    public async Task<Cliente?> GetByIdWithOrdersAsync(int id)
    {
        return await _context.Set<Cliente>()
            .Include(c => c.Ordenes)
            .FirstOrDefaultAsync(c => c.ClienteId == id);
    }
}