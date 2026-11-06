using BookApp.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register the repository in dependency injection.
// Singleton: one shared instance, so the in-memory repository
// persists across requests.
builder.Services.AddSingleton<IBookRepository, BookRepository>();

// Build the app. This turns the configured services into a ready-to-run
// web application that can start handling requests.
var app = builder.Build();

// --- The HTTP request pipeline ---
// Every incoming request flows through the middleware registered below,
// top to bottom, before an endpoint processes it.

// Enable routing so the endpoints mapped further down (static assets and
// controller routes) can match incoming requests and assign a handler.
app.UseRouting();

// Serve files from wwwroot (CSS, JS, images) directly as static files.
app.MapStaticAssets();

// Map the default controller route. A request like /Book/Edit/2 is resolved
// to the BookController, action Edit, with id = 2. If parts are left out,
// HomeController and Index are used as defaults.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
  .WithStaticAssets();

// Start listening for HTTP requests and block here until the server stops.
app.Run();