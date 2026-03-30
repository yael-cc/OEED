using System;
using System.Collections.Generic;

namespace OEED_ITT.Models;

public partial class Lugar
{
    public int IdLugar { get; set; }

    public string? NombreLugar { get; set; }

    public int? CapacidadLugar { get; set; }

    public string? DescripcionLugar { get; set; }

    public virtual ICollection<Evento> Eventos { get; set; } = new List<Evento>();
}
