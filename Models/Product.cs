using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models;

public partial class Product
{
    [Key]
    public int ProductId { get; set; }

    // --- VALIDASYON KURALLARI ---

    [Required(ErrorMessage = "Lütfen ürün adını giriniz.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Ürün adı 2 ile 100 karakter arasında olmalıdır.")]
    [Display(Name = "Ürün Adı")]
    public string? Type { get; set; }

    [Range(0, 100000, ErrorMessage = "Stok adedi 0'dan küçük olamaz.")]
    [Display(Name = "Stok Miktarı")]
    public int? Stock { get; set; }

    [Display(Name = "Satışta mı?")]
    public bool? Availability { get; set; }

    // --- İLİŞKİLER (Foreign Keys) ---

    public int? MaterialId { get; set; }
    public int? SubcontractorId { get; set; }
    public int? InvoiceId { get; set; }

    // --- NAVIGATION PROPERTIES (Bağlantılar) ---

    public virtual Invoice? Invoice { get; set; }
    public virtual Material? Material { get; set; }
    public virtual Subcontractor? Subcontractor { get; set; }

    // Siparişler listesi (Eski kodunda vardı, koruyoruz)
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}