using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace cybersoft_final_project.Entities;

[Table("category")]
public partial class category
{
    [Key]
    public int categoryid { get; set; }

    [StringLength(255)]
    public string? categoryname { get; set; }

    [Column(TypeName = "timestamp")]
    public DateTime? created_at { get; set; }

    [Column(TypeName = "text")]
    public string? image { get; set; }

    [InverseProperty("category")]
    public virtual ICollection<product> products { get; set; } = new List<product>();
}
