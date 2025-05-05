using System;
using System.Collections.Generic;

namespace cybersoft_final_project.Entities;

public partial class Tblorderdetail
{
    public int Detailid { get; set; }

    public decimal? Price { get; set; }

    public int? Quantity { get; set; }

    public int? Orderid { get; set; }

    public int? Productid { get; set; }

    public string? Status { get; set; }

    public virtual Tblorder? Order { get; set; }

    public virtual Tblproduct? Product { get; set; }
}
