
using Bloggie.Web.Data;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args); // Container. Sisteme giriþ yapýyoruz.

// Container'a servis eklediðimiz kýsým.
builder.Services.AddControllersWithViews();
// DbContexti builder özelliðini kullanarak çaðýrma.
builder.Services.AddDbContext<BloggieDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("BloggieDbConnectionString")));

builder.Services.AddDbContext<AuthDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("BloggieAuthDbConnectionString")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AuthDbContext>();

builder.Services.AddScoped<ITagInterface, TagRepository>();
builder.Services.AddScoped<IBlogPostRepository, BlogPostRepository>();
builder.Services.AddScoped<IImageRepository, CloudinaryImageRepository>();

//Middle-ware
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection(); // HTTPS üzerinden projeni çalýþtýrmana izin veriyor.
app.UseStaticFiles(); // Statik dosyalarý kullanmana izin veriyor

app.UseRouting(); // Routing'e izin veren kod. Sayfalar arasý geçiþlere izin veriyor.

app.UseAuthentication();
app.UseAuthorization(); // Yetkilendirme.

app.MapControllerRoute( // Rotalama sistemi.
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run(); // Programý çalýþtýr.
