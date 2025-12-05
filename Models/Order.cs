using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        public string? OrderType { get; set; } // "Satış" veya "Alım"

        public int Quantity { get; set; } // Kaç adet?

        public DateTime OrderDate { get; set; } // Ne zaman?

        // Hangi Ürün?
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }

        // İlerde Müşteri (Customer) ID de buraya eklenebilir.
    }
}