using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Net.Http.Headers;
using RealEstateProject.BLL.Services;
using RealEstateProject.DAL;
using RealEstateProject.DAL.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services
    .AddScoped<DbContext, DatabaseContext>()
    .AddDbContext<DatabaseContext>(options => options.UseSqlServer("Server=.;Database=RealEstateDb;Trusted_Connection=True;MultipleActiveResultSets=true"))
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
app.UseStaticFiles(new StaticFileOptions
{
    HttpsCompression = HttpsCompressionMode.Compress,
    FileProvider = new CompositeFileProvider(app.Environment.WebRootFileProvider),
    RequestPath = "",
    ContentTypeProvider = new FileExtensionContentTypeProvider(),
    OnPrepareResponse = ctx =>
    {
        var logger = ctx.Context.RequestServices.GetRequiredService<ILogger<StaticFileOptions>>();
        logger.LogInformation("[{Path}] Requesting a {Type} resource",
            ctx.Context.Request.Path.Value,
            ctx.Context.Request.ContentType);
        var headers = ctx.Context.Response.GetTypedHeaders();
        headers.CacheControl = new CacheControlHeaderValue
        {
            MaxAge = TimeSpan.FromDays(30),
            Public = true
        };
        headers.Expires = DateTimeOffset.Now.AddDays(30);
    }
});

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
