using System.ComponentModel.DataAnnotations;

namespace Lab04_MaytaAlberth.DTOs
{
    public class PagoCreateDto
    {
        public int OrdenId { get; set; }

        public decimal Monto { get; set; }

        public string? MetodoPago { get; set; }
    }

    public class PagoUpdateDto
    {
        public int OrdenId { get; set; }

        public decimal Monto { get; set; }

        public string? MetodoPago { get; set; }
    }
    
    public class PagoResponseDto
    {
        public int PagoId { get; set; }
        public int OrdenId { get; set; }
        public string NombreCliente { get; set; } = null!;
        public decimal MontoOrden { get; set; }
        public decimal Monto { get; set; }
        public string? MetodoPago { get; set; }
        public string Estado { get; set; } = null!;
    }
}