using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace cybersoft_final_project.Entities;

[Table("productattribute")]
[Index("productid", Name = "productid")]
public partial class productattribute
{
    [Key]
    public int attributeid { get; set; }

    public int? productid { get; set; }

    [StringLength(255)]
    public string? attribute_name { get; set; }

    [StringLength(255)]
    public string? attribute_value { get; set; }

    [Column(TypeName = "timestamp")]
    public DateTime? created_at { get; set; }

    [ForeignKey("productid")]
    [InverseProperty("productattributes")]
    public virtual product? product { get; set; }
}
