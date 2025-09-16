using Lab04_MaytaAlberth.Models;
using Lab04_MaytaAlberth.Repositories;
using Lab04_MaytaAlberth.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Lab04_MaytaAlberth.Repositories
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(TiendaContext context) : base(context) { }

        public async Task<Categoria?> GetByNombreAsync(string nombre)
        {
            return await _context.Set<Categoria>()
                .FirstOrDefaultAsync(c => c.Nombre == nombre);
        }

        public async Task<IEnumerable<Categoria>> GetAllWithProductsAsync()
        {
            return await _context.Set<Categoria>()
                .Include(c => c.Productos)
                .ToListAsync();
        }

        public async Task<Categoria?> GetByIdWithProductsAsync(int id)
        {
            return await _context.Set<Categoria>()
                .Include(c => c.Productos)
                .FirstOrDefaultAsync(c => c.CategoriaId == id);
        }
    }
}