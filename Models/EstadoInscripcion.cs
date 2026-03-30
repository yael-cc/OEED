using System;
using System.Collections.Generic;

namespace OEED_ITT.Models;

public partial class EstadoInscripcion
{
    public int IdEstadoInscripcion { get; set; }

    public string? NombreEstadoInscripcion { get; set; }

    public string? DescripcionEstadoInscripcion { get; set; }

    public virtual ICollection<Inscripcion> Inscripcions { get; set; } = new List<Inscripcion>();
}
