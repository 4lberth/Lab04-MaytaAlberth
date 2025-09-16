using Lab04_MaytaAlberth;
using Lab04_MaytaAlberth.Models;
using Lab04_MaytaAlberth.Repositories;
using Lab04_MaytaAlberth.Repositories.Interfaces;

public class UnitOfWork : IUnitOfWork
{
    private readonly TiendaContext _context;

    public IClienteRepository Clientes { get; }
    public IProductoRepository Productos { get; }
    public ICategoriaRepository Categorias { get; }
    public IOrdenRepository Ordenes { get; }
    public IDetalleOrdenRepository DetallesOrden { get; }
    public IPagoRepository Pagos { get; }

    public UnitOfWork(TiendaContext context)
    {
        _context = context;
        Clientes = new ClienteRepository(_context);
        Productos = new ProductoRepository(_context);
        Categorias = new CategoriaRepository(_context);
        Ordenes = new OrdenRepository(_context);
        DetallesOrden = new DetalleOrdenRepository(_context);
        Pagos = new PagoRepository(_context);
    }

    public int SaveChanges()
    {
        return _context.SaveChanges();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}