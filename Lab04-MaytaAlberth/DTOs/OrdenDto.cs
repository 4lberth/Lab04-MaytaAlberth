using System.ComponentModel.DataAnnotations;

namespace Lab04_MaytaAlberth.DTOs
{
    public class OrdenCreateDto
    {
        public int ClienteId { get; set; }

        public decimal Total { get; set; }
    }

    public class OrdenUpdateDto
    {
        public int ClienteId { get; set; }

        public decimal Total { get; set; }
    }
    
    public class OrdenResponseDto
    {
        public int OrdenId { get; set; }
        public int ClienteId { get; set; }
        public string NombreCliente { get; set; } = null!;
        public decimal Total { get; set; }
        public int CantidadDetalles { get; set; }
        public int CantidadPagos { get; set; }
        public decimal TotalPagado { get; set; }
        public string Estado { get; set; } = null!;
    }
}