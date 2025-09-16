using Lab04_MaytaAlberth.DTOs;
using Lab04_MaytaAlberth.Models;
using Lab04_MaytaAlberth.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Lab04_MaytaAlberth.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClienteController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetClientes()
        {
            var clientes = await _unitOfWork.Clientes.GetAllWithOrdersAsync(); 

            var clientesDto = clientes.Select(c => new ClienteResponseDto
            {
                Nombre = c.Nombre,
                Correo = c.Correo,
                CantidadOrdenes = c.Ordenes?.Count ?? 0
            });

            return Ok(clientesDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCliente(int id)
        {
            var cliente = await _unitOfWork.Clientes.GetByIdWithOrdersAsync(id); 

            if (cliente == null)
                return NotFound($"Cliente con ID {id} no encontrado");

            var clienteDto = new ClienteResponseDto
            {
                Nombre = cliente.Nombre,
                Correo = cliente.Correo,
                CantidadOrdenes = cliente.Ordenes?.Count ?? 0
            };

            return Ok(clienteDto);
        }
        

        [HttpPost]
        public async Task<IActionResult> CreateCliente([FromBody] ClienteCreateDto clienteDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingCliente = await _unitOfWork.Clientes.GetByEmailAsync(clienteDto.Correo);
            if (existingCliente != null)
                return Conflict("Ya existe un cliente con este correo");

            var cliente = new Cliente
            {
                Nombre = clienteDto.Nombre,
                Correo = clienteDto.Correo,
            };

            await _unitOfWork.Clientes.AddAsync(cliente);
            await _unitOfWork.SaveChangesAsync();

            var responseDto = new ClienteResponseDto
            {
                Nombre = cliente.Nombre,
                Correo = cliente.Correo,
                CantidadOrdenes = 0
            };

            return CreatedAtAction(nameof(GetCliente), new { id = cliente.ClienteId }, responseDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCliente(int id, [FromBody] ClienteUpdateDto clienteDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cliente = await _unitOfWork.Clientes.GetByIdWithOrdersAsync(id);
            if (cliente == null)
                return NotFound($"Cliente con ID {id} no encontrado");

            var existingCliente = await _unitOfWork.Clientes.GetByEmailAsync(clienteDto.Correo);
            if (existingCliente != null && existingCliente.ClienteId != id)
                return Conflict("Ya existe otro cliente con este correo");

            cliente.Nombre = clienteDto.Nombre;
            cliente.Correo = clienteDto.Correo;

            await _unitOfWork.Clientes.UpdateAsync(cliente);
            await _unitOfWork.SaveChangesAsync();

            var responseDto = new ClienteResponseDto
            {
                Nombre = cliente.Nombre,
                Correo = cliente.Correo,
                CantidadOrdenes = cliente.Ordenes?.Count ?? 0
            };

            return Ok(responseDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var exists = await _unitOfWork.Clientes.ExistsAsync(id);
            if (!exists)
                return NotFound($"Cliente con ID {id} no encontrado");

            await _unitOfWork.Clientes.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }
    }
}