using System;
using System.Collections.Generic;

namespace cybersoft_final_project.Entities;

public partial class Tblproductimage
{
    public int Imageid { get; set; }

    public int? Productid { get; set; }

    public string? ImageUrl { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Tblproduct? Product { get; set; }
}
