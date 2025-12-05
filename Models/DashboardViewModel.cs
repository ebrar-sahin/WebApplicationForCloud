namespace WebApplication1.Models
{
    public class DashboardViewModel
    {
        public int TotalProducts { get; set; }      // Toplam Ürün
        public int CriticalStockCount { get; set; } // Stoğu 10'dan az olanlar
        public int TotalSubcontractors { get; set; } // Toplam Tedarikçi
        public int RecentLogsCount { get; set; }    // Son hareket sayısı

        // İleride buraya grafik verileri de eklenebilir
    }
}