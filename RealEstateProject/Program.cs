using Microsoft.EntityFrameworkCore;
using RealEstateProject.BLL.Services;
using RealEstateProject.DAL;
using RealEstateProject.DAL.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services
    .AddScoped<DbContext, DatabaseContext>()
    .AddDbContext<DatabaseContext>(options => options.UseSqlServer("Server=DESKTOP-8FQ8PSR\\SQLEXPRESS;Database=RealEstateDb;Trusted_Connection=True;MultipleActiveResultSets=true"))
    .AddTransient<IEstateRepository, EstateRepository>()
    .AddTransient<IEstateService, EstateService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

using(IServiceScope scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<DatabaseContext>();
    if (context.Database.EnsureCreated())
    {
        context.Database.Migrate();
    }
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
