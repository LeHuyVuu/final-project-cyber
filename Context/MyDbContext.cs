using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using cybersoft_final_project.Entities;

namespace cybersoft_final_project.Context;

public partial class MyDbContext : DbContext
{
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<__EFMigrationsHistory> __EFMigrationsHistories { get; set; }

    public virtual DbSet<category> categories { get; set; }

    public virtual DbSet<order> orders { get; set; }

    public virtual DbSet<orderdetail> orderdetails { get; set; }

    public virtual DbSet<product> products { get; set; }

    public virtual DbSet<product_image> product_images { get; set; }

    public virtual DbSet<productattribute> productattributes { get; set; }

    public virtual DbSet<recommendation_product> recommendation_products { get; set; }

    public virtual DbSet<recommendation_type> recommendation_types { get; set; }

    public virtual DbSet<user> users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySQL("Server=mysql-189f42b1-lehuyvuok-ac0e.l.aivencloud.com;Port=22626;Database=defaultdb;Uid=avnadmin;Pwd=AVNS_c3ZWGDumzgd6oRFSAr8;SslMode=Required;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<__EFMigrationsHistory>(entity =>
        {
            entity.HasKey(e => e.MigrationId).HasName("PRIMARY");
        });

        modelBuilder.Entity<category>(entity =>
        {
            entity.HasKey(e => e.categoryid).HasName("PRIMARY");
        });

        modelBuilder.Entity<order>(entity =>
        {
            entity.HasKey(e => e.orderid).HasName("PRIMARY");

            entity.Property(e => e.orderdate).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.status).HasDefaultValueSql("'Processing'");
        });

        modelBuilder.Entity<orderdetail>(entity =>
        {
            entity.HasKey(e => e.detailid).HasName("PRIMARY");

            entity.Property(e => e.status).HasDefaultValueSql("'Processing'");

            entity.HasOne(d => d.order).WithMany(p => p.orderdetails)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_orderdetail_order");

            entity.HasOne(d => d.product).WithMany(p => p.orderdetails)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_orderdetail_product");
        });

        modelBuilder.Entity<product>(entity =>
        {
            entity.HasKey(e => e.productid).HasName("PRIMARY");

            entity.HasOne(d => d.category).WithMany(p => p.products).HasConstraintName("product_ibfk_1");
        });

        modelBuilder.Entity<product_image>(entity =>
        {
            entity.HasKey(e => e.imageid).HasName("PRIMARY");

            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.product).WithMany(p => p.product_images)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("product_image_ibfk_1");
        });

        modelBuilder.Entity<productattribute>(entity =>
        {
            entity.HasKey(e => e.attributeid).HasName("PRIMARY");

            entity.HasOne(d => d.product).WithMany(p => p.productattributes).HasConstraintName("productattribute_ibfk_1");
        });

        modelBuilder.Entity<recommendation_product>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.product).WithMany(p => p.recommendation_products)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("recommendation_product_ibfk_2");

            entity.HasOne(d => d.recommendation_type).WithMany(p => p.recommendation_products)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("recommendation_product_ibfk_1");
        });

        modelBuilder.Entity<recommendation_type>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<user>(entity =>
        {
            entity.HasKey(e => e.userid).HasName("PRIMARY");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
