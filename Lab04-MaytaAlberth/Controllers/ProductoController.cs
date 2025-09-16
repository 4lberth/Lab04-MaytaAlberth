using Lab04_MaytaAlberth.DTOs;
using Lab04_MaytaAlberth.Models;
using Lab04_MaytaAlberth.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Lab04_MaytaAlberth.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductoController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductoController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetProductos()
        {
            var productos = await _unitOfWork.Productos.GetAllWithDetailsAsync();
            
            var productosDto = productos.Select(p => new ProductoResponseDto
            {
                ProductoId = p.ProductoId,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                Precio = p.Precio,
                Stock = p.Stock,
                NombreCategoria = p.Categoria?.Nombre ?? "Sin categoría",
                CantidadVentas = p.Detallesordens?.Count ?? 0,
                Estado = GetEstadoProducto(p.Stock)
            });
            
            return Ok(productosDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProducto(int id)
        {
            var producto = await _unitOfWork.Productos.GetByIdWithDetailsAsync(id);
            
            if (producto == null)
                return NotFound($"Producto con ID {id} no encontrado");

            var productoDto = new ProductoResponseDto
            {
                ProductoId = producto.ProductoId,
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                Precio = producto.Precio,
                Stock = producto.Stock,
                NombreCategoria = producto.Categoria?.Nombre ?? "Sin categoría",
                CantidadVentas = producto.Detallesordens?.Count ?? 0,
                Estado = GetEstadoProducto(producto.Stock)
            };

            return Ok(productoDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProducto([FromBody] ProductoCreateDto productoDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Verificar que la categoría existe si se especifica
            if (productoDto.CategoriaId.HasValue)
            {
                var categoriaExists = await _unitOfWork.Categorias.ExistsAsync(productoDto.CategoriaId.Value);
                if (!categoriaExists)
                    return BadRequest("La categoría especificada no existe");
            }

            var producto = new Producto
            {
                Nombre = productoDto.Nombre,
                Descripcion = productoDto.Descripcion,
                Precio = productoDto.Precio,
                Stock = productoDto.Stock,
                CategoriaId = productoDto.CategoriaId
            };

            await _unitOfWork.Productos.AddAsync(producto);
            await _unitOfWork.SaveChangesAsync();

            // Cargar el producto con las relaciones para la respuesta
            var productoCreado = await _unitOfWork.Productos.GetByIdWithDetailsAsync(producto.ProductoId);

            var responseDto = new ProductoResponseDto
            {
                ProductoId = productoCreado.ProductoId,
                Nombre = productoCreado.Nombre,
                Descripcion = productoCreado.Descripcion,
                Precio = productoCreado.Precio,
                Stock = productoCreado.Stock,
                NombreCategoria = productoCreado.Categoria?.Nombre ?? "Sin categoría",
                CantidadVentas = 0,
                Estado = GetEstadoProducto(productoCreado.Stock)
            };

            return CreatedAtAction(nameof(GetProducto), new { id = producto.ProductoId }, responseDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProducto(int id, [FromBody] ProductoUpdateDto productoDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var producto = await _unitOfWork.Productos.GetByIdWithDetailsAsync(id);
            if (producto == null)
                return NotFound($"Producto con ID {id} no encontrado");

            // Verificar que la categoría existe si se especifica
            if (productoDto.CategoriaId.HasValue)
            {
                var categoriaExists = await _unitOfWork.Categorias.ExistsAsync(productoDto.CategoriaId.Value);
                if (!categoriaExists)
                    return BadRequest("La categoría especificada no existe");
            }

            producto.Nombre = productoDto.Nombre;
            producto.Descripcion = productoDto.Descripcion;
            producto.Precio = productoDto.Precio;
            producto.Stock = productoDto.Stock;
            producto.CategoriaId = productoDto.CategoriaId;

            await _unitOfWork.Productos.UpdateAsync(producto);
            await _unitOfWork.SaveChangesAsync();

            // Recargar para obtener las relaciones actualizadas
            var productoActualizado = await _unitOfWork.Productos.GetByIdWithDetailsAsync(id);

            var responseDto = new ProductoResponseDto
            {
                ProductoId = productoActualizado.ProductoId,
                Nombre = productoActualizado.Nombre,
                Descripcion = productoActualizado.Descripcion,
                Precio = productoActualizado.Precio,
                Stock = productoActualizado.Stock,
                NombreCategoria = productoActualizado.Categoria?.Nombre ?? "Sin categoría",
                CantidadVentas = productoActualizado.Detallesordens?.Count ?? 0,
                Estado = GetEstadoProducto(productoActualizado.Stock)
            };

            return Ok(responseDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            var exists = await _unitOfWork.Productos.ExistsAsync(id);
            if (!exists)
                return NotFound($"Producto con ID {id} no encontrado");

            await _unitOfWork.Productos.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        // Método auxiliar para determinar el estado del producto
        private string GetEstadoProducto(int stock)
        {
            if (stock == 0)
                return "Agotado";
            else if (stock < 10)
                return "Stock Bajo";
            else
                return "Disponible";
        }
    }
}