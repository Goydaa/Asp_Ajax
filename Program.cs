using MyProject;
using Microsoft.OpenApi; // или Models, в зависимости от того, как у тебя сейчас
using Microsoft.AspNetCore.Authentication.Cookies; // <--- ДОБАВИТЬ ЭТО
using System.Security.Claims; // <--- ДОБАВИТЬ ЭТО

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// --- ДОБАВЛЯЕМ АУТЕНТИФИКАЦИЮ ЧЕРЕЗ КУКИ ---
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = ".MyProject.Auth";
        options.Cookie.HttpOnly = true;
        options.LoginPath = "/Auth/Login";       // Куда перекидывать неавторизованных
        options.AccessDeniedPath = "/Auth/AccessDenied"; // Куда перекидывать, если нет прав
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    });
// --------------------------------------------

builder.Services.AddAuthorization();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyProject API", Version = "v1" });
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyProject API v1");
});

app.UseMiddleware<LifecycleMiddleware>();
app.UseRouting();

// --- ВАЖНО: Authentication всегда ДО Authorization ---
app.UseAuthentication(); // <--- ДОБАВИТЬ ЭТО
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();