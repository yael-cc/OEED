using System;
using System.Collections.Generic;

namespace OEED_ITT.Models;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public partial class Evento : IValidatableObject
{
    public int IdEvento { get; set; }

    [Required(ErrorMessage = "El nombre del evento es obligatorio")]
    [StringLength(30, ErrorMessage = "El nombre del evento no puede superar los 30 caracteres")]
    public string? NombreEvento { get; set; }

    [Required(ErrorMessage = "La descripcion del evento es obligatoria")]
    [StringLength(255, ErrorMessage = "La descripción del evento no puede superar los 255 caracteres")]
    public string? DescripcionEvento { get; set; }

    [Required(ErrorMessage = "La fecha del evento es obligatoria")]
    public DateOnly? FechaEvento { get; set; }

    [Required(ErrorMessage = "El tipo de evento es obligatorio")]
    public int? IdTipoEvento { get; set; }

    [Required(ErrorMessage = "La hora de inicio del evento es obligatoria")]
    public DateTime? HoraInicioEvento { get; set; }

    [Required(ErrorMessage = "La hora de fin del evento es obligatoria")]
    public DateTime? HoraFinEvento { get; set; }

    [Required(ErrorMessage = "El lugar del evento es obligatorio")]
    public int? IdlugarEvento { get; set; }

    [Required(ErrorMessage = "El departamento de origen es obligatorio")]
    public string? IddepartamentoOrigen { get; set; }

    [Required(ErrorMessage = "El estado del evento es obligatorio")]
    public int? IdEstadoEvento { get; set; }

    [Range(5, 10000, ErrorMessage = "El número máximo de asistentes debe ser al menos de 5")]
    public int? NumMaxEvento { get; set; }

    public string? Imagen { get; set; }

    public virtual ICollection<ActividadesEvento> ActividadesEventos { get; set; } = new List<ActividadesEvento>();

    public virtual ICollection<Asistencium> Asistencia { get; set; } = new List<Asistencium>();

    public virtual ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();

    public virtual EstadoEvento? oEstado { get; set; }

    public virtual TipoEvento? oTipoEvento { get; set; }

    public virtual Departamento? oDepartamento { get; set; }

    public virtual Lugar? oLugar { get; set; }

    public virtual ICollection<Inscripcion> Inscripcions { get; set; } = new List<Inscripcion>();

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var results = new List<ValidationResult>();

        // Validar que HoraInicioEvento sea mayor o igual a FechaEvento
        if (FechaEvento.HasValue && HoraInicioEvento.HasValue)
        {
            var fechaHoraInicio = new DateTime(
                FechaEvento.Value.Year,
                FechaEvento.Value.Month,
                FechaEvento.Value.Day,
                HoraInicioEvento.Value.Hour,
                HoraInicioEvento.Value.Minute,
                HoraInicioEvento.Value.Second
            );

            if (HoraInicioEvento < fechaHoraInicio)
            {
                results.Add(new ValidationResult(
                    "La hora de inicio del evento debe ser mayor o igual a la fecha del evento.",
                    new[] { nameof(HoraInicioEvento) }
                ));
            }
        }

        // Validar que HoraFinEvento sea mayor a HoraInicioEvento
        if (HoraInicioEvento.HasValue && HoraFinEvento.HasValue)
        {
            if (HoraFinEvento <= HoraInicioEvento)
            {
                results.Add(new ValidationResult(
                    "La hora de fin del evento debe ser mayor a la hora de inicio del evento.",
                    new[] { nameof(HoraFinEvento) }
                ));
            }
        }

        return results;
    }
}