using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Areas.Identity.Data; // Kendi user sınıfının yolu
using WebApplication1.Models; // ActivityLog için gerekli
using System.Linq;
using System.Threading.Tasks;
using System;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "Admin")] // Sadece Admin girebilir!
    public class AdminController : Controller
    {
        private readonly UserManager<WebApplication1User> _userManager;
        // Loglama için context lazım. Repository şart değil ama context şart.
        private readonly WarehouseDbContext _context;

        // Constructor'da dependency injection yapıyoruz
        public AdminController(UserManager<WebApplication1User> userManager, WarehouseDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // --- EKSİK OLAN INDEX METODU ---
        // Kullanıcıları listeler
        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }
        // -------------------------------

        // Kullanıcı Silme (Loglama dahil)
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                // 1. Önce Logla
                var log = new ActivityLog
                {
                    ActionType = "Kullanıcı Silindi",
                    Description = $"Admin, {user.Email} kullanıcısını sildi.",
                    LogDate = DateTime.Now
                    // Product_ID null kalabilir
                };

                _context.ActivityLogs.Add(log);
                await _context.SaveChangesAsync();

                // 2. Sonra Sil
                await _userManager.DeleteAsync(user);
            }
            return RedirectToAction("Index");
        }
    }
}