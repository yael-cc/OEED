using System;
using System.Collections.Generic;

namespace OEED_ITT.Models;

public partial class Departamento
{
    public string IdDepartamento { get; set; } = null!;

    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }

    public virtual ICollection<Evento> Eventos { get; set; } = new List<Evento>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
