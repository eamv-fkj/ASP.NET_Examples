using FileApp.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

string uploadPath = Path.Combine(
  builder.Environment.ContentRootPath, 
  builder.Configuration["UploadPath"] ?? "wwwroot/images");

builder.Services.AddSingleton<IFileRepository>(new FileRepository(uploadPath));

var app = builder.Build();

app.UseRouting();

app.UseStaticFiles();
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
  .WithStaticAssets();

app.Run();
