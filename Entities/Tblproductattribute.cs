using System;
using System.Collections.Generic;

namespace cybersoft_final_project.Entities;

public partial class Tblproductattribute
{
    public int Attributeid { get; set; }

    public int? Productid { get; set; }

    public string? AttributeName { get; set; }

    public string? AttributeValue { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Tblproduct? Product { get; set; }
}
