using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace cybersoft_final_project.Entities;

[Table("recommendation_product")]
[Index("productid", Name = "productid")]
[Index("recommendation_type_id", Name = "recommendation_type_id")]
public partial class recommendation_product
{
    [Key]
    public int id { get; set; }

    public int recommendation_type_id { get; set; }

    public int productid { get; set; }

    [Column(TypeName = "timestamp")]
    public DateTime? created_at { get; set; }

    [ForeignKey("productid")]
    [InverseProperty("recommendation_products")]
    public virtual product product { get; set; } = null!;

    [ForeignKey("recommendation_type_id")]
    [InverseProperty("recommendation_products")]
    public virtual recommendation_type recommendation_type { get; set; } = null!;
}
