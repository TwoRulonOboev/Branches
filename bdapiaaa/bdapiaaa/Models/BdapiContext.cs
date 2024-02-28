using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace bdapiaaa.Models;

public partial class BdapiContext : DbContext
{
    public BdapiContext()
    {
    }

    public BdapiContext(DbContextOptions<BdapiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Abonent> Abonents { get; set; }

    public virtual DbSet<PaymentRegistration> PaymentRegistrations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Data Source=(localDB)\\MSSQLLocalDB;Initial Catalog=bdapi;Integrated Security=True;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Abonent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Abonent__3214EC27F667EA23");

            entity.ToTable("Abonent");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.MonthlyPayment).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PhoneNumber).HasMaxLength(15);
        });

        modelBuilder.Entity<PaymentRegistration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PaymentR__3214EC27D7C035AC");

            entity.ToTable("PaymentRegistration");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.DebtOrOverpayment).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.MonthlyPayment).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PaymentAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PaymentDate).HasColumnType("date");
            entity.Property(e => e.PhoneNumber).HasMaxLength(15);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
