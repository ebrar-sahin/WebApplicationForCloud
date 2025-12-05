using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication1.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        // ARTIK _context YOK, SADECE UNIT OF WORK VAR
        public ProductsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Products
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;
            var products = await _unitOfWork.Products.SearchAsync(searchString);
            return View(products);
        }

        // GET: Products/Create
        public async Task<IActionResult> Create()
        {
            var subcontractors = await _unitOfWork.Products.GetSubcontractorsAsync();
            var materials = await _unitOfWork.Products.GetMaterialsAsync();
            var invoices = await _unitOfWork.Products.GetInvoicesAsync();

            ViewBag.Subcontractors = new SelectList(subcontractors ?? new List<Subcontractor>(), "SubcontractorId", "Name");
            ViewBag.Materials = new SelectList(materials ?? new List<Material>(), "MaterialId", "MaterialType");
            ViewBag.Invoices = new SelectList(invoices ?? new List<Invoice>(), "InvoiceId", "InvoiceId");

            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (product.Stock > 0) product.Availability = true;
            else
            {
                product.Availability = false;
                product.Stock = 0;
            }
            ModelState.Remove("Availability");

            if (ModelState.IsValid)
            {
                // 1. Ürünü Ekle
                await _unitOfWork.Products.AddAsync(product);

                // 2. Logu Ekle (Artık UnitOfWork üzerinden)
                var log = new ActivityLog
                {
                    ActionType = "Yeni Ürün",
                    Description = $"Yeni ürün eklendi: {product.Type}",
                    LogDate = DateTime.Now
                };
                await _unitOfWork.ActivityLogs.AddAsync(log);

                // Aslında AddAsync içinde SaveChanges var ama garanti olsun
                // Unit of Work mantığında genelde en sonda bir kere SaveChanges çağrılır.
                // Bizim Repository yapımızda her işlem kendi kaydediyor, sorun yok.

                TempData["SuccessMessage"] = "Ürün başarıyla eklendi.";
                return RedirectToAction(nameof(Index));
            }

            var subcontractors = await _unitOfWork.Products.GetSubcontractorsAsync();
            var materials = await _unitOfWork.Products.GetMaterialsAsync();
            var invoices = await _unitOfWork.Products.GetInvoicesAsync();

            ViewBag.Subcontractors = new SelectList(subcontractors ?? new List<Subcontractor>(), "SubcontractorId", "Name");
            ViewBag.Materials = new SelectList(materials ?? new List<Material>(), "MaterialId", "MaterialType");
            ViewBag.Invoices = new SelectList(invoices ?? new List<Invoice>(), "InvoiceId", "InvoiceId");

            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.ProductId) return NotFound();

            if (product.Stock > 0) product.Availability = true;
            else
            {
                product.Availability = false;
                product.Stock = 0;
            }
            ModelState.Remove("Availability");

            if (ModelState.IsValid)
            {
                try
                {
                    await _unitOfWork.Products.UpdateAsync(product);

                    // Güncelleme Logu
                    var log = new ActivityLog
                    {
                        Product_ID = id,
                        ActionType = "Güncelleme",
                        Description = $"{product.Type} güncellendi.",
                        LogDate = DateTime.Now
                    };
                    await _unitOfWork.ActivityLogs.AddAsync(log);

                    TempData["SuccessMessage"] = "Ürün güncellendi.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_unitOfWork.Products.ProductExists(product.ProductId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Silmeden önce log atalım (Çünkü silince ID boşa çıkabilir)
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product != null)
            {
                var log = new ActivityLog
                {
                    // Silinen ürünün ID'sini tutmak riskli olabilir (Constraint hatası), 
                    // o yüzden null bırakıp açıklamaya yazıyoruz.
                    Product_ID = null,
                    ActionType = "Silme",
                    Description = $"{product.Type} silindi.",
                    LogDate = DateTime.Now
                };
                await _unitOfWork.ActivityLogs.AddAsync(log);
            }

            await _unitOfWork.Products.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // --- SÜRÜKLE BIRAK & SİPARİŞ & LOGLAMA ---
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, bool status, int? newStock)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null) return NotFound();

            int oldStock = product.Stock ?? 0;
            int finalStock = oldStock;

            // 1. YENİ STOK BELİRLEME
            if (newStock.HasValue)
            {
                finalStock = newStock.Value;
                product.Stock = finalStock;
            }

            // 2. DURUM BELİRLEME
            if (finalStock > 0) product.Availability = true;
            else
            {
                product.Availability = false;
                product.Stock = 0;
                finalStock = 0;
            }

            // Ürünü Güncelle
            await _unitOfWork.Products.UpdateAsync(product);

            // 3. SİPARİŞ (ORDER) OLUŞTURMA
            int fark = finalStock - oldStock;
            if (fark != 0)
            {
                var order = new Order
                {
                    ProductId = id,
                    OrderDate = DateTime.Now,
                    Quantity = Math.Abs(fark),
                    OrderType = fark > 0 ? "Stok Girişi (Alım)" : "Stok Çıkışı (Satış)"
                };
                await _unitOfWork.Orders.AddAsync(order);
            }

            // 4. LOGLAMA (LOGGING)
            string actionMessage = "";
            string actionType = "";

            if (fark > 0)
            {
                actionType = "Stok Girişi";
                actionMessage = $"Stok güncellendi. Eski: {oldStock}, Yeni: {finalStock} (Fark: +{fark})";
            }
            else if (fark < 0)
            {
                actionType = "Tükendi/Satış";
                actionMessage = $"Stok azaldı. Eski: {oldStock}, Yeni: {finalStock} (Fark: {fark})";
            }
            else
            {
                // Fark yoksa sadece durum değişmiştir
                actionType = "Durum Değişimi";
                actionMessage = $"Durum {(status ? "Var" : "Yok")} olarak işaretlendi.";
            }

            var newLog = new ActivityLog
            {
                Product_ID = id,
                ActionType = actionType,
                Description = actionMessage,
                LogDate = DateTime.Now
            };

            // LOGU EKLE VE KAYDET
            await _unitOfWork.ActivityLogs.AddAsync(newLog);

            // !!! GARANTİ KAYIT !!! (Repository AddAsync içinde zaten save var ama burada emin olalım)
            // Eğer AddAsync içinde SaveChanges varsa buraya gerek yok, ama yoksa veya UnitOfWork mantığı gereği:
            // await _unitOfWork.CompleteAsync(); // Eğer IUnitOfWork'te bu metod varsa açabilirsin.

            return Ok(new { message = "Güncellendi" });
        }
    }
}