using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OEED_ITT.Models;

public partial class Comentario
{
    public int IdComentario { get; set; }
    [Required(ErrorMessage = "El asunto del comentario es obligatorio")]
    [StringLength(30, ErrorMessage = "El asunto del evento no puede superar los 30 caracteres")]
    public string? AsuntoComentario { get; set; }
    [Required(ErrorMessage = "La descripcion del comentario es obligatoria")]
    [StringLength(255, ErrorMessage = "El nombre del evento no puede superar los 255 caracteres")]
    public string? DescripcionComentario { get; set; }

    public DateTime? FechaComentario { get; set; }

    public int? IdUsuario { get; set; }

    public int? IdEvento { get; set; }

    public virtual Evento? IdEventoNavigation { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }
}
