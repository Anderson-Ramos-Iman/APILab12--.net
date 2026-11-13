using APILab12.Models;
using Microsoft.EntityFrameworkCore;

namespace APILab12.Data;

public class AcademiaDbContext(DbContextOptions<AcademiaDbContext> options) : DbContext(options)
{
    public DbSet<Instructor> Instructores => Set<Instructor>();
    public DbSet<Estudiante> Estudiantes => Set<Estudiante>();
    public DbSet<Curso> Cursos => Set<Curso>();
    public DbSet<Matricula> Matriculas => Set<Matricula>();
    public DbSet<Pago> Pagos => Set<Pago>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Instructor>()
            .HasIndex(x => x.Email)
            .IsUnique();

        modelBuilder.Entity<Estudiante>()
            .HasIndex(x => x.Email)
            .IsUnique();

        modelBuilder.Entity<Curso>()
            .Property(x => x.Precio)
            .HasPrecision(10, 2);

        modelBuilder.Entity<Matricula>()
            .Property(x => x.MontoTotal)
            .HasPrecision(10, 2);

        modelBuilder.Entity<Pago>()
            .Property(x => x.Monto)
            .HasPrecision(10, 2);

        modelBuilder.Entity<Curso>()
            .HasOne(x => x.Instructor)
            .WithMany(x => x.Cursos)
            .HasForeignKey(x => x.IdInstructor)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Matricula>()
            .HasOne(x => x.Estudiante)
            .WithMany(x => x.Matriculas)
            .HasForeignKey(x => x.IdEstudiante)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Matricula>()
            .HasOne(x => x.Curso)
            .WithMany(x => x.Matriculas)
            .HasForeignKey(x => x.IdCurso)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Pago>()
            .HasOne(x => x.Matricula)
            .WithMany(x => x.Pagos)
            .HasForeignKey(x => x.IdMatricula)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
