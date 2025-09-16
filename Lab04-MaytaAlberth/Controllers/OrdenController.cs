using Lab04_MaytaAlberth.DTOs;
using Lab04_MaytaAlberth.Models;
using Lab04_MaytaAlberth.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Lab04_MaytaAlberth.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdenController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrdenController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrdenes()
        {
            var ordenes = await _unitOfWork.Ordenes.GetAllWithDetailsAsync();
                
            var ordenesDto = ordenes.Select(o => new OrdenResponseDto
            {
                OrdenId = o.OrdenId,
                ClienteId = o.ClienteId ?? 0,
                NombreCliente = o.Cliente?.Nombre ?? "Sin cliente",
                Total = o.Total,
                CantidadDetalles = o.Detallesordens?.Count ?? 0,
                CantidadPagos = o.Pagos?.Count ?? 0,
                TotalPagado = o.Pagos?.Sum(p => p.Monto) ?? 0,
                Estado = GetEstadoOrden(o.Total, o.Pagos?.Sum(p => p.Monto) ?? 0)
            });
                
            return Ok(ordenesDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrden(int id)
        {
            var orden = await _unitOfWork.Ordenes.GetByIdWithDetailsAsync(id);
            
            if (orden == null)
                return NotFound($"Orden con ID {id} no encontrada");

            var ordenDto = new OrdenResponseDto
            {
                OrdenId = orden.OrdenId,
                ClienteId = orden.ClienteId ?? 0,
                NombreCliente = orden.Cliente?.Nombre ?? "Sin cliente",
                Total = orden.Total,
                CantidadDetalles = orden.Detallesordens?.Count ?? 0,
                CantidadPagos = orden.Pagos?.Count ?? 0,
                TotalPagado = orden.Pagos?.Sum(p => p.Monto) ?? 0,
                Estado = GetEstadoOrden(orden.Total, orden.Pagos?.Sum(p => p.Monto) ?? 0)
            };

            return Ok(ordenDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrden([FromBody] OrdenCreateDto ordenDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var clienteExists = await _unitOfWork.Clientes.ExistsAsync(ordenDto.ClienteId);
            if (!clienteExists)
                return BadRequest("El cliente especificado no existe");

            var orden = new Ordene
            {
                ClienteId = ordenDto.ClienteId,
                FechaOrden = DateTime.Now,
                Total = ordenDto.Total
            };

            await _unitOfWork.Ordenes.AddAsync(orden);
            await _unitOfWork.SaveChangesAsync();

            var ordenCreada = await _unitOfWork.Ordenes.GetByIdWithDetailsAsync(orden.OrdenId);

            var responseDto = new OrdenResponseDto
            {
                OrdenId = ordenCreada.OrdenId,
                ClienteId = ordenCreada.ClienteId ?? 0,
                NombreCliente = ordenCreada.Cliente?.Nombre ?? "Sin cliente",
                Total = ordenCreada.Total,
                CantidadDetalles = 0,
                CantidadPagos = 0,
                TotalPagado = 0,
                Estado = "Pendiente"
            };

            return CreatedAtAction(nameof(GetOrden), new { id = orden.OrdenId }, responseDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrden(int id, [FromBody] OrdenUpdateDto ordenDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var orden = await _unitOfWork.Ordenes.GetByIdWithDetailsAsync(id);
            if (orden == null)
                return NotFound($"Orden con ID {id} no encontrada");

            var clienteExists = await _unitOfWork.Clientes.ExistsAsync(ordenDto.ClienteId);
            if (!clienteExists)
                return BadRequest("El cliente especificado no existe");

            orden.ClienteId = ordenDto.ClienteId;
            orden.Total = ordenDto.Total;

            await _unitOfWork.Ordenes.UpdateAsync(orden);
            await _unitOfWork.SaveChangesAsync();

            var ordenActualizada = await _unitOfWork.Ordenes.GetByIdWithDetailsAsync(id);

            var responseDto = new OrdenResponseDto
            {
                OrdenId = ordenActualizada.OrdenId,
                ClienteId = ordenActualizada.ClienteId ?? 0,
                NombreCliente = ordenActualizada.Cliente?.Nombre ?? "Sin cliente",
                Total = ordenActualizada.Total,
                CantidadDetalles = ordenActualizada.Detallesordens?.Count ?? 0,
                CantidadPagos = ordenActualizada.Pagos?.Count ?? 0,
                TotalPagado = ordenActualizada.Pagos?.Sum(p => p.Monto) ?? 0,
                Estado = GetEstadoOrden(ordenActualizada.Total, ordenActualizada.Pagos?.Sum(p => p.Monto) ?? 0)
            };

            return Ok(responseDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrden(int id)
        {
            var exists = await _unitOfWork.Ordenes.ExistsAsync(id);
            if (!exists)
                return NotFound($"Orden con ID {id} no encontrada");

            await _unitOfWork.Ordenes.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        private string GetEstadoOrden(decimal total, decimal totalPagado)
        {
            if (totalPagado == 0)
                return "Pendiente";
            else if (totalPagado < total)
                return "Pago Parcial";
            else if (totalPagado >= total)
                return "Pagado";
            else
                return "Estado Desconocido";
        }
    }
}