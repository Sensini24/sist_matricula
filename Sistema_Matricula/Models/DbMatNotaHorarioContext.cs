﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Sistema_Matricula.Models;

public partial class DbMatNotaHorarioContext : DbContext
{
    public DbMatNotaHorarioContext()
    {
    }

    public DbMatNotaHorarioContext(DbContextOptions<DbMatNotaHorarioContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Apoderado> Apoderados { get; set; }

    public virtual DbSet<ApoderadoAlumno> ApoderadoAlumnos { get; set; }

    public virtual DbSet<Aula> Aulas { get; set; }

    public virtual DbSet<Bimestre> Bimestres { get; set; }

    public virtual DbSet<Concepto> Conceptos { get; set; }

    public virtual DbSet<Curso> Cursos { get; set; }

    public virtual DbSet<CursoDocente> CursoDocentes { get; set; }

    public virtual DbSet<CursoSeccion> CursoSeccions { get; set; }

    public virtual DbSet<Docente> Docentes { get; set; }

    public virtual DbSet<Especialidad> Especialidads { get; set; }

    public virtual DbSet<Estudiante> Estudiantes { get; set; }

    public virtual DbSet<Factura> Facturas { get; set; }

    public virtual DbSet<Grado> Grados { get; set; }

    public virtual DbSet<Horario> Horarios { get; set; }

    public virtual DbSet<HorarioCursoSeccion> HorarioCursoSeccions { get; set; }

    public virtual DbSet<Matricula> Matriculas { get; set; }

    public virtual DbSet<Monto> Montos { get; set; }

    public virtual DbSet<Nivel> Nivels { get; set; }

    public virtual DbSet<Notum> Nota { get; set; }

    public virtual DbSet<Pago> Pagos { get; set; }

    public virtual DbSet<PeriodoEscolar> PeriodoEscolars { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<Seccion> Seccions { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<UsuarioRol> UsuarioRols { get; set; }

    public virtual DbSet<VistaCursoAlumno> VistaCursoAlumnos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server=DESKTOP-MNHLOO0;database=db_mat_nota_horario;integrated security=true; TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Apoderado>(entity =>
        {
            entity.HasKey(e => e.IdApoderado);

            entity.ToTable("Apoderado");

            entity.Property(e => e.IdApoderado).HasColumnName("id_apoderado");
            entity.Property(e => e.Apellido)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("apellido");
            entity.Property(e => e.Direccion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("direccion");
            entity.Property(e => e.FechNacimiento).HasColumnName("fech_nacimiento");
            entity.Property(e => e.Nombre)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Ocupacion)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("ocupacion");
            entity.Property(e => e.Sexo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("sexo");
            entity.Property(e => e.Telefono)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("telefono");
        });

        modelBuilder.Entity<ApoderadoAlumno>(entity =>
        {
            entity.HasKey(e => e.IdApoderadoAlumno);

            entity.ToTable("ApoderadoAlumno");

            entity.Property(e => e.IdApoderadoAlumno).HasColumnName("id_apoderadoAlumno");
            entity.Property(e => e.IdApoderado).HasColumnName("id_apoderado");
            entity.Property(e => e.IdEstudiante).HasColumnName("id_estudiante");
            entity.Property(e => e.Parentesco)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("parentesco");

            entity.HasOne(d => d.IdApoderadoNavigation).WithMany(p => p.ApoderadoAlumnos)
                .HasForeignKey(d => d.IdApoderado)
                .HasConstraintName("FK_ApoderadoAlumno_Apoderado");

            entity.HasOne(d => d.IdEstudianteNavigation).WithMany(p => p.ApoderadoAlumnos)
                .HasForeignKey(d => d.IdEstudiante)
                .HasConstraintName("FK_ApoderadoAlumno_Estudiante");
        });

        modelBuilder.Entity<Aula>(entity =>
        {
            entity.HasKey(e => e.IdAula);

            entity.ToTable("Aula");

            entity.Property(e => e.IdAula).HasColumnName("id_aula");
            entity.Property(e => e.Capacidad).HasColumnName("capacidad");
            entity.Property(e => e.IdSeccion).HasColumnName("id_seccion");
            entity.Property(e => e.VacantLibres).HasColumnName("vacant_libres");

            entity.HasOne(d => d.IdSeccionNavigation).WithMany(p => p.Aulas)
                .HasForeignKey(d => d.IdSeccion)
                .HasConstraintName("FK_Aula_Seccion");
        });

        modelBuilder.Entity<Bimestre>(entity =>
        {
            entity.HasKey(e => e.IdBimestre);

            entity.ToTable("Bimestre");

            entity.Property(e => e.IdBimestre).HasColumnName("id_bimestre");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("descripcion");
        });

        modelBuilder.Entity<Concepto>(entity =>
        {
            entity.HasKey(e => e.IdConcepto);

            entity.ToTable("Concepto");

            entity.Property(e => e.IdConcepto).HasColumnName("id_concepto");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("descripcion");
        });

        modelBuilder.Entity<Curso>(entity =>
        {
            entity.HasKey(e => e.IdCurso);

            entity.ToTable("Curso");

            entity.Property(e => e.IdCurso).HasColumnName("id_curso");
            entity.Property(e => e.Nombre)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<CursoDocente>(entity =>
        {
            entity.HasKey(e => e.IdCursoDocente).HasName("PK__CursoDoc__721C0631FEBB75D6");

            entity.ToTable("CursoDocente");

            entity.Property(e => e.IdCursoDocente).HasColumnName("id_cursoDocente");
            entity.Property(e => e.IdCurso).HasColumnName("id_curso");
            entity.Property(e => e.IdDocente).HasColumnName("id_docente");

            entity.HasOne(d => d.IdCursoNavigation).WithMany(p => p.CursoDocentes)
                .HasForeignKey(d => d.IdCurso)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CursoDocente_Curso");

            entity.HasOne(d => d.IdDocenteNavigation).WithMany(p => p.CursoDocentes)
                .HasForeignKey(d => d.IdDocente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CursoDocente_Docente");
        });

        modelBuilder.Entity<CursoSeccion>(entity =>
        {
            entity.HasKey(e => e.IdCursoSeccion);

            entity.ToTable("CursoSeccion");

            entity.Property(e => e.IdCursoSeccion).HasColumnName("id_cursoSeccion");
            entity.Property(e => e.IdCurso).HasColumnName("id_curso");
            entity.Property(e => e.IdDocente).HasColumnName("id_docente");
            entity.Property(e => e.IdSeccion).HasColumnName("id_seccion");

            entity.HasOne(d => d.IdCursoNavigation).WithMany(p => p.CursoSeccions)
                .HasForeignKey(d => d.IdCurso)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CursoSeccion_Curso");

            entity.HasOne(d => d.IdDocenteNavigation).WithMany(p => p.CursoSeccions)
                .HasForeignKey(d => d.IdDocente)
                .HasConstraintName("FK_CursoSeccion_Docente");

            entity.HasOne(d => d.IdSeccionNavigation).WithMany(p => p.CursoSeccions)
                .HasForeignKey(d => d.IdSeccion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CursoSeccion_Seccion");
        });

        modelBuilder.Entity<Docente>(entity =>
        {
            entity.HasKey(e => e.IdDocente);

            entity.ToTable("Docente");

            entity.Property(e => e.IdDocente).HasColumnName("id_docente");
            entity.Property(e => e.Apellido)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("apellido");
            entity.Property(e => e.Edad).HasColumnName("edad");
            entity.Property(e => e.Estado)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("estado");
            entity.Property(e => e.FechNacimiento).HasColumnName("fech_nacimiento");
            entity.Property(e => e.IdEspecialidad).HasColumnName("id_especialidad");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Nombre)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Sexo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("sexo");
            entity.Property(e => e.Telefono)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("telefono");

            entity.HasOne(d => d.IdEspecialidadNavigation).WithMany(p => p.Docentes)
                .HasForeignKey(d => d.IdEspecialidad)
                .HasConstraintName("FK_Docente_Especialidad");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Docentes)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK_Docente_Usuario");
        });

        modelBuilder.Entity<Especialidad>(entity =>
        {
            entity.HasKey(e => e.IdEspecialidad);

            entity.ToTable("Especialidad");

            entity.Property(e => e.IdEspecialidad).HasColumnName("id_especialidad");
            entity.Property(e => e.Especialidad1)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("especialidad");
        });

        modelBuilder.Entity<Estudiante>(entity =>
        {
            entity.HasKey(e => e.IdEstudiante).HasName("PK_Estduia");

            entity.ToTable("Estudiante");

            entity.Property(e => e.IdEstudiante).HasColumnName("id_estudiante");
            entity.Property(e => e.Apellido)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("apellido");
            entity.Property(e => e.Direccion)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("direccion");
            entity.Property(e => e.Dni)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("dni");
            entity.Property(e => e.Estado)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("estado");
            entity.Property(e => e.FechNacimiento).HasColumnName("fech_nacimiento");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Nombre)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Sexo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("sexo");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Estudiantes)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK_Estudiante_Usuario");
        });

        modelBuilder.Entity<Factura>(entity =>
        {
            entity.HasKey(e => e.IdFactura);

            entity.ToTable("Factura");

            entity.Property(e => e.IdFactura).HasColumnName("id_factura");
            entity.Property(e => e.FechEmision).HasColumnName("fech_emision");
            entity.Property(e => e.IdPago).HasColumnName("id_pago");
            entity.Property(e => e.MontoTotal)
                .HasColumnType("decimal(7, 2)")
                .HasColumnName("monto_total");

            entity.HasOne(d => d.IdPagoNavigation).WithMany(p => p.Facturas)
                .HasForeignKey(d => d.IdPago)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Factura_Pago");
        });

        modelBuilder.Entity<Grado>(entity =>
        {
            entity.HasKey(e => e.IdGrado).HasName("PK__Grado__6DB797EE0E394425");

            entity.ToTable("Grado");

            entity.Property(e => e.IdGrado).HasColumnName("id_grado");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.IdNivel).HasColumnName("id_nivel");

            entity.HasOne(d => d.IdNivelNavigation).WithMany(p => p.Grados)
                .HasForeignKey(d => d.IdNivel)
                .HasConstraintName("FK_Grado_Nivel");
        });

        modelBuilder.Entity<Horario>(entity =>
        {
            entity.HasKey(e => e.IdHorario);

            entity.ToTable("Horario");

            entity.Property(e => e.IdHorario).HasColumnName("id_horario");
            entity.Property(e => e.DiaSemana)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("dia_semana");
            entity.Property(e => e.HoraFin)
                .HasPrecision(0)
                .HasColumnName("hora_fin");
            entity.Property(e => e.HoraInicio)
                .HasPrecision(0)
                .HasColumnName("hora_inicio");
        });

        modelBuilder.Entity<HorarioCursoSeccion>(entity =>
        {
            entity.HasKey(e => e.IdHorarioCursoSeccion).HasName("PK__HorarioC__506D9692F15C63F0");

            entity.ToTable("HorarioCursoSeccion");

            entity.Property(e => e.IdHorarioCursoSeccion).HasColumnName("id_horarioCursoSeccion");
            entity.Property(e => e.IdCursoSeccion).HasColumnName("id_cursoSeccion");
            entity.Property(e => e.IdHorario).HasColumnName("id_horario");

            entity.HasOne(d => d.IdCursoSeccionNavigation).WithMany(p => p.HorarioCursoSeccions)
                .HasForeignKey(d => d.IdCursoSeccion)
                .HasConstraintName("FK_HorarioCursoSeccion_CursoSeccion");

            entity.HasOne(d => d.IdHorarioNavigation).WithMany(p => p.HorarioCursoSeccions)
                .HasForeignKey(d => d.IdHorario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HorarioCurso_Horario");
        });

        modelBuilder.Entity<Matricula>(entity =>
        {
            entity.HasKey(e => e.IdMatricula);

            entity.ToTable("Matricula");

            entity.Property(e => e.IdMatricula).HasColumnName("id_matricula");
            entity.Property(e => e.Estado)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("estado");
            entity.Property(e => e.FechMatricula).HasColumnName("fech_matricula");
            entity.Property(e => e.IdEstudiante).HasColumnName("id_estudiante");
            entity.Property(e => e.IdGrado).HasColumnName("id_grado");
            entity.Property(e => e.IdMonto).HasColumnName("id_monto");
            entity.Property(e => e.IdNivel).HasColumnName("id_nivel");
            entity.Property(e => e.IdPeriodEscolar).HasColumnName("id_periodEscolar");
            entity.Property(e => e.IdSeccion).HasColumnName("id_seccion");

            entity.HasOne(d => d.IdEstudianteNavigation).WithMany(p => p.Matriculas)
                .HasForeignKey(d => d.IdEstudiante)
                .HasConstraintName("FK_Matricula_Estudiante");

            entity.HasOne(d => d.IdGradoNavigation).WithMany(p => p.Matriculas)
                .HasForeignKey(d => d.IdGrado)
                .HasConstraintName("FK_Matricula_Grado");

            entity.HasOne(d => d.IdMontoNavigation).WithMany(p => p.Matriculas)
                .HasForeignKey(d => d.IdMonto)
                .HasConstraintName("FK_Matricula_Monto");

            entity.HasOne(d => d.IdNivelNavigation).WithMany(p => p.Matriculas)
                .HasForeignKey(d => d.IdNivel)
                .HasConstraintName("FK_Matricula_Nivel");

            entity.HasOne(d => d.IdPeriodEscolarNavigation).WithMany(p => p.Matriculas)
                .HasForeignKey(d => d.IdPeriodEscolar)
                .HasConstraintName("FK_Matricula_PeriodoEscolar");

            entity.HasOne(d => d.IdSeccionNavigation).WithMany(p => p.Matriculas)
                .HasForeignKey(d => d.IdSeccion)
                .HasConstraintName("FK_Matricula_Seccion");
        });

        modelBuilder.Entity<Monto>(entity =>
        {
            entity.HasKey(e => e.IdMonto);

            entity.ToTable("Monto");

            entity.Property(e => e.IdMonto).HasColumnName("id_monto");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.Monto1)
                .HasColumnType("decimal(7, 2)")
                .HasColumnName("monto");
        });

        modelBuilder.Entity<Nivel>(entity =>
        {
            entity.HasKey(e => e.IdNivel);

            entity.ToTable("Nivel");

            entity.Property(e => e.IdNivel).HasColumnName("id_nivel");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("descripcion");
        });

        modelBuilder.Entity<Notum>(entity =>
        {
            entity.HasKey(e => e.IdNota);

            entity.Property(e => e.IdNota).HasColumnName("id_nota");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.IdBimestre).HasColumnName("id_bimestre");
            entity.Property(e => e.IdCurso).HasColumnName("id_curso");
            entity.Property(e => e.IdDocente).HasColumnName("id_docente");
            entity.Property(e => e.IdEstudiante).HasColumnName("id_estudiante");
            entity.Property(e => e.Nota)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("nota");

            entity.HasOne(d => d.IdBimestreNavigation).WithMany(p => p.Nota)
                .HasForeignKey(d => d.IdBimestre)
                .HasConstraintName("FK_Nota_Bimestre");

            entity.HasOne(d => d.IdCursoNavigation).WithMany(p => p.Nota)
                .HasForeignKey(d => d.IdCurso)
                .HasConstraintName("FK_Nota_Curso");

            entity.HasOne(d => d.IdDocenteNavigation).WithMany(p => p.Nota)
                .HasForeignKey(d => d.IdDocente)
                .HasConstraintName("FK_Nota_Docente");

            entity.HasOne(d => d.IdEstudianteNavigation).WithMany(p => p.Nota)
                .HasForeignKey(d => d.IdEstudiante)
                .HasConstraintName("FK_Nota_Estudiante");
        });

        modelBuilder.Entity<Pago>(entity =>
        {
            entity.HasKey(e => e.IdPago);

            entity.ToTable("Pago");

            entity.Property(e => e.IdPago).HasColumnName("id_pago");
            entity.Property(e => e.Estado)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("estado");
            entity.Property(e => e.FechPago).HasColumnName("fech_pago");
            entity.Property(e => e.IdConcepto).HasColumnName("id_concepto");
            entity.Property(e => e.IdEstudiante).HasColumnName("id_estudiante");
            entity.Property(e => e.MontoPago)
                .HasColumnType("decimal(7, 2)")
                .HasColumnName("monto_pago");
            entity.Property(e => e.TipoPago)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("tipo_pago");

            entity.HasOne(d => d.IdConceptoNavigation).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.IdConcepto)
                .HasConstraintName("FK_Pago_Concepto");

            entity.HasOne(d => d.IdEstudianteNavigation).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.IdEstudiante)
                .HasConstraintName("FK_Pago_Estudiante");
        });

        modelBuilder.Entity<PeriodoEscolar>(entity =>
        {
            entity.HasKey(e => e.IdPeriodEscolar);

            entity.ToTable("PeriodoEscolar");

            entity.Property(e => e.IdPeriodEscolar).HasColumnName("id_periodEscolar");
            entity.Property(e => e.Estado)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("estado");
            entity.Property(e => e.FechFinal).HasColumnName("fech_final");
            entity.Property(e => e.FechInicio).HasColumnName("fech_inicio");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__Rol__6ABCB5E001964946");

            entity.ToTable("Rol");

            entity.HasIndex(e => e.Nombre, "UQ__Rol__72AFBCC6F29A7C0A").IsUnique();

            entity.Property(e => e.IdRol).HasColumnName("id_rol");
            entity.Property(e => e.Nombre)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Seccion>(entity =>
        {
            entity.HasKey(e => e.IdSeccion);

            entity.ToTable("Seccion");

            entity.Property(e => e.IdSeccion).HasColumnName("id_seccion");
            entity.Property(e => e.IdGrado).HasColumnName("id_grado");
            entity.Property(e => e.Nombre)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("nombre");

            entity.HasOne(d => d.IdGradoNavigation).WithMany(p => p.Seccions)
                .HasForeignKey(d => d.IdGrado)
                .HasConstraintName("FK_Seccion_Grado");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuario__4E3E04ADFDA08180");

            entity.ToTable("Usuario");

            entity.HasIndex(e => e.Email, "UQ__Usuario__AB6E616402ED5D2B").IsUnique();

            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("passwordHash");
        });

        modelBuilder.Entity<UsuarioRol>(entity =>
        {
            entity.HasKey(e => e.IdUsuarioRol).HasName("PK__UsuarioR__F1D5AB7BBFF5DDF7");

            entity.ToTable("UsuarioRol");

            entity.Property(e => e.IdUsuarioRol).HasColumnName("id_usuarioRol");
            entity.Property(e => e.IdRol).HasColumnName("id_rol");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.UsuarioRols)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UsuarioRo__id_ro__55F4C372");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.UsuarioRols)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UsuarioRo__id_us__55009F39");
        });

        modelBuilder.Entity<VistaCursoAlumno>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VistaCursoAlumno");

            entity.Property(e => e.Bimestre)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.Descripcion)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.IdEstudiante).HasColumnName("id_estudiante");
            entity.Property(e => e.Nombre)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Nota).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Profesor)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
