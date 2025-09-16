using Lab04_MaytaAlberth.DTOs;
using Lab04_MaytaAlberth.Models;
using Lab04_MaytaAlberth.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Lab04_MaytaAlberth.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PagoController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public PagoController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetPagos()
        {
            var pagos = await _unitOfWork.Pagos.GetAllWithDetailsAsync();
            
            var pagosDto = pagos.Select(p => new PagoResponseDto
            {
                PagoId = p.PagoId,
                OrdenId = p.OrdenId ?? 0,
                NombreCliente = p.Orden?.Cliente?.Nombre ?? "Sin cliente",
                MontoOrden = p.Orden?.Total ?? 0,
                Monto = p.Monto,
                MetodoPago = p.MetodoPago,
                Estado = GetEstadoPago(p.Monto, p.Orden?.Total ?? 0)
            });
            
            return Ok(pagosDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPago(int id)
        {
            var pago = await _unitOfWork.Pagos.GetByIdWithDetailsAsync(id);
            
            if (pago == null)
                return NotFound($"Pago con ID {id} no encontrado");

            var pagoDto = new PagoResponseDto
            {
                PagoId = pago.PagoId,
                OrdenId = pago.OrdenId ?? 0,
                NombreCliente = pago.Orden?.Cliente?.Nombre ?? "Sin cliente",
                MontoOrden = pago.Orden?.Total ?? 0,
                Monto = pago.Monto,
                MetodoPago = pago.MetodoPago,
                Estado = GetEstadoPago(pago.Monto, pago.Orden?.Total ?? 0)
            };

            return Ok(pagoDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePago([FromBody] PagoCreateDto pagoDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ordenExists = await _unitOfWork.Ordenes.ExistsAsync(pagoDto.OrdenId);
            if (!ordenExists)
                return BadRequest("La orden especificada no existe");

            var pago = new Pago
            {
                OrdenId = pagoDto.OrdenId,
                Monto = pagoDto.Monto,
                FechaPago = DateTime.Now,
                MetodoPago = pagoDto.MetodoPago
            };

            await _unitOfWork.Pagos.AddAsync(pago);
            await _unitOfWork.SaveChangesAsync();

            var pagoCreado = await _unitOfWork.Pagos.GetByIdWithDetailsAsync(pago.PagoId);

            var responseDto = new PagoResponseDto
            {
                PagoId = pagoCreado.PagoId,
                OrdenId = pagoCreado.OrdenId ?? 0,
                NombreCliente = pagoCreado.Orden?.Cliente?.Nombre ?? "Sin cliente",
                MontoOrden = pagoCreado.Orden?.Total ?? 0,
                Monto = pagoCreado.Monto,
                MetodoPago = pagoCreado.MetodoPago,
                Estado = GetEstadoPago(pagoCreado.Monto, pagoCreado.Orden?.Total ?? 0)
            };

            return CreatedAtAction(nameof(GetPago), new { id = pago.PagoId }, responseDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePago(int id, [FromBody] PagoUpdateDto pagoDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var pago = await _unitOfWork.Pagos.GetByIdWithDetailsAsync(id);
            if (pago == null)
                return NotFound($"Pago con ID {id} no encontrado");

            var ordenExists = await _unitOfWork.Ordenes.ExistsAsync(pagoDto.OrdenId);
            if (!ordenExists)
                return BadRequest("La orden especificada no existe");

            pago.OrdenId = pagoDto.OrdenId;
            pago.Monto = pagoDto.Monto;
            pago.MetodoPago = pagoDto.MetodoPago;

            await _unitOfWork.Pagos.UpdateAsync(pago);
            await _unitOfWork.SaveChangesAsync();

            var pagoActualizado = await _unitOfWork.Pagos.GetByIdWithDetailsAsync(id);

            var responseDto = new PagoResponseDto
            {
                PagoId = pagoActualizado.PagoId,
                OrdenId = pagoActualizado.OrdenId ?? 0,
                NombreCliente = pagoActualizado.Orden?.Cliente?.Nombre ?? "Sin cliente",
                MontoOrden = pagoActualizado.Orden?.Total ?? 0,
                Monto = pagoActualizado.Monto,
                MetodoPago = pagoActualizado.MetodoPago,
                Estado = GetEstadoPago(pagoActualizado.Monto, pagoActualizado.Orden?.Total ?? 0)
            };

            return Ok(responseDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePago(int id)
        {
            var exists = await _unitOfWork.Pagos.ExistsAsync(id);
            if (!exists)
                return NotFound($"Pago con ID {id} no encontrado");

            await _unitOfWork.Pagos.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        private string GetEstadoPago(decimal montoPago, decimal montoOrden)
        {
            if (montoPago == montoOrden)
                return "Pago Completo";
            else if (montoPago < montoOrden)
                return "Pago Parcial";
            else if (montoPago > montoOrden)
                return "Sobrepago";
            else
                return "Estado Desconocido";
        }
    }
}