using System;
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

    public virtual DbSet<Docente> Docentes { get; set; }

    public virtual DbSet<DocenteHorario> DocenteHorarios { get; set; }

    public virtual DbSet<Especialidad> Especialidads { get; set; }

    public virtual DbSet<Estudiante> Estudiantes { get; set; }

    public virtual DbSet<Factura> Facturas { get; set; }

    public virtual DbSet<Grado> Grados { get; set; }

    public virtual DbSet<Horario> Horarios { get; set; }

    public virtual DbSet<HorarioCurso> HorarioCursos { get; set; }

    public virtual DbSet<Matricula> Matriculas { get; set; }

    public virtual DbSet<Monto> Montos { get; set; }

    public virtual DbSet<Nivel> Nivels { get; set; }

    public virtual DbSet<Notum> Nota { get; set; }

    public virtual DbSet<Pago> Pagos { get; set; }

    public virtual DbSet<PeriodoEscolar> PeriodoEscolars { get; set; }

    public virtual DbSet<Seccion> Seccions { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

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
            entity.Property(e => e.Edad).HasColumnName("edad");
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
            entity.Property(e => e.VacantLibres).HasColumnName("vacant_libres");
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
            entity.HasKey(e => e.IdCursoDocente).HasName("PK__CursoDoc__721C063101044463");

            entity.ToTable("CursoDocente");

            entity.Property(e => e.IdCursoDocente)
                .ValueGeneratedNever()
                .HasColumnName("id_cursoDocente");
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
        });

        modelBuilder.Entity<DocenteHorario>(entity =>
        {
            entity.HasKey(e => e.IdDocenteHorario).HasName("PK__DocenteH__555F4F5281D7C9B6");

            entity.ToTable("DocenteHorario");

            entity.Property(e => e.IdDocenteHorario).HasColumnName("id_docenteHorario");
            entity.Property(e => e.IdDocente).HasColumnName("id_docente");
            entity.Property(e => e.IdHorario).HasColumnName("id_horario");

            entity.HasOne(d => d.IdDocenteNavigation).WithMany(p => p.DocenteHorarios)
                .HasForeignKey(d => d.IdDocente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DocenteHorario_Docente");

            entity.HasOne(d => d.IdHorarioNavigation).WithMany(p => p.DocenteHorarios)
                .HasForeignKey(d => d.IdHorario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DocenteHorario_Horario");
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
            entity.Property(e => e.Email)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Estado)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("estado");
            entity.Property(e => e.FechNacimiento).HasColumnName("fech_nacimiento");
            entity.Property(e => e.Nombre)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Telefono)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("telefono");
        });

        modelBuilder.Entity<Factura>(entity =>
        {
            entity.HasKey(e => e.IdFactura);

            entity.ToTable("Factura");

            entity.Property(e => e.IdFactura).HasColumnName("id_factura");
            entity.Property(e => e.FechEmision).HasColumnName("fech_emision");
            entity.Property(e => e.IdApoderado).HasColumnName("id_apoderado");
            entity.Property(e => e.IdConcepto).HasColumnName("id_concepto");
            entity.Property(e => e.IdEstudiante).HasColumnName("id_estudiante");
            entity.Property(e => e.IdPago).HasColumnName("id_pago");
            entity.Property(e => e.MontoTotal)
                .HasColumnType("decimal(7, 2)")
                .HasColumnName("monto_total");

            entity.HasOne(d => d.IdApoderadoNavigation).WithMany(p => p.Facturas)
                .HasForeignKey(d => d.IdApoderado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Factura_Apoderado");

            entity.HasOne(d => d.IdConceptoNavigation).WithMany(p => p.Facturas)
                .HasForeignKey(d => d.IdConcepto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Factura_Concepto");

            entity.HasOne(d => d.IdEstudianteNavigation).WithMany(p => p.Facturas)
                .HasForeignKey(d => d.IdEstudiante)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Factura_Estudiante");

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
        });

        modelBuilder.Entity<Horario>(entity =>
        {
            entity.HasKey(e => e.IdHorario);

            entity.ToTable("Horario");

            entity.Property(e => e.IdHorario).HasColumnName("id_horario");
            entity.Property(e => e.HoraFin)
                .HasPrecision(0)
                .HasColumnName("hora_fin");
            entity.Property(e => e.HoraInicio)
                .HasPrecision(0)
                .HasColumnName("hora_inicio");
            entity.Property(e => e.IdAula).HasColumnName("id_aula");
            entity.Property(e => e.IdSeccion).HasColumnName("id_seccion");

            entity.HasOne(d => d.IdAulaNavigation).WithMany(p => p.Horarios)
                .HasForeignKey(d => d.IdAula)
                .HasConstraintName("FK_Horario_Aula");

            entity.HasOne(d => d.IdSeccionNavigation).WithMany(p => p.Horarios)
                .HasForeignKey(d => d.IdSeccion)
                .HasConstraintName("FK_Horario_Seccion");
        });

        modelBuilder.Entity<HorarioCurso>(entity =>
        {
            entity.HasKey(e => e.IdHorarioCurso).HasName("PK__HorarioC__506D9692F15C63F0");

            entity.ToTable("HorarioCurso");

            entity.Property(e => e.IdHorarioCurso).HasColumnName("id_horarioCurso");
            entity.Property(e => e.IdCurso).HasColumnName("id_curso");
            entity.Property(e => e.IdHorario).HasColumnName("id_horario");

            entity.HasOne(d => d.IdCursoNavigation).WithMany(p => p.HorarioCursos)
                .HasForeignKey(d => d.IdCurso)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HorarioCurso_Curso");

            entity.HasOne(d => d.IdHorarioNavigation).WithMany(p => p.HorarioCursos)
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
            entity.Property(e => e.IdAula).HasColumnName("id_aula");
            entity.Property(e => e.IdEstudiante).HasColumnName("id_estudiante");
            entity.Property(e => e.IdGrado).HasColumnName("id_grado");
            entity.Property(e => e.IdMonto).HasColumnName("id_monto");
            entity.Property(e => e.IdNivel).HasColumnName("id_nivel");
            entity.Property(e => e.IdPeriodEscolar).HasColumnName("id_periodEscolar");
            entity.Property(e => e.IdSeccion).HasColumnName("id_seccion");

            entity.HasOne(d => d.IdAulaNavigation).WithMany(p => p.Matriculas)
                .HasForeignKey(d => d.IdAula)
                .HasConstraintName("FK_Matricula_Aula");

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
            entity.Property(e => e.IdMatricula).HasColumnName("id_matricula");
            entity.Property(e => e.MontoPago)
                .HasColumnType("decimal(7, 2)")
                .HasColumnName("monto_pago");
            entity.Property(e => e.TipoPago)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("tipo_pago");

            entity.HasOne(d => d.IdMatriculaNavigation).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.IdMatricula)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pago_Matricula");
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

        modelBuilder.Entity<Seccion>(entity =>
        {
            entity.HasKey(e => e.IdSeccion);

            entity.ToTable("Seccion");

            entity.Property(e => e.IdSeccion).HasColumnName("id_seccion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario);

            entity.ToTable("Usuario");

            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Apellido)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("apellido");
            entity.Property(e => e.Contraseña)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("contraseña");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Nombre)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Rol)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("rol");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
