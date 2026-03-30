using System;
using System.Collections.Generic;

namespace OEED_ITT.Models;

public partial class GrupoOrganizador
{
    public int IdGrupoOrganizador { get; set; }
    public int? IdEvento { get; set; }

    public int? IdUsuario { get; set; }

    public virtual Evento? IdEventoNavigation { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }
}
