using System.ComponentModel.DataAnnotations;

namespace Lab04_MaytaAlberth.DTOs
{
    public class CategoriaCreateDto
    {
        public string Nombre { get; set; } = null!;
    }

    public class CategoriaUpdateDto
    {
        public string Nombre { get; set; } = null!;
    }
    
    public class CategoriaResponseDto
    {
        public string Nombre { get; set; } = null!;
        public int CantidadProductos { get; set; }
    }
}