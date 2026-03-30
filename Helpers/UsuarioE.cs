using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OEED_ITT.Helpers;

public partial class UsuarioE
{
    [Required(ErrorMessage = "El número de control es obligatorio.")]
    [StringLength(8, MinimumLength = 8, ErrorMessage = "El número de control debe tener 8 dígitos.")]
    public int IdUsuario { get; set; }

    public string? NombreUsuario { get; set; }

    public string? ApellidoUsuario { get; set; }

    public int? Idrol { get; set; }

    public string? Iddepartamento { get; set; }

    public string? NombreDepartamento { get; set; }

    public string? NombrePfUsuario { get; set; }

    public string? CorreoUsuario { get; set; }

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    public string? ContrasenaUsuario { get; set; }

}

