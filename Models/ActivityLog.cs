using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // <--- BU SATIR EKLENDİ

namespace WebApplication1.Models
{
    public class ActivityLog
    {
        [Key]
        public int Log_ID { get; set; }

        // Veritabanındaki sütun adı (Alt tireli)
        public int? Product_ID { get; set; }

        public string ActionType { get; set; }

        public string Description { get; set; }

        public DateTime LogDate { get; set; }

        // --- DÜZELTME BURADA ---
        // Bu etiketi ekleyerek EF'ye diyoruz ki: 
        // "Bu ilişkiyi kurarken Product_ID sütununu kullan, ProductId arama."
        [ForeignKey("Product_ID")]
        public virtual Product Product { get; set; }
    }
}