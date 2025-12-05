using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "Admin")] // Sadece Admin yönetebilir
    public class SubcontractorsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public SubcontractorsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var suppliers = await _unitOfWork.Subcontractors.GetAllAsync();
            return View(suppliers);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Subcontractor subcontractor)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.Subcontractors.AddAsync(subcontractor);
                // UnitOfWork mantığında SaveChanges burada çağrılmalı ama
                // bizim Repository içindeki AddAsync zaten kaydediyor.
                // İleride sadece CompleteAsync() ile de yapabiliriz.

                return RedirectToAction(nameof(Index));
            }
            return View(subcontractor);
        }

        // Silme İşlemi
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _unitOfWork.Subcontractors.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}