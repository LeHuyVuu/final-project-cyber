using System;
using System.Collections.Generic;

namespace cybersoft_final_project.Entities;

public partial class Tblproduct
{
    public int Productid { get; set; }

    public string? Productname { get; set; }

    public string? Image { get; set; }

    public decimal? Price { get; set; }

    public int? Quantity { get; set; }

    public DateTime? Importdate { get; set; }

    public DateTime? Usingdate { get; set; }

    public string? Brand { get; set; }

    public string? Sku { get; set; }

    public decimal? DiscountPrice { get; set; }

    public int? DiscountPercent { get; set; }

    public int? StockQuantity { get; set; }

    public int? SoldQuantity { get; set; }

    public bool? IsAvailable { get; set; }

    public float? RatingAvg { get; set; }

    public int? RatingCount { get; set; }

    public int? Categoryid { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? Status { get; set; }

    public string? Recommendation { get; set; }

    public bool FlashSale { get; set; }

    public virtual Tblcategory? Category { get; set; }

    public virtual ICollection<Tblorderdetail> Tblorderdetails { get; set; } = new List<Tblorderdetail>();

    public virtual ICollection<Tblproductattribute> Tblproductattributes { get; set; } = new List<Tblproductattribute>();

    public virtual ICollection<Tblproductimage> Tblproductimages { get; set; } = new List<Tblproductimage>();
}
