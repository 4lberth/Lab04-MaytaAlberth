using System.ComponentModel.DataAnnotations;

namespace Lab04_MaytaAlberth.DTOs
{
    public class DetalleOrdenCreateDto
    {
        public int OrdenId { get; set; }

        public int ProductoId { get; set; }
        
        public int Cantidad { get; set; }

        public decimal Precio { get; set; }
    }

    public class DetalleOrdenUpdateDto
    {
        public int OrdenId { get; set; }

        public int ProductoId { get; set; }

        public int Cantidad { get; set; }

        public decimal Precio { get; set; }
    }
    
    public class DetalleOrdenResponseDto
    {
        public int DetalleId { get; set; }
        public int OrdenId { get; set; }
        public int ProductoId { get; set; }
        public string NombreProducto { get; set; } = null!;
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
        public decimal Subtotal { get; set; }
    }
}