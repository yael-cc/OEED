using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OEED_ITT.Models
{
    public partial class ActividadesEvento : IValidatableObject
    {
        [Key]
        public int IdActividad { get; set; }

        [Required(ErrorMessage = "El nombre de la actividad es obligatorio.")]
        [StringLength(30, ErrorMessage = "El nombre de la actividad no puede exceder los 30 caracteres.")]
        public string? NombreActividad { get; set; }

        [Required(ErrorMessage = "La descripción de la actividad es obligatoria.")]
        [StringLength(255, ErrorMessage = "La descripción de la actividad no puede exceder los 255 caracteres.")]
        public string? DescripcionActividad { get; set; }

        [Required(ErrorMessage = "La hora de inicio es obligatoria.")]
        [DataType(DataType.DateTime, ErrorMessage = "La hora de inicio debe ser una fecha y hora válida.")]
        public DateTime? HoraInicio { get; set; }

        [Required(ErrorMessage = "La hora de fin es obligatoria.")]
        [DataType(DataType.DateTime, ErrorMessage = "La hora de fin debe ser una fecha y hora válida.")]
        public DateTime? HoraFin { get; set; }

        [Required(ErrorMessage = "El ID del evento es obligatorio.")]
        public int? IdEvento { get; set; }

        public virtual Evento? IdEventoNavigation { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            if (HoraInicio.HasValue && HoraFin.HasValue)
            {
                if (HoraFin <= HoraInicio)
                {
                    results.Add(new ValidationResult(
                        "La hora de fin debe ser mayor a la hora de inicio.",
                        new[] { nameof(HoraFin) }
                    ));
                }
            }
            return results;
        }
    }
}