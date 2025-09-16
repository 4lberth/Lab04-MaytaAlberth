using Lab04_MaytaAlberth.DTOs;
using Lab04_MaytaAlberth.Models;
using Lab04_MaytaAlberth.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Lab04_MaytaAlberth.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriaController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoriaController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategorias()
        {
            var categorias = await _unitOfWork.Categorias.GetAllWithProductsAsync();
            
            var categoriasDto = categorias.Select(c => new CategoriaResponseDto
            {
                Nombre = c.Nombre,
                CantidadProductos = c.Productos?.Count ?? 0
            });
            
            return Ok(categoriasDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoria(int id)
        {
            var categoria = await _unitOfWork.Categorias.GetByIdWithProductsAsync(id);
            
            if (categoria == null)
                return NotFound($"Categoría con ID {id} no encontrada");

            var categoriaDto = new CategoriaResponseDto
            {
                Nombre = categoria.Nombre,
                CantidadProductos = categoria.Productos?.Count ?? 0
            };

            return Ok(categoriaDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategoria([FromBody] CategoriaCreateDto categoriaDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingCategoria = await _unitOfWork.Categorias.GetByNombreAsync(categoriaDto.Nombre);
            if (existingCategoria != null)
                return Conflict("Ya existe una categoría con ese nombre");

            var categoria = new Categoria
            {
                Nombre = categoriaDto.Nombre
            };

            await _unitOfWork.Categorias.AddAsync(categoria);
            await _unitOfWork.SaveChangesAsync();

            var responseDto = new CategoriaResponseDto
            {
                Nombre = categoria.Nombre,
                CantidadProductos = 0
            };

            return CreatedAtAction(nameof(GetCategoria), new { id = categoria.CategoriaId }, responseDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategoria(int id, [FromBody] CategoriaUpdateDto categoriaDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var categoria = await _unitOfWork.Categorias.GetByIdWithProductsAsync(id);
            if (categoria == null)
                return NotFound($"Categoría con ID {id} no encontrada");

            var existingCategoria = await _unitOfWork.Categorias.GetByNombreAsync(categoriaDto.Nombre);
            if (existingCategoria != null && existingCategoria.CategoriaId != id)
                return Conflict("Ya existe otra categoría con ese nombre");

            categoria.Nombre = categoriaDto.Nombre;

            await _unitOfWork.Categorias.UpdateAsync(categoria);
            await _unitOfWork.SaveChangesAsync();

            var responseDto = new CategoriaResponseDto
            {
                Nombre = categoria.Nombre,
                CantidadProductos = categoria.Productos?.Count ?? 0
            };

            return Ok(responseDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoria(int id)
        {
            var exists = await _unitOfWork.Categorias.ExistsAsync(id);
            if (!exists)
                return NotFound($"Categoría con ID {id} no encontrada");

            await _unitOfWork.Categorias.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }
    }
}