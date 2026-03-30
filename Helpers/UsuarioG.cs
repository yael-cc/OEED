using Microsoft.EntityFrameworkCore;
using OEED_ITT.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OEED_ITT.Helpers;

public static class UsuarioG
{
    static EventosInstitucionalesContext context;

    static UsuarioG()
    {
        context = new EventosInstitucionalesContext(); // Inicialización en el bloque estático
    }


    [Required(ErrorMessage = "El número de control es obligatorio.")]
    [StringLength(8, MinimumLength = 8, ErrorMessage = "El número de control debe tener 8 dígitos.")]
    public static int IdUsuario { get; set; }

    public static string? NombreUsuario { get; set; }

    public static string? ApellidoUsuario { get; set; }

    public static int? Idrol { get; set; }

    public static string? Iddepartamento { get; set; }

    public static string? NombreDepartamento { get; set; }
    public static string? NombrePfUsuario { get; set; }

    public static string? CorreoUsuario { get; set; }

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    public static string? ContrasenaUsuario { get; set; }

    public static string? mensajeError { get; set; }

    public static void vaciarMensaje() {
        mensajeError = "";
    }

    public static void vaciarUsuario() {
        IdUsuario = 0;
        NombreUsuario = "";
        ApellidoUsuario = "";
        Idrol = 0;
        Iddepartamento = "";
        NombreDepartamento = "";
        NombrePfUsuario = "";
        CorreoUsuario = "";
        ContrasenaUsuario = "";
        mensajeError = "";
    }

    public static void ActualizarEstadoEventos(EventosInstitucionalesContext context)
    {
        // Obtener la hora actual
        DateTime horaActual = DateTime.Now;

        // Actualizar eventos que hayan terminado
        var eventosTerminados = context.Eventos
            .Where(e => (e.IdEstadoEvento == 1 || e.IdEstadoEvento == 2) && horaActual > e.HoraFinEvento)
            .ToList();

        foreach (var eventoTerminado in eventosTerminados)
        {
            eventoTerminado.IdEstadoEvento = 4;
        }

        // Actualizar eventos en curso
        var eventosEnCurso = context.Eventos
            .Where(e => e.IdEstadoEvento == 1 && horaActual > e.HoraInicioEvento && horaActual < e.HoraFinEvento)
            .ToList();

        foreach (var eventoEnCurso in eventosEnCurso)
        {
            eventoEnCurso.IdEstadoEvento = 2;
        }

        // Guardar los cambios en la base de datos
        context.SaveChanges();
    }

}
