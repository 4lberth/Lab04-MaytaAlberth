using System.ComponentModel.DataAnnotations;

namespace Lab04_MaytaAlberth.DTOs
{
    public class ClienteCreateDto
    {
        public string Nombre { get; set; } = null!;
        public string Correo { get; set; } = null!;
        
    }

    public class ClienteUpdateDto
    {
        public string Nombre { get; set; } = null!;
        public string Correo { get; set; } = null!;
    }
    
    public class ClienteResponseDto
    {
        public string Nombre { get; set; } = null!;
        public string Correo { get; set; } = null!;
        public int CantidadOrdenes { get; set; } 
    }
}