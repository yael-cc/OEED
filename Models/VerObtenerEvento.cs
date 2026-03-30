using System;
using System.Collections.Generic;

namespace OEED_ITT.Models;

public partial class VerObtenerEvento
{
    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }

    public DateOnly? Fecha { get; set; }

    public string? Tipo { get; set; }

    public DateTime? Comienzo { get; set; }

    public DateTime? Finalizacion { get; set; }

    public string? Lugar { get; set; }

    public string? DepartamentoDeOrigen { get; set; }

    public string? Estado { get; set; }

    public int? Capacidad { get; set; }

    public string? Imagen { get; set; }
}
