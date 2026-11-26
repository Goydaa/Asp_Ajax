namespace MyProject
{
    public class LifecycleMiddleware
    {
        private readonly RequestDelegate _next;

        public LifecycleMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // ЭТАП 1: Запрос вошёл в middleware
            Console.WriteLine($"[Middleware] ▶ Запрос: {context.Request.Method} {context.Request.Path}");

            // Передаём управление дальше по конвейеру
            await _next(context);

            // ЭТАП 2: Ответ возвращается обратно
            Console.WriteLine($"[Middleware] ◀ Ответ: {context.Response.StatusCode}");
        }
    }
}