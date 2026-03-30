using System;
using System.Collections.Generic;

namespace OEED_ITT.Models;

public partial class EstadoEvento
{
    public int IdEstadoEvento { get; set; }

    public string? NombreEstadoEvento { get; set; }

    public string? DescripcionEstadoEvento { get; set; }

    public virtual ICollection<Evento> Eventos { get; set; } = new List<Evento>();
}
