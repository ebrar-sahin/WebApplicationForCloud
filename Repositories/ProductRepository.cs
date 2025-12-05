using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace WebApplication1.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(WarehouseDbContext context) : base(context)
        {
        }

        public async Task UpdateStatusAsync(int id, bool status, int? newStock)
        {
            var product = await GetByIdAsync(id);
            if (product != null)
            {
                product.Availability = status;
                if (status == true && newStock.HasValue) product.Stock = newStock.Value;
                else if (status == false) product.Stock = 0;

                await _context.SaveChangesAsync();
            }
        }

        // --- DROPDOWN VERİLERİ ---
        public async Task<List<Subcontractor>> GetSubcontractorsAsync()
        {
            return await _context.Set<Subcontractor>().ToListAsync();
        }

        public async Task<List<Material>> GetMaterialsAsync()
        {
            return await _context.Set<Material>().ToListAsync();
        }

        public async Task<List<Invoice>> GetInvoicesAsync()
        {
            return await _context.Set<Invoice>().ToListAsync();
        }

        // --- ARAMA METODU (YENİ) ---
        public async Task<List<Product>> SearchAsync(string keyword)
        {
            // Eğer arama kutusu boşsa, bütün ürünleri getir
            if (string.IsNullOrEmpty(keyword))
            {
                return await GetAllAsync();
            }

            // Değilse; Ürün Adı (Type) içinde aranan kelime geçenleri filtrele
            // (Contains SQL'deki LIKE %kelime% işlevini görür)
            return await _context.Products
                .Where(p => p.Type.Contains(keyword))
                .ToListAsync();
        }

        public bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}