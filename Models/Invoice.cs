using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Invoice
{
    public int InvoiceId { get; set; }

    public decimal? Price { get; set; }

    public decimal? Tax { get; set; }

    public DateTime? Date { get; set; }

    public DateTime? DueDate { get; set; }

    public decimal? Total { get; set; }

    public int? CustomerId { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
