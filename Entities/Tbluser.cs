using System;
using System.Collections.Generic;

namespace cybersoft_final_project.Entities;

public partial class Tbluser
{
    public int Userid { get; set; }

    public string? Fullname { get; set; }

    public string? Password { get; set; }

    public int? Roleid { get; set; }

    public string? Address { get; set; }

    public DateTime? Birthday { get; set; }

    public string? Phone { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<Tblorder> Tblorders { get; set; } = new List<Tblorder>();
}
