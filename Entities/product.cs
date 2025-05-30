using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace cybersoft_final_project.Entities;

[Table("product")]
[Index("categoryid", Name = "categoryid")]
public partial class product
{
    [Key]
    public int productid { get; set; }

    [StringLength(255)]
    public string? productname { get; set; }

    [StringLength(255)]
    public string? image { get; set; }

    [Precision(10, 0)]
    public decimal? price { get; set; }

    public int? quantity { get; set; }

    [Column(TypeName = "date")]
    public DateTime? importdate { get; set; }

    [Column(TypeName = "date")]
    public DateTime? usingdate { get; set; }

    [StringLength(100)]
    public string? brand { get; set; }

    [StringLength(50)]
    public string? sku { get; set; }

    [Precision(10, 0)]
    public decimal? discount_price { get; set; }

    public int? discount_percent { get; set; }

    public int? stock_quantity { get; set; }

    public int? sold_quantity { get; set; }

    public bool? is_available { get; set; }

    [Column(TypeName = "float(5,2)")]
    public float? rating_avg { get; set; }

    public int? rating_count { get; set; }

    public int? categoryid { get; set; }

    [Column(TypeName = "timestamp")]
    public DateTime? created_at { get; set; }

    [Column(TypeName = "timestamp")]
    public DateTime? updated_at { get; set; }

    public bool? status { get; set; }

    [Column(TypeName = "text")]
    public string? highlight { get; set; }

    public bool? is_sale { get; set; }

    [ForeignKey("categoryid")]
    [InverseProperty("products")]
    public virtual category? category { get; set; }

    [InverseProperty("product")]
    public virtual ICollection<orderdetail> orderdetails { get; set; } = new List<orderdetail>();

    [InverseProperty("product")]
    public virtual ICollection<product_image> product_images { get; set; } = new List<product_image>();

    [InverseProperty("product")]
    public virtual ICollection<productattribute> productattributes { get; set; } = new List<productattribute>();

    [InverseProperty("product")]
    public virtual ICollection<recommendation_product> recommendation_products { get; set; } = new List<recommendation_product>();
}
