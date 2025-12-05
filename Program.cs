using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. SERVİSLERİ EKLEME (Services)
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages(); // Identity sayfaları için gerekli

// Ürün Veritabanı Bağlantısı
builder.Services.AddDbContext<WarehouseDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Güvenlik (Identity) Veritabanı Bağlantısı
builder.Services.AddDbContext<WebApplication1Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Unit of Work Tanımlaması
builder.Services.AddScoped<WebApplication1.Repositories.IUnitOfWork, WebApplication1.Repositories.UnitOfWork>();
// Identity (Üyelik) Ayarları (Şifre zorluğunu kaldırdık)
builder.Services.AddIdentity<WebApplication1User, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false; // Email onayı kapalı
    options.Password.RequireNonAlphanumeric = false; // Sembol zorunluluğu yok
    options.Password.RequireUppercase = false;       // Büyük harf zorunluluğu yok
    options.Password.RequireDigit = false;           // Rakam zorunluluğu yok
    options.Password.RequiredLength = 3;             // En az 3 karakter
})
    .AddEntityFrameworkStores<WebApplication1Context>()
    .AddDefaultTokenProviders()
    .AddDefaultUI();

// --- UYGULAMAYI OLUŞTUR (BU SATIR SADECE BURADA OLMALI) ---
var app = builder.Build();
// -----------------------------------------------------------

// 2. OTOMATİK ADMİN OLUŞTURMA (Seed Method)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<WebApplication1User>>();

        // "Admin" rolü yoksa oluştur
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        // Admin kullanıcısı yoksa oluştur (admin@depo.com / Admin123!)
        var adminEmail = "admin@depo.com";
        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            var newAdmin = new WebApplication1User { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
            await userManager.CreateAsync(newAdmin, "Admin123!");
            await userManager.AddToRoleAsync(newAdmin, "Admin");
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Admin oluşturulurken bir hata meydana geldi.");
    }
}

// 3. HTTP İSTEK HATTI (Pipeline)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Kimlik doğrulama (Kimsin?)
app.UseAuthorization();  // Yetkilendirme (Girebilir misin?)

// Rotalar
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Products}/{action=Index}/{id?}"); // Ana sayfa Products olsun

app.MapRazorPages(); // Identity sayfalarını aktif et

app.Run();