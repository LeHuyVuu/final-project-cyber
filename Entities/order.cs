using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace cybersoft_final_project.Entities;

[Table("order")]
[Index("userid", Name = "idx_userid")]
public partial class order
{
    [Key]
    public int orderid { get; set; }

    [Column(TypeName = "timestamp")]
    public DateTime? orderdate { get; set; }

    [Precision(10, 0)]
    public decimal? total { get; set; }

    public int? userid { get; set; }

    [StringLength(50)]
    public string? status { get; set; }

    [InverseProperty("order")]
    public virtual ICollection<orderdetail> orderdetails { get; set; } = new List<orderdetail>();
}
