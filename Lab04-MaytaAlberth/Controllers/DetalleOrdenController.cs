using Lab04_MaytaAlberth.DTOs;
using Lab04_MaytaAlberth.Models;
using Lab04_MaytaAlberth.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Lab04_MaytaAlberth.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DetalleOrdenController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public DetalleOrdenController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetDetallesOrden()
        {
            var detalles = await _unitOfWork.DetallesOrden.GetAllWithDetailsAsync();
            
            var detallesDto = detalles.Select(d => new DetalleOrdenResponseDto
            {
                DetalleId = d.DetalleId,
                OrdenId = d.OrdenId ?? 0,
                ProductoId = d.ProductoId ?? 0,
                NombreProducto = d.Producto?.Nombre ?? "Sin producto",
                Cantidad = d.Cantidad,
                Precio = d.Precio,
                Subtotal = d.Cantidad * d.Precio
            });
            
            return Ok(detallesDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetalleOrden(int id)
        {
            var detalle = await _unitOfWork.DetallesOrden.GetByIdWithDetailsAsync(id);
            
            if (detalle == null)
                return NotFound($"Detalle de orden con ID {id} no encontrado");

            var detalleDto = new DetalleOrdenResponseDto
            {
                DetalleId = detalle.DetalleId,
                OrdenId = detalle.OrdenId ?? 0,
                ProductoId = detalle.ProductoId ?? 0,
                NombreProducto = detalle.Producto?.Nombre ?? "Sin producto",
                Cantidad = detalle.Cantidad,
                Precio = detalle.Precio,
                Subtotal = detalle.Cantidad * detalle.Precio
            };

            return Ok(detalleDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDetalleOrden([FromBody] DetalleOrdenCreateDto detalleDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ordenExists = await _unitOfWork.Ordenes.ExistsAsync(detalleDto.OrdenId);
            if (!ordenExists)
                return BadRequest("La orden especificada no existe");

            var productoExists = await _unitOfWork.Productos.ExistsAsync(detalleDto.ProductoId);
            if (!productoExists)
                return BadRequest("El producto especificado no existe");

            var detalle = new Detallesorden
            {
                OrdenId = detalleDto.OrdenId,
                ProductoId = detalleDto.ProductoId,
                Cantidad = detalleDto.Cantidad,
                Precio = detalleDto.Precio
            };

            await _unitOfWork.DetallesOrden.AddAsync(detalle);
            await _unitOfWork.SaveChangesAsync();

            var detalleCreado = await _unitOfWork.DetallesOrden.GetByIdWithDetailsAsync(detalle.DetalleId);

            var responseDto = new DetalleOrdenResponseDto
            {
                DetalleId = detalleCreado.DetalleId,
                OrdenId = detalleCreado.OrdenId ?? 0,
                ProductoId = detalleCreado.ProductoId ?? 0,
                NombreProducto = detalleCreado.Producto?.Nombre ?? "Sin producto",
                Cantidad = detalleCreado.Cantidad,
                Precio = detalleCreado.Precio,
                Subtotal = detalleCreado.Cantidad * detalleCreado.Precio
            };

            return CreatedAtAction(nameof(GetDetalleOrden), new { id = detalle.DetalleId }, responseDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDetalleOrden(int id, [FromBody] DetalleOrdenUpdateDto detalleDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var detalle = await _unitOfWork.DetallesOrden.GetByIdWithDetailsAsync(id);
            if (detalle == null)
                return NotFound($"Detalle de orden con ID {id} no encontrado");

            var ordenExists = await _unitOfWork.Ordenes.ExistsAsync(detalleDto.OrdenId);
            if (!ordenExists)
                return BadRequest("La orden especificada no existe");

            var productoExists = await _unitOfWork.Productos.ExistsAsync(detalleDto.ProductoId);
            if (!productoExists)
                return BadRequest("El producto especificado no existe");

            detalle.OrdenId = detalleDto.OrdenId;
            detalle.ProductoId = detalleDto.ProductoId;
            detalle.Cantidad = detalleDto.Cantidad;
            detalle.Precio = detalleDto.Precio;

            await _unitOfWork.DetallesOrden.UpdateAsync(detalle);
            await _unitOfWork.SaveChangesAsync();

            var detalleActualizado = await _unitOfWork.DetallesOrden.GetByIdWithDetailsAsync(id);

            var responseDto = new DetalleOrdenResponseDto
            {
                DetalleId = detalleActualizado.DetalleId,
                OrdenId = detalleActualizado.OrdenId ?? 0,
                ProductoId = detalleActualizado.ProductoId ?? 0,
                NombreProducto = detalleActualizado.Producto?.Nombre ?? "Sin producto",
                Cantidad = detalleActualizado.Cantidad,
                Precio = detalleActualizado.Precio,
                Subtotal = detalleActualizado.Cantidad * detalleActualizado.Precio
            };

            return Ok(responseDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDetalleOrden(int id)
        {
            var exists = await _unitOfWork.DetallesOrden.ExistsAsync(id);
            if (!exists)
                return NotFound($"Detalle de orden con ID {id} no encontrado");

            await _unitOfWork.DetallesOrden.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }
    }
}