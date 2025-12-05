using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Subcontractor
{
    public int SubcontractorId { get; set; }

    public string? Name { get; set; }

    public string? Address { get; set; }

    public string? PostalCode { get; set; }

    public string? Email { get; set; }

    public virtual ICollection<Material> Materials { get; set; } = new List<Material>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
