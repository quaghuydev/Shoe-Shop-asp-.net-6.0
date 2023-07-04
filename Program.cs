using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShoeShop.Dao;
using ShoeShop.Helper;
using ShoeShop.Interfaces;
using ShoeShop.Models;
using ShoeShop.Repository;
using ShoeShop.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IContactRepository, ContactRepository>();
builder.Services.AddScoped<IPhotoService, PhotoService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.Configure<CloudinarySetting>(builder.Configuration.GetSection("cloudinarySetting"));
builder.Services.AddDbContext<ApplicationDbContext>(
    option =>
    {
        option.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"));
    });
builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
builder.Services.AddScoped<IEmailService, EmailService>();
var app = builder.Build();
app.UseSession();
if (args.Length == 1 && args[0].ToLower() == "seeddata")
{
    await Seed.SeedUsersAndRolesAsync(app);
    Seed.SeedData(app);
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

// Thứ tự này cần được thay đổi
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    // Cấu hình route cho các Areas
    endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );

    // Cấu hình route cho Area "Admin"
    endpoints.MapAreaControllerRoute("Admin", "Admin", "Admin/{controller}/{action}/{id?}");

    // Cấu hình route cho các Controller không thuộc Area nào
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"
    );
});



app.Run();
