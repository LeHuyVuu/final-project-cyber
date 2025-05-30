using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace cybersoft_final_project.Entities;

public partial class user
{
    [Key]
    public int userid { get; set; }

    [StringLength(255)]
    public string? fullname { get; set; }

    [StringLength(255)]
    public string? password { get; set; }

    [StringLength(255)]
    public string? address { get; set; }

    [Column(TypeName = "date")]
    public DateTime? birthday { get; set; }

    [StringLength(20)]
    public string? phone { get; set; }

    public bool? status { get; set; }

    [StringLength(255)]
    public string username { get; set; } = null!;

    [StringLength(255)]
    public string email { get; set; } = null!;

    [Column(TypeName = "enum('admin','customer')")]
    public string? role { get; set; }
}
