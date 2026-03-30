using System;
using System.Collections.Generic;

namespace OEED_ITT.Models;

public partial class Inscripcion
{
    public int IdInscripcion { get; set; }

    public DateTime? HoraInscripcion { get; set; }

    public int? IdUsuario { get; set; }

    public int? IdEvento { get; set; }

    public int? IdEstadoInscripcion { get; set; }

    public virtual EstadoInscripcion? oEstadoI { get; set; }

    public virtual Evento? OEvento { get; set; }

    public virtual Usuario? oUsuario { get; set; }
}
