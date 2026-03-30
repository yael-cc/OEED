using System;
using System.Collections.Generic;

namespace OEED_ITT.Models;

public partial class DepartamentoInvitado
{
    public int IdDepartamentoInvitado { get; set; }
    public int? IdEvento { get; set; }

    public string? IdDepartamento { get; set; }

    public virtual Departamento? IdDepartamentoNavigation { get; set; }

    public virtual Evento? IdEventoNavigation { get; set; }
}
