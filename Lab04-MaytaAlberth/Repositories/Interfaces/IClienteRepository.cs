using Lab04_MaytaAlberth;
using Lab04_MaytaAlberth.Models;
using Lab04_MaytaAlberth.Repositories.Interfaces;

public interface IClienteRepository : IRepository<Cliente>
{
    Task<Cliente?> GetByEmailAsync(string correo);
    Task<IEnumerable<Cliente>> GetAllWithOrdersAsync();
    Task<Cliente?> GetByIdWithOrdersAsync(int id); 
}