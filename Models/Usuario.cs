using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OEED_ITT.Models
{
    public partial class Usuario
    {
        [Key]
        [Required(ErrorMessage = "El numero de control del usuario es obligatorio")]
        [Range(10000000, 99999999, ErrorMessage = "El numero de control del usuario debe ser de 8 digitos")]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        [StringLength(50, ErrorMessage = "El nombre de usuario no puede exceder los 50 caracteres")]
        public string? NombreUsuario { get; set; }

        [Required(ErrorMessage = "El apellido de usuario es obligatorio")]
        [StringLength(50, ErrorMessage = "El apellido de usuario no puede exceder los 50 caracteres")]
        public string? ApellidoUsuario { get; set; }

        [Required(ErrorMessage = "El rol es obligatorio")]
        public int? Idrol { get; set; }

        [Required(ErrorMessage = "El departamento es obligatorio")]
        public string? Iddepartamento { get; set; }

        [Required(ErrorMessage = "El nombre de perfil es obligatorio")]
        [StringLength(50, ErrorMessage = "El nombre de perfil no puede exceder los 50 caracteres")]
        public string? NombrePfUsuario { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio")]
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido")]
        public string? CorreoUsuario { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 100 caracteres")]
        public string? ContrasenaUsuario { get; set; }

        public virtual ICollection<Asistencium> Asistencia { get; set; } = new List<Asistencium>();

        public virtual ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();

        public virtual Departamento? IddepartamentoNavigation { get; set; }

        public virtual Rol? IdrolNavigation { get; set; }

        public virtual ICollection<Inscripcion> Inscripcions { get; set; } = new List<Inscripcion>();
    }
}