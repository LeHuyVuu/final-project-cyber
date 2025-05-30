using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace cybersoft_final_project.Entities;

[Table("recommendation_type")]
public partial class recommendation_type
{
    [Key]
    public int id { get; set; }

    [StringLength(255)]
    public string name { get; set; } = null!;

    [Column(TypeName = "timestamp")]
    public DateTime? created_at { get; set; }

    [Column("slug-name", TypeName = "text")]
    public string slug_name { get; set; } = null!;

    [InverseProperty("recommendation_type")]
    public virtual ICollection<recommendation_product> recommendation_products { get; set; } = new List<recommendation_product>();
}
