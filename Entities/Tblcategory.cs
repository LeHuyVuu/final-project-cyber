using System;
using System.Collections.Generic;

namespace cybersoft_final_project.Entities;

public partial class Tblcategory
{
    public int Categoryid { get; set; }

    public string? Categoryname { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Tblproduct> Tblproducts { get; set; } = new List<Tblproduct>();
}
