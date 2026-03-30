using System;
using System.Collections.Generic;

namespace OEED_ITT.Models;

public partial class TipoEvento
{
    public int IdTipoEvento { get; set; }

    public string? NombreTipoEvento { get; set; }

    public string? DescripcionTipoEvento { get; set; }

    public virtual ICollection<Evento> Eventos { get; set; } = new List<Evento>();
}
