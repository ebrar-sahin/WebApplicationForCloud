using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class HistoryController : Controller
    {
        private readonly WarehouseDbContext _context;

        public HistoryController(WarehouseDbContext context)
        {
            _context = context;
        }

        // Sayfa numarası (pageNumber) alıyor. Varsayılan 1.
        public async Task<IActionResult> Index(int? pageNumber)
        {
            // Veritabanı sorgusunu hazırlıyoruz (Henüz çekmedik)
            var logs = _context.ActivityLogs
                               .Include(l => l.Product)
                               .OrderByDescending(l => l.LogDate); // En yeni en üstte

            int pageSize = 10; // Sayfa başı 10 kayıt göster

            // Sayfalanmış listeyi View'a gönder
            return View(await PaginatedList<WebApplication1.Models.ActivityLog>.CreateAsync(logs.AsNoTracking(), pageNumber ?? 1, pageSize));
        }
    }
}