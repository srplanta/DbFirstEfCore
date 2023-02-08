using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DbFirstEfCore.Models;

public partial class StudentDbContext : DbContext
{
    public StudentDbContext()
    {
    }

    public StudentDbContext(DbContextOptions<StudentDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<FeeBill> FeeBills { get; set; }
    public virtual DbSet<Student> Students { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("server=DESKTOP-MAE99H0; database=StudentDb; trusted_connection=true; TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FeeBill>(entity =>
        {
            entity.ToTable("FeeBill");

            entity.Property(e => e.AdmissionFee).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.BillingMonth).HasColumnType("date");
            entity.Property(e => e.FeePaid).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.FeePayable).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Fine).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.NextArrears).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.PreviousArrears).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.StationaryCharges).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.StudentId).HasColumnName("StudentID");
            entity.Property(e => e.TutionFee).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.Student).WithMany(p => p.FeeBills)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FeeBill_Student");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_tbl_Student");

            entity.ToTable("Student");

            entity.Property(e => e.Gender).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
