using BackEndProject.Database;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDb>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseServer"));
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();
app.MapControllerRoute(name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();