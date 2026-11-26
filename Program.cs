using MyProject; // Важно! Подключаем наш Middleware

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

// --- ЖИЗНЕННЫЙ ЦИКЛ НАЧИНАЕТСЯ ЗДЕСЬ ---
// 1. Наш Middleware стоит ДО маршрутизации, чтобы перехватывать все запросы
app.UseMiddleware<LifecycleMiddleware>();

// 2. Маршрутизация (определяет, какой контроллер вызвать)
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

// 3. Маршрут по умолчанию
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();