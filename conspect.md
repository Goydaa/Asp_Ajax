Конспект: Channels (Очереди) и Тонкий клиент в ASP.NET Core
1. Channels в ADO.NET / ASP.NET Core
Что это: System.Threading.Channels — это современная и очень быстрая замена BlockingCollection для организации асинхронных очередей между «производителями» (например, веб-запросами) и «потребителями» (фоновыми службами).

Основные концепции:
Bounded Channel — очередь с ограничением размера (capacity). Если она заполнена, WriteAsync будет ждать (или отбрасывать данные, в зависимости от настроек), защищая сервер от переполнения памяти (OOM).

Unbounded Channel — очередь без ограничений (опасно при большом потоке данных).

Режимы (BoundedChannelFullMode):

Wait — писатель ждет, пока читатель освободит место.

DropOldest / DropNewest — отбрасывают старые/новые элементы.

SingleReader / SingleWriter — оптимизация производительности, если читатель или писатель строго один.

Схема работы:
Интерфейс IBackgroundTaskQueue абстрагирует работу с каналом.

BackgroundTaskQueue оборачивает Channel<Func<CancellationToken, ValueTask>>. Она создается как Singleton.

BackgroundService (например, QueueConsumerService) в методе ExecuteAsync бесконечно читает задачи из канала и выполняет их.

Веб-запрос (или контроллер) вызывает QueueAsync, кладет задачу (например, отправку email) в очередь и мгновенно отвечает пользователю 200 OK.

2. Паттерн «Тонкий клиент» (Thin Client)
Что это: Слой абстракции для работы с внешним API. Вместо того чтобы разбрасывать HttpClient по всему коду, мы создаем одну обертку, которая централизованно обрабатывает заголовки, токены, JSON и ошибки.

Главные принципы:
Единый транспорт (HttpTransport):

Отвечает за базовый URL, подстановку токена (Authorization: Bearer), сериализацию тела запроса в JSON.

Настраивает JsonSerializerOptions (CamelCase).

Единый результат (ApiResult<T>):

Никаких throw new Exception при сетевых ошибках или 404!

Возвращается структура: IsSuccess (успех/провал), Data (данные), Error (объект ошибки).

Типизация ошибок (ApiError):

Ошибки делятся на категории (ApiErrorKind):

Network — нет соединения.

Timeout — сервер не ответил вовремя.

Http — сервер вернул 4xx/5xx (ошибка валидации, нет доступа).

Parse — сервер вернул кривой JSON.

Canceled — запрос отменен клиентом (не считается ошибкой UI).

Контракты API (UsersApi):

Класс, который знает конкретные пути (/users, /users/{id}) и DTO (объекты запроса/ответа). Он использует HttpTransport для отправки.

Важные нюансы работы с HttpClient:
HttpClient должен переиспользоваться! Создание нового HttpClient на каждый запрос приводит к исчерпанию сокетов (Socket Exhaustion). Он создается один раз (static readonly или через IHttpClientFactory).

Отмена (CancellationToken): Всегда пробрасывается через методы. Если клиент отвалился, запрос нужно отменить, чтобы не тратить ресурсы сервера.

Обработка таймаутов: HttpClient кидает TaskCanceledException, даже если это просто таймаут, а не ручная отмена. Нужно различать эти состояния (проверяя ct.IsCancellationRequested).

Пример использования (как это выглядит в коде):
csharp
var transport = new HttpTransport(client, baseUrl, getToken);
var usersApi = new UsersApi(transport);

var result = await usersApi.GetByIdAsync("42");

if (result.IsSuccess) 
{
    Console.WriteLine(result.Data.Name);
} 
else if (result.Error.Kind == ApiErrorKind.Http && result.StatusCode == HttpStatusCode.NotFound) 
{
    Console.WriteLine("Пользователь не найден");
}
Суть: Такой подход делает код предсказуемым (ошибки не крашат приложение, а возвращаются как данные), тестируемым и устойчивым к сбоям сети.