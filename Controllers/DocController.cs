using Microsoft.AspNetCore.Mvc;
using MyProject.Models;

namespace MyProject.Controllers
{
    [ApiController]
    [Route("api/doc")]
    public class DocController : ControllerBase
    {
        // В реальном приложении здесь была бы БД. Для теста храним в памяти.
        private static Document _doc = new() { Text = "Исходный текст", Version = 1 };

        private static string GetETag(Document d) => $"\"v{d.Version}\"";

        [HttpGet]
        public IActionResult Get()
        {
            var etag = GetETag(_doc);
            Response.Headers.ETag = etag;

            // Если клиент прислал If-None-Match с тем же ETag — отдаем 304 (экономия трафика)
            if (Request.Headers.IfNoneMatch.ToString() == etag)
                return StatusCode(304);

            return Ok(_doc);
        }

        [HttpPut]
        public IActionResult Put([FromBody] Document incoming)
        {
            var currentETag = GetETag(_doc);
            var clientETag = Request.Headers.IfMatch.ToString();

            // ШАГ 1: проверяем, прислал ли клиент заголовок If-Match
            if (string.IsNullOrEmpty(clientETag))
            {
                return StatusCode(428, new { error = "Требуется заголовок If-Match", currentETag });
            }

            // ШАГ 2: сравниваем версии (оптимистичная блокировка)
            if (clientETag != currentETag)
            {
                return StatusCode(412, new
                {
                    error = "Документ был изменен другим клиентом",
                    currentETag,
                    yourETag = clientETag
                });
            }

            // ШАГ 3: версии совпали, обновляем документ
            _doc.Text = incoming.Text;
            _doc.Version++;

            Response.Headers.ETag = GetETag(_doc);
            return Ok(_doc);
        }
    }
}