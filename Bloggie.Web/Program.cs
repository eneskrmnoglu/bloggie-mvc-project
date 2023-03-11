
var builder = WebApplication.CreateBuilder(args); // Container. Sisteme giri� yap�yoruz.

// Container'a servis ekledi�imiz k�s�m.
builder.Services.AddControllersWithViews();

//Middle-ware
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection(); // HTTPS �zerinden projeni �al��t�rmana izin veriyor.
app.UseStaticFiles(); // Statik dosyalar� kullanmana izin veriyor

app.UseRouting(); // Routing'e izin veren kod. Sayfalar aras� ge�i�lere izin veriyor.

app.UseAuthorization(); // Yetkilendirme.

app.MapControllerRoute( // Rotalama sistemi.
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run(); // Program� �al��t�r.
