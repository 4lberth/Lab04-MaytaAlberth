using System.ComponentModel.DataAnnotations;

namespace Lab04_MaytaAlberth.DTOs
{
    public class ProductoCreateDto
    {
        public string Nombre { get; set; } = null!;

        public string? Descripcion { get; set; }

        public decimal Precio { get; set; }

        public int Stock { get; set; }

        public int? CategoriaId { get; set; }
    }

    public class ProductoUpdateDto
    {
        public string Nombre { get; set; } = null!;

        public string? Descripcion { get; set; }

        public decimal Precio { get; set; }

        public int Stock { get; set; }

        public int? CategoriaId { get; set; }
    }
    
    public class ProductoResponseDto
    {
        public int ProductoId { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string NombreCategoria { get; set; } = null!;
        public int CantidadVentas { get; set; }
        public string Estado { get; set; } = null!;
    }
}