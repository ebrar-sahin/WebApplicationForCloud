using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        // 1. Sürükle-Bırak için özel metot
        Task UpdateStatusAsync(int id, bool status, int? newStock);

        // 2. İŞTE EKSİK OLAN KISIM: Dropdown Verilerini İsteme Metotları
        Task<List<Subcontractor>> GetSubcontractorsAsync();
        Task<List<Material>> GetMaterialsAsync();
        Task<List<Invoice>> GetInvoicesAsync();

        bool ProductExists(int id);
        // Arama Metodu: Hem ürün adına hem de barkoda göre arayabilsin
        Task<List<Product>> SearchAsync(string keyword);

    }
}