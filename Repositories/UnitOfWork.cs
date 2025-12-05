using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly WarehouseDbContext _context;

        public IProductRepository Products { get; private set; }
        public ISubcontractorRepository Subcontractors { get; private set; }
        public IOrderRepository Orders { get; private set; }
        public IActivityLogRepository ActivityLogs { get; private set; } // Özellik Tanımı

        public UnitOfWork(WarehouseDbContext context)
        {
            _context = context;
            // İŞÇİLERİ BAŞLATIYORUZ
            Products = new ProductRepository(_context);
            Subcontractors = new SubcontractorRepository(_context);
            Orders = new OrderRepository(_context);

            // --- BU SATIR EKSİKSE HATA VERİR ---
            ActivityLogs = new ActivityLogRepository(_context);
            // -----------------------------------
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}