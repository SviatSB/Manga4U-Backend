using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace Services.DTOs.MangaDTOs
{
    public class MangaDexMangaDto
    {
        // Классы для отображения вложенной структуры JSON (остаются)

        // 1. Класс для ID тега (внутри массива tags)
        public class TagId
        {
            [JsonProperty("id")]
            public string Id { get; set; }
        }

        // 2. Класс для объекта "title"
        public class MangaTitle
        {
            [JsonProperty("en")]
            public string En { get; set; }
        }

        // 3. Класс для объекта "attributes"
        public class MangaAttributes
        {
            [JsonProperty("title")]
            public MangaTitle Title { get; set; }

            [JsonProperty("tags")]
            public List<TagId> Tags { get; set; }
        }

        // --- Основной класс с прямым доступом к данным ---

        // 4. Основной класс для элемента "data"
        public class MangaResponse
        {
            [JsonProperty("id")]
            public string Id { get; set; } // ID манги

            [JsonProperty("attributes")]
            public MangaAttributes Attributes { get; set; }

            // Новое поле 1: Название манги (только для чтения)
            // Используем оператор ?. для безопасного доступа к вложенным полям.
            public string Name => Attributes?.Title?.En;

            // Новое поле 2: Список ID тегов (только для чтения)
            // Используем LINQ для преобразования List<TagId> в List<string>
            public List<string> TagIds => Attributes?.Tags?.Select(t => t.Id).ToList() ?? new List<string>();
        }

        // 5. Класс-контейнер для всего ответа
        public class RootResponse
        {
            [JsonProperty("data")]
            public MangaResponse Data { get; set; }
        }
    }
}
