using Lab04_MaytaAlberth;
using Lab04_MaytaAlberth.Models;
using Lab04_MaytaAlberth.Repositories.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IClienteRepository Clientes { get; }
    IProductoRepository Productos { get; }
    ICategoriaRepository Categorias { get; }
    IOrdenRepository Ordenes { get; }
    IDetalleOrdenRepository DetallesOrden { get; }
    IPagoRepository Pagos { get; }
        
    int SaveChanges();
    Task<int> SaveChangesAsync();
}