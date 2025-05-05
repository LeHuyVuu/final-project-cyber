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

    public virtual DbSet<Tblcategory> Tblcategories { get; set; }

    public virtual DbSet<Tblorder> Tblorders { get; set; }

    public virtual DbSet<Tblorderdetail> Tblorderdetails { get; set; }

    public virtual DbSet<Tblproduct> Tblproducts { get; set; }

    public virtual DbSet<Tblproductattribute> Tblproductattributes { get; set; }

    public virtual DbSet<Tblproductimage> Tblproductimages { get; set; }

    public virtual DbSet<Tbluser>  Tblusers { get; set; }

    private string GetConnectionString()
    {
        // Khởi tạo ConfigurationBuilder và cấu hình tệp appsettings.json
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory()) // Đặt đường dẫn cơ sở cho tệp cấu hình
            .AddJsonFile("appsettings.json", optional: false,
                reloadOnChange: true) // Thêm tệp appsettings.json vào cấu hình
            .Build(); // Xây dựng đối tượng IConfiguration

        // Lấy giá trị từ ConnectionStrings trong tệp appsettings.json
        var strConn = config.GetConnectionString("DefaultConnection");

        // Trả về chuỗi kết nối
        return strConn;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySQL(GetConnectionString());

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tblcategory>(entity =>
        {
            entity.HasKey(e => e.Categoryid).HasName("PRIMARY");

            entity.ToTable("tblcategory");

            entity.Property(e => e.Categoryid).HasColumnName("categoryid");
            entity.Property(e => e.Categoryname)
                .HasMaxLength(255)
                .HasColumnName("categoryname");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
        });

        modelBuilder.Entity<Tblorder>(entity =>
        {
            entity.HasKey(e => e.Orderid).HasName("PRIMARY");

            entity.ToTable("tblorder");

            entity.HasIndex(e => e.Userid, "userid");

            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Orderdate)
                .HasColumnType("timestamp")
                .HasColumnName("orderdate");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.Total)
                .HasPrecision(10)
                .HasColumnName("total");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.User).WithMany(p => p.Tblorders)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("tblorder_ibfk_1");
        });

        modelBuilder.Entity<Tblorderdetail>(entity =>
        {
            entity.HasKey(e => e.Detailid).HasName("PRIMARY");

            entity.ToTable("tblorderdetail");

            entity.HasIndex(e => e.Orderid, "orderid");

            entity.HasIndex(e => e.Productid, "productid");

            entity.Property(e => e.Detailid).HasColumnName("detailid");
            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Price)
                .HasPrecision(10)
                .HasColumnName("price");
            entity.Property(e => e.Productid).HasColumnName("productid");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");

            entity.HasOne(d => d.Order).WithMany(p => p.Tblorderdetails)
                .HasForeignKey(d => d.Orderid)
                .HasConstraintName("tblorderdetail_ibfk_1");

            entity.HasOne(d => d.Product).WithMany(p => p.Tblorderdetails)
                .HasForeignKey(d => d.Productid)
                .HasConstraintName("tblorderdetail_ibfk_2");
        });

        modelBuilder.Entity<Tblproduct>(entity =>
        {
            entity.HasKey(e => e.Productid).HasName("PRIMARY");

            entity.ToTable("tblproduct");

            entity.HasIndex(e => e.Categoryid, "categoryid");

            entity.Property(e => e.Productid).HasColumnName("productid");
            entity.Property(e => e.Brand)
                .HasMaxLength(100)
                .HasColumnName("brand");
            entity.Property(e => e.Categoryid).HasColumnName("categoryid");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.DiscountPercent).HasColumnName("discount_percent");
            entity.Property(e => e.DiscountPrice)
                .HasPrecision(10)
                .HasColumnName("discount_price");
            entity.Property(e => e.FlashSale).HasColumnName("flash_sale");
            entity.Property(e => e.Image)
                .HasColumnType("text")
                .HasColumnName("image");
            entity.Property(e => e.Importdate)
                .HasColumnType("date")
                .HasColumnName("importdate");
            entity.Property(e => e.IsAvailable).HasColumnName("is_available");
            entity.Property(e => e.Price)
                .HasPrecision(10)
                .HasColumnName("price");
            entity.Property(e => e.Productname)
                .HasMaxLength(255)
                .HasColumnName("productname");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.RatingAvg)
                .HasColumnType("float(5,2)")
                .HasColumnName("rating_avg");
            entity.Property(e => e.RatingCount).HasColumnName("rating_count");
            entity.Property(e => e.Recommendation)
                .HasMaxLength(255)
                .HasColumnName("recommendation");
            entity.Property(e => e.Sku)
                .HasMaxLength(50)
                .HasColumnName("sku");
            entity.Property(e => e.SoldQuantity).HasColumnName("sold_quantity");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.StockQuantity).HasColumnName("stock_quantity");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp")
                .HasColumnName("updated_at");
            entity.Property(e => e.Usingdate)
                .HasColumnType("date")
                .HasColumnName("usingdate");

            entity.HasOne(d => d.Category).WithMany(p => p.Tblproducts)
                .HasForeignKey(d => d.Categoryid)
                .HasConstraintName("tblproduct_ibfk_1");
        });

        modelBuilder.Entity<Tblproductattribute>(entity =>
        {
            entity.HasKey(e => e.Attributeid).HasName("PRIMARY");

            entity.ToTable("tblproductattribute");

            entity.HasIndex(e => e.Productid, "productid");

            entity.Property(e => e.Attributeid).HasColumnName("attributeid");
            entity.Property(e => e.AttributeName)
                .HasMaxLength(255)
                .HasColumnName("attribute_name");
            entity.Property(e => e.AttributeValue)
                .HasMaxLength(255)
                .HasColumnName("attribute_value");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.Productid).HasColumnName("productid");

            entity.HasOne(d => d.Product).WithMany(p => p.Tblproductattributes)
                .HasForeignKey(d => d.Productid)
                .HasConstraintName("tblproductattribute_ibfk_1");
        });

        modelBuilder.Entity<Tblproductimage>(entity =>
        {
            entity.HasKey(e => e.Imageid).HasName("PRIMARY");

            entity.ToTable("tblproductimage");

            entity.HasIndex(e => e.Productid, "productid");

            entity.Property(e => e.Imageid).HasColumnName("imageid");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.ImageUrl)
                .HasColumnType("text")
                .HasColumnName("image_url");
            entity.Property(e => e.Productid).HasColumnName("productid");

            entity.HasOne(d => d.Product).WithMany(p => p.Tblproductimages)
                .HasForeignKey(d => d.Productid)
                .HasConstraintName("tblproductimage_ibfk_1");
        });

        modelBuilder.Entity<Tbluser>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("PRIMARY");

            entity.ToTable("tblusers");

            entity.Property(e => e.Userid).HasColumnName("userid");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.Birthday)
                .HasColumnType("date")
                .HasColumnName("birthday");
            entity.Property(e => e.Fullname)
                .HasMaxLength(255)
                .HasColumnName("fullname");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.Roleid).HasColumnName("roleid");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}