using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SaepSimulado.Models;

public partial class ClinicadbContext : DbContext
{
    public ClinicadbContext()
    {
    }

    public ClinicadbContext(DbContextOptions<ClinicadbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Consulta> Consulta { get; set; }

    public virtual DbSet<Medico> Medicos { get; set; }

    public virtual DbSet<Paciente> Pacientes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=Clinicadb;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Consulta>(entity =>
        {
            entity.HasKey(e => e.Codigo);

            entity.Property(e => e.DataHora).HasColumnType("datetime");
            entity.Property(e => e.MedicoId).HasColumnName("MedicoID");
            entity.Property(e => e.PacienteId).HasColumnName("PacienteID");
            entity.Property(e => e.StatusConsulta)
                .HasMaxLength(50)
                .IsFixedLength();

            entity.HasOne(d => d.Medico).WithMany(p => p.Consulta)
                .HasForeignKey(d => d.MedicoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Consulta_Medico");

            entity.HasOne(d => d.Paciente).WithMany(p => p.Consulta)
                .HasForeignKey(d => d.PacienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Consulta_Paciente");
        });

        modelBuilder.Entity<Medico>(entity =>
        {
            entity.HasKey(e => e.Codigo);

            entity.ToTable("Medico");

            entity.Property(e => e.Crm)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("CRM");
            entity.Property(e => e.Especialidade)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Paciente>(entity =>
        {
            entity.HasKey(e => e.Codigo);

            entity.ToTable("Paciente");

            entity.Property(e => e.Cpf)
                .HasMaxLength(14)
                .IsFixedLength()
                .HasColumnName("cpf");
            entity.Property(e => e.DataNascimento).HasColumnType("date");
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nome");
            entity.Property(e => e.Telefone)
                .HasMaxLength(20)
                .IsFixedLength()
                .HasColumnName("telefone");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
