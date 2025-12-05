using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Material
{
    public int MaterialId { get; set; }

    public string? MaterialType { get; set; }

    public bool? Availability { get; set; }

    public int? Stock { get; set; }

    public int? SubcontractorId { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual Subcontractor? Subcontractor { get; set; }
}
