using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Event
{
    public int EventId { get; set; }

    public string? Location { get; set; }

    public DateTime? Date { get; set; }

    public int? AddressId { get; set; }

    public int? CustomerId { get; set; }

    public virtual Customer? Customer { get; set; }
}
