using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace cybersoft_final_project.Entities;

[Table("orderdetail")]
[Index("orderid", Name = "idx_orderid")]
[Index("productid", Name = "idx_productid")]
public partial class orderdetail
{
    [Key]
    public int detailid { get; set; }

    [Precision(10, 0)]
    public decimal? price { get; set; }

    public int? quantity { get; set; }

    public int? orderid { get; set; }

    public int? productid { get; set; }

    [StringLength(50)]
    public string? status { get; set; }

    [ForeignKey("orderid")]
    [InverseProperty("orderdetails")]
    public virtual order? order { get; set; }

    [ForeignKey("productid")]
    [InverseProperty("orderdetails")]
    public virtual product? product { get; set; }
}
