using System;
using System.Collections.Generic;

namespace cybersoft_final_project.Entities;

public partial class Tblorder
{
    public int Orderid { get; set; }

    public DateTime? Orderdate { get; set; }

    public decimal? Total { get; set; }

    public int? Userid { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Tblorderdetail> Tblorderdetails { get; set; } = new List<Tblorderdetail>();

    public virtual Tbluser? User { get; set; }
}
