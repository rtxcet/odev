using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Odev;
using Odev.Models;
using Odev.Utility.Account;

var builder = WebApplication.CreateBuilder(args);

// Veritabaný baðlantýsýný yapýlandýrýyoruz
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 26)))); // MySQL versiyonunu doðru belirtin

// Identity ekliyoruz
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

//Idenetity Özellik
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false; // Rakam gereksinimi kapalý
    options.Password.RequiredLength = 4;   // Minimum 4 karakter
    options.Password.RequireNonAlphanumeric = false; // Özel karakter gereksinimi kapalý
    options.Password.RequireUppercase = false; // Büyük harf gereksinimi kapalý
    options.Password.RequireLowercase = false; // Küçük harf gereksinimi kapalý
});

// Login
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/admin/login/index";
    options.AccessDeniedPath = "/admin/login/index";
});

// Data Protection (Önemli ve gerekli!)
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(builder.Environment.ContentRootPath, "datakeys")));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Seed default user (app oluþturulduktan sonra eklenir)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    // Default admin ve rol eklenmesi için Account.SeedDefaultUser çaðrýlýr
    await Account.SeedDefaultUser(userManager, roleManager);
}




// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();  // Authentication önce gelir
app.UseAuthorization();   // Authorization'dan önce gelmeli

app.MapControllerRoute(
    name: "admin",
    pattern: "{area:exists}/{controller=Default}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Default}/{action=Index}/{id?}");


app.Run();
