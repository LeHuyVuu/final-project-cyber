using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace cybersoft_final_project.Entities;

[Table("product_image")]
[Index("productid", Name = "productid")]
public partial class product_image
{
    [Key]
    public int imageid { get; set; }

    public int productid { get; set; }

    [Column(TypeName = "text")]
    public string image_url { get; set; } = null!;

    [Column(TypeName = "timestamp")]
    public DateTime? created_at { get; set; }

    [ForeignKey("productid")]
    [InverseProperty("product_images")]
    public virtual product product { get; set; } = null!;
}
