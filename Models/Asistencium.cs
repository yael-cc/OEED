using System;
using System.Collections.Generic;

namespace OEED_ITT.Models;

public partial class Asistencium
{
    public int IdAsistencia { get; set; }

    public int? IdEvento { get; set; }

    public int? IdUsuario { get; set; }

    public DateTime? HoraAsistencia { get; set; }

    public string? DetallesAsistencia { get; set; }

    public virtual Evento? IdEventoNavigation { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }
}
