using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SecureApp.Data;
using SecureApp.Models;
using SecureApp.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
  options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddIdentity<AppUser, IdentityRole>(options => {
  // Krav til password (eksempler)
  options.Password.RequireDigit = true;
  options.Password.RequiredLength = 8;
  options.Password.RequireNonAlphanumeric = false;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// Konfigurerer cookie-redirects for AccountController
builder.Services.ConfigureApplicationCookie(options => {
  options.LoginPath = "/Account/Login";
  options.AccessDeniedPath = "/Account/AccessDenied";

  // cookie levetid (f.eks. 14 dage),
  // dernæst skal man logge ind igen
  options.ExpireTimeSpan = TimeSpan.FromDays(14);
});

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IOrderRepository, OrderRepository>();

var app = builder.Build();

// initielle data til Identity (brugere og roller)
using (IServiceScope scope = app.Services.CreateScope())
  IdentityInitializer.SeedData(scope.ServiceProvider);

app.UseRouting();

app.UseStaticFiles();
app.MapStaticAssets();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
  .WithStaticAssets();

app.Run();
