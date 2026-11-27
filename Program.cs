using MyProject;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// Раздача статических файлов (css, js, картинки)
app.UseStaticFiles(); 

// --- ЖИЗНЕННЫЙ ЦИКЛ НАЧИНАЕТСЯ ЗДЕСЬ ---
// 1. Наш Middleware стоит ДО маршрутизации
app.UseMiddleware<LifecycleMiddleware>();

// 2. Маршрутизация
app.UseRouting();

app.UseAuthorization();

// 3. Маршрут по умолчанию
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();