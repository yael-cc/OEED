using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace OEED_ITT.Models;

public partial class EventosInstitucionalesContext : DbContext
{
    public EventosInstitucionalesContext()
    {
    }

    public EventosInstitucionalesContext(DbContextOptions<EventosInstitucionalesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ActividadesEvento> ActividadesEventos { get; set; }

    public virtual DbSet<Asistencium> Asistencia { get; set; }

    public virtual DbSet<Comentario> Comentarios { get; set; }

    public virtual DbSet<Departamento> Departamentos { get; set; }

    public virtual DbSet<DepartamentoInvitado> DepartamentoInvitados { get; set; }

    public virtual DbSet<EstadoEvento> EstadoEventos { get; set; }

    public virtual DbSet<EstadoInscripcion> EstadoInscripcions { get; set; }

    public virtual DbSet<Evento> Eventos { get; set; }

    public virtual DbSet<GrupoOrganizador> GrupoOrganizadors { get; set; }

    public virtual DbSet<Inscripcion> Inscripcions { get; set; }

    public virtual DbSet<Lugar> Lugars { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<TipoEvento> TipoEventos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<VerObtenerEvento> VerObtenerEventos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    { 
        
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ActividadesEvento>(entity =>
        {
            entity.HasKey(e => e.IdActividad).HasName("PK__Activida__327F9BEDB84C7F2F");

            entity.ToTable("ActividadesEvento");

            entity.Property(e => e.IdActividad).HasColumnName("idActividad");
            entity.Property(e => e.DescripcionActividad)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("descripcionActividad");
            entity.Property(e => e.HoraFin).HasColumnName("horaFin");
            entity.Property(e => e.HoraInicio).HasColumnName("horaInicio");
            entity.Property(e => e.IdEvento).HasColumnName("idEvento");
            entity.Property(e => e.NombreActividad)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombreActividad");

            entity.HasOne(d => d.IdEventoNavigation).WithMany(p => p.ActividadesEventos)
                .HasForeignKey(d => d.IdEvento)
                .HasConstraintName("FK__Actividad__idEve__4D94879B");
        });

        modelBuilder.Entity<Asistencium>(entity =>
        {
            entity.HasKey(e => e.IdAsistencia).HasName("PK__Asistenc__4E1AB894EA4DAE30");

            entity.Property(e => e.IdAsistencia).HasColumnName("idAsistencia");
            entity.Property(e => e.DetallesAsistencia)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("detallesAsistencia");
            entity.Property(e => e.HoraAsistencia).HasColumnName("horaAsistencia");
            entity.Property(e => e.IdEvento).HasColumnName("idEvento");
            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

            entity.HasOne(d => d.IdEventoNavigation).WithMany(p => p.Asistencia)
                .HasForeignKey(d => d.IdEvento)
                .HasConstraintName("FK__Asistenci__idEve__5441852A");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Asistencia)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__Asistenci__idUsu__5535A963");
        });

        modelBuilder.Entity<Comentario>(entity =>
        {
            entity.HasKey(e => e.IdComentario).HasName("PK__Comentar__C74515DACDCB30B7");

            entity.Property(e => e.IdComentario).HasColumnName("idComentario");
            entity.Property(e => e.AsuntoComentario)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("asuntoComentario");
            entity.Property(e => e.DescripcionComentario)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("descripcionComentario");
            entity.Property(e => e.FechaComentario).HasColumnName("fechaComentario");
            entity.Property(e => e.IdEvento).HasColumnName("idEvento");
            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

            entity.HasOne(d => d.IdEventoNavigation).WithMany(p => p.Comentarios)
                .HasForeignKey(d => d.IdEvento)
                .HasConstraintName("FK__Comentari__idEve__5165187F");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Comentarios)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__Comentari__idUsu__5070F446");
        });

        modelBuilder.Entity<Departamento>(entity =>
        {
            entity.HasKey(e => e.IdDepartamento).HasName("PK__Departam__C225F98D166FA750");

            entity.ToTable("Departamento");

            entity.Property(e => e.IdDepartamento)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("idDepartamento");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .HasColumnName("descripcion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<DepartamentoInvitado>(entity =>
        {
            entity.HasKey(e => e.IdDepartamentoInvitado).HasName("PK__Departam__B76FE24DF92CF3BC");
            
            entity.ToTable("DepartamentoInvitado");

            entity.Property(e => e.IdDepartamento)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("idDepartamento");
            entity.Property(e => e.IdEvento).HasColumnName("idEvento");

            entity.HasOne(d => d.IdDepartamentoNavigation).WithMany()
                .HasForeignKey(d => d.IdDepartamento)
                .HasConstraintName("FK__Departame__idDep__6FE99F9F");

            entity.HasOne(d => d.IdEventoNavigation).WithMany()
                .HasForeignKey(d => d.IdEvento)
                .HasConstraintName("FK__Departame__idEve__6EF57B66");
        });

        modelBuilder.Entity<EstadoEvento>(entity =>
        {
            entity.HasKey(e => e.IdEstadoEvento).HasName("PK__EstadoEv__895EDC513AB9CAB1");

            entity.ToTable("EstadoEvento");

            entity.Property(e => e.IdEstadoEvento).HasColumnName("idEstadoEvento");
            entity.Property(e => e.DescripcionEstadoEvento)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("descripcionEstadoEvento");
            entity.Property(e => e.NombreEstadoEvento)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("nombreEstadoEvento");
        });

        modelBuilder.Entity<EstadoInscripcion>(entity =>
        {
            entity.HasKey(e => e.IdEstadoInscripcion).HasName("PK__EstadoIn__5CD68FF4DFAA2FAB");

            entity.ToTable("EstadoInscripcion");

            entity.Property(e => e.IdEstadoInscripcion).HasColumnName("idEstadoInscripcion");
            entity.Property(e => e.DescripcionEstadoInscripcion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("descripcionEstadoInscripcion");
            entity.Property(e => e.NombreEstadoInscripcion)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("nombreEstadoInscripcion");
        });

        modelBuilder.Entity<Evento>(entity =>
        {
            entity.HasKey(e => e.IdEvento).HasName("PK__Evento__C8DC7BDAAE1B2C55");

            entity.ToTable("Evento");

            entity.Property(e => e.IdEvento).HasColumnName("idEvento");
            entity.Property(e => e.DescripcionEvento)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("descripcionEvento");
            entity.Property(e => e.FechaEvento).HasColumnName("fechaEvento");
            entity.Property(e => e.HoraFinEvento).HasColumnName("horaFinEvento");
            entity.Property(e => e.HoraInicioEvento).HasColumnName("horaInicioEvento");
            entity.Property(e => e.IdEstadoEvento).HasColumnName("idEstadoEvento");
            entity.Property(e => e.IdTipoEvento).HasColumnName("idTipoEvento");
            entity.Property(e => e.IddepartamentoOrigen)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("iddepartamentoOrigen");
            entity.Property(e => e.IdlugarEvento).HasColumnName("idlugarEvento");
            entity.Property(e => e.Imagen)
                .IsUnicode(false)
                .HasColumnName("imagen");
            entity.Property(e => e.NombreEvento)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombreEvento");
            entity.Property(e => e.NumMaxEvento).HasColumnName("numMaxEvento");

            entity.HasOne(d => d.oEstado).WithMany(p => p.Eventos)
                .HasForeignKey(d => d.IdEstadoEvento)
                .HasConstraintName("FK__Evento__idEstado__47DBAE45");

            entity.HasOne(d => d.oTipoEvento).WithMany(p => p.Eventos)
                .HasForeignKey(d => d.IdTipoEvento)
                .HasConstraintName("FK__Evento__idTipoEv__45F365D3");

            entity.HasOne(d => d.oDepartamento).WithMany(p => p.Eventos)
                .HasForeignKey(d => d.IddepartamentoOrigen)
                .HasConstraintName("FK__Evento__iddepart__46E78A0C");

            entity.HasOne(d => d.oLugar).WithMany(p => p.Eventos)
                .HasForeignKey(d => d.IdlugarEvento)
                .HasConstraintName("FK__Evento__idlugarE__44FF419A");
        });

        modelBuilder.Entity<GrupoOrganizador>(entity =>
        {
            entity.HasKey(e => e.IdGrupoOrganizador).HasName("PK__GrupoOrg__3D79751AA8668203");

            entity.ToTable("GrupoOrganizador");

            entity.Property(e => e.IdEvento).HasColumnName("idEvento");
            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

            entity.HasOne(d => d.IdEventoNavigation).WithMany()
                .HasForeignKey(d => d.IdEvento)
                .HasConstraintName("FK__GrupoOrga__idEve__49C3F6B7");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany()
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__GrupoOrga__idUsu__4AB81AF0");
        });

        modelBuilder.Entity<Inscripcion>(entity =>
        {
            entity.HasKey(e => e.IdInscripcion).HasName("PK__Inscripc__3D58AB69A6328B35");

            entity.ToTable("Inscripcion");

            entity.Property(e => e.IdInscripcion).HasColumnName("idInscripcion");
            entity.Property(e => e.HoraInscripcion).HasColumnName("horaInscripcion");
            entity.Property(e => e.IdEstadoInscripcion).HasColumnName("idEstadoInscripcion");
            entity.Property(e => e.IdEvento).HasColumnName("idEvento");
            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

            entity.HasOne(d => d.oEstadoI).WithMany(p => p.Inscripcions)
                .HasForeignKey(d => d.IdEstadoInscripcion)
                .HasConstraintName("FK__Inscripci__idEst__5BE2A6F2");

            entity.HasOne(d => d.OEvento).WithMany(p => p.Inscripcions)
                .HasForeignKey(d => d.IdEvento)
                .HasConstraintName("FK__Inscripci__idEve__5AEE82B9");

            entity.HasOne(d => d.oUsuario).WithMany(p => p.Inscripcions)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__Inscripci__idUsu__59FA5E80");
        });

        modelBuilder.Entity<Lugar>(entity =>
        {
            entity.HasKey(e => e.IdLugar).HasName("PK__Lugar__F7460D5F8AFB9A6F");

            entity.ToTable("Lugar");

            entity.Property(e => e.IdLugar).HasColumnName("idLugar");
            entity.Property(e => e.CapacidadLugar).HasColumnName("capacidadLugar");
            entity.Property(e => e.DescripcionLugar)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("descripcionLugar");
            entity.Property(e => e.NombreLugar)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombreLugar");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__Rol__3C872F7632A0B6EA");

            entity.ToTable("Rol");

            entity.Property(e => e.IdRol).HasColumnName("idRol");
            entity.Property(e => e.DescripcionRol)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("descripcionRol");
            entity.Property(e => e.NombreRol)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombreRol");
        });

        modelBuilder.Entity<TipoEvento>(entity =>
        {
            entity.HasKey(e => e.IdTipoEvento).HasName("PK__TipoEven__09EED93A1B2B9720");

            entity.ToTable("TipoEvento");

            entity.Property(e => e.IdTipoEvento).HasColumnName("idTipoEvento");
            entity.Property(e => e.DescripcionTipoEvento)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("descripcionTipoEvento");
            entity.Property(e => e.NombreTipoEvento)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("nombreTipoEvento");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuario__645723A63F97396C");

            entity.ToTable("Usuario");

            entity.Property(e => e.IdUsuario)
                .ValueGeneratedNever()
                .HasColumnName("idUsuario");
            entity.Property(e => e.ApellidoUsuario)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("apellidoUsuario");
            entity.Property(e => e.ContrasenaUsuario)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("contrasenaUsuario");
            entity.Property(e => e.CorreoUsuario)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("correoUsuario");
            entity.Property(e => e.Iddepartamento)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("iddepartamento");
            entity.Property(e => e.Idrol).HasColumnName("idrol");
            entity.Property(e => e.NombrePfUsuario)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombrePfUsuario");
            entity.Property(e => e.NombreUsuario)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombreUsuario");

            entity.HasOne(d => d.IddepartamentoNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.Iddepartamento)
                .HasConstraintName("FK__Usuario__iddepar__3C69FB99");

            entity.HasOne(d => d.IdrolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.Idrol)
                .HasConstraintName("FK__Usuario__idrol__3B75D760");
        });

        modelBuilder.Entity<VerObtenerEvento>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("ver_Obtener_Evento");

            entity.Property(e => e.DepartamentoDeOrigen)
                .HasMaxLength(100)
                .HasColumnName("Departamento de origen");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Estado)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Imagen)
                .IsUnicode(false)
                .HasColumnName("imagen");
            entity.Property(e => e.Lugar)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Tipo)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
