using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // CountAsync için gerekli
using WebApplication1.Models;
using WebApplication1.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication1.Controllers
{
    [Authorize] // Ana sayfayý da kilitleyelim
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly WarehouseDbContext _context; // Loglarý saymak için

        public HomeController(IUnitOfWork unitOfWork, WarehouseDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // 1. Verileri Çek
            var allProducts = await _unitOfWork.Products.GetAllAsync();
            var allSubs = await _unitOfWork.Subcontractors.GetAllAsync();
            var logsCount = await _context.ActivityLogs.CountAsync();

            // 2. ViewModel'i Doldur
            var dashboardModel = new DashboardViewModel
            {
                TotalProducts = allProducts.Count,
                CriticalStockCount = allProducts.Count(p => p.Stock < 10), // Kritik stok (10'dan az)
                TotalSubcontractors = allSubs.Count,
                RecentLogsCount = logsCount
            };

            // 3. View'a Gönder
            return View(dashboardModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}