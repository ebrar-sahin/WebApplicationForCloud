using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models;

public partial class WarehouseDbContext : DbContext
{
    public WarehouseDbContext()
    {
    }

    public WarehouseDbContext(DbContextOptions<WarehouseDbContext> options)
        : base(options)
    {
    }

    // Tüm Tablolarımız
    public virtual DbSet<ActivityLog> ActivityLogs { get; set; }
    public virtual DbSet<Customer> Customers { get; set; }
    public virtual DbSet<Event> Events { get; set; }
    public virtual DbSet<Invoice> Invoices { get; set; }
    public virtual DbSet<Material> Materials { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<Subcontractor> Subcontractors { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultConnection");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // --- Müşteri (Customer) Ayarları ---
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK_Customer"); // İsimler sadeleştirildi
            entity.ToTable("Customer");

            entity.Property(e => e.CustomerId).HasColumnName("Customer_ID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.PostalCode).HasMaxLength(20).HasColumnName("Postal_Code");
            entity.Property(e => e.Surname).HasMaxLength(50);
        });

        // --- Etkinlik (Event) Ayarları ---
        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.EventId).HasName("PK_Event");
            entity.ToTable("Event");

            entity.Property(e => e.EventId).HasColumnName("Event_ID");
            entity.Property(e => e.AddressId).HasColumnName("Address_ID");
            entity.Property(e => e.CustomerId).HasColumnName("Customer_ID");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Location).HasMaxLength(100);

            entity.HasOne(d => d.Customer).WithMany(p => p.Events)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_Event_Customer");
        });

        // --- Fatura (Invoice) Ayarları ---
        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.InvoiceId).HasName("PK_Invoice");
            entity.ToTable("Invoice");

            entity.Property(e => e.InvoiceId).HasColumnName("Invoice_ID");
            entity.Property(e => e.CustomerId).HasColumnName("Customer_ID");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.DueDate).HasColumnType("datetime").HasColumnName("Due_Date");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Tax).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Total).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Customer).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_Invoice_Customer");
        });

        // --- Malzeme (Material) Ayarları ---
        modelBuilder.Entity<Material>(entity =>
        {
            entity.HasKey(e => e.MaterialId).HasName("PK_Material");
            entity.ToTable("Material");

            entity.Property(e => e.MaterialId).HasColumnName("Material_ID");
            entity.Property(e => e.MaterialType).HasMaxLength(50).HasColumnName("Material_Type");
            entity.Property(e => e.SubcontractorId).HasColumnName("Subcontractor_ID");

            entity.HasOne(d => d.Subcontractor).WithMany(p => p.Materials)
                .HasForeignKey(d => d.SubcontractorId)
                .HasConstraintName("FK_Material_Subcontractor");
        });

        // --- SİPARİŞ (ORDER) AYARLARI [DÜZELTİLEN KISIM] ---
        modelBuilder.Entity<Order>(entity =>
        {
            // Artık CustomerId, ProductLocation YOK. Sadece var olanlar:
            entity.HasKey(e => e.OrderId);

            // Sütun isimlerini C# modeline göre otomatik eşleştirir.
            // İstersen özel isim verebiliriz ama gerek yok.

            // İlişkiyi kuralım:
            entity.HasOne(d => d.Product)
                  .WithMany(p => p.Orders)
                  .HasForeignKey(d => d.ProductId)
                  .OnDelete(DeleteBehavior.Cascade); // Ürün silinirse siparişleri de silinsin
        });

        // --- Ürün (Product) Ayarları ---
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK_Product");
            entity.ToTable("Product");

            entity.Property(e => e.ProductId).HasColumnName("Product_ID");
            entity.Property(e => e.InvoiceId).HasColumnName("Invoice_ID");
            entity.Property(e => e.MaterialId).HasColumnName("Material_ID");
            entity.Property(e => e.SubcontractorId).HasColumnName("Subcontractor_ID");
            entity.Property(e => e.Type).HasMaxLength(50);

            entity.HasOne(d => d.Invoice).WithMany(p => p.Products)
                .HasForeignKey(d => d.InvoiceId)
                .HasConstraintName("FK_Product_Invoice");

            entity.HasOne(d => d.Material).WithMany(p => p.Products)
                .HasForeignKey(d => d.MaterialId)
                .HasConstraintName("FK_Product_Material");

            entity.HasOne(d => d.Subcontractor).WithMany(p => p.Products)
                .HasForeignKey(d => d.SubcontractorId)
                .HasConstraintName("FK_Product_Subcontractor");
        });

        // --- Tedarikçi (Subcontractor) Ayarları ---
        modelBuilder.Entity<Subcontractor>(entity =>
        {
            entity.HasKey(e => e.SubcontractorId).HasName("PK_Subcontractor");
            entity.ToTable("Subcontractor");

            entity.Property(e => e.SubcontractorId).HasColumnName("Subcontractor_ID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.PostalCode).HasMaxLength(20).HasColumnName("Postal_Code");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}