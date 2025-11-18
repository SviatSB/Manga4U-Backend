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
        // 1. Класс для ID тега (без изменений)
        public class TagId
        {
            [JsonProperty("id")]
            public string Id { get; set; }
        }

        // 2. Обновленный класс для объекта "title"
        // Он наследует от Dictionary<string, string>, чтобы захватить все языки,
        // включая те, для которых нет явного поля (например, "ja", "ko", и т.д.)
        public class MangaTitle : Dictionary<string, string>
        {
            // Явные поля для приоритетных языков, если они нужны. 
            // Однако, наследование от Dictionary<string, string> делает их необязательными 
            // для десериализации, но полезными для понимания структуры JSON.
            // Newtonsoft.Json автоматически десериализует все ключи в словарь.

            // Если вы хотите иметь легкий доступ к приоритетным языкам как к свойствам,
            // вы можете оставить их, но это не обязательно, так как они будут в словаре.

            // [JsonProperty("en")]
            // public string En { get; set; }

            // [JsonProperty("ru")]
            // public string Ru { get; set; }

            // [JsonProperty("uk")]
            // public string Uk { get; set; }
        }

        // 3. Класс для объекта "attributes" (без изменений, так как Title обновлен)
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

            // ОБНОВЛЕННОЕ поле 1: Название манги (только для чтения)
            // Реализует логику приоритетного выбора
            public string Name
            {
                get
                {
                    // Проверяем, существует ли объект Title и содержит ли он данные
                    if (Attributes?.Title == null || !Attributes.Title.Any())
                    {
                        return "No Title Available"; // Возвращаем заглушку, если данных нет
                    }

                    var title = Attributes.Title;

                    // 1. Приоритет: Английский (en)
                    if (title.TryGetValue("en", out string enTitle) && !string.IsNullOrEmpty(enTitle))
                    {
                        return enTitle;
                    }

                    // 2. Приоритет: Украинский (uk)
                    if (title.TryGetValue("uk", out string ukTitle) && !string.IsNullOrEmpty(ukTitle))
                    {
                        return ukTitle;
                    }

                    // 3. Приоритет: Русский (ru)
                    if (title.TryGetValue("ru", out string ruTitle) && !string.IsNullOrEmpty(ruTitle))
                    {
                        return ruTitle;
                    }

                    // 4. Последний шанс: Любое первое встречное значение
                    // Берем первое значение из словаря
                    return title.First().Value;
                }
            }

            // Новое поле 2: Список ID тегов (без изменений)
            public List<string> TagIds => Attributes?.Tags?.Select(t => t.Id).ToList() ?? new List<string>();
        }

        // 5. Класс-контейнер для всего ответа (без изменений)
        public class RootResponse
        {
            [JsonProperty("data")]
            public MangaResponse Data { get; set; }
        }
    }
}