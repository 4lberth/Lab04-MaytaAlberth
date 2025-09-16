using System;
using System.Collections.Generic;

namespace Lab04_MaytaAlberth.Models;

public partial class Pago
{
    public int PagoId { get; set; }

    public int? OrdenId { get; set; }

    public decimal Monto { get; set; }

    public DateTime? FechaPago { get; set; }

    public string? MetodoPago { get; set; }

    public virtual Ordene? Orden { get; set; }
}
