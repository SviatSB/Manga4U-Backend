using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace Services.Respones.Tags
{
    public class TagResponse
    {
        [JsonProperty("id")]
        public string id { get; set; }

        [JsonProperty("attributes")]
        public TagAttributes Attributes { get; set; } // Ссылка на TagAttributes

        public string tag { get => this.Attributes.Name.en; }
        // Добавьте остальные поля, такие как "type", "relationships", если они нужны
        // ...
    }

    // 1. Класс для поля "en"
    public class NameAttributes
    {
        [JsonProperty("en")]
        public string en { get; set; } // Получает значение "Oneshot", "Thriller" и т.д.
    }

    // 2. Класс для поля "attributes"
    public class TagAttributes
    {
        [JsonProperty("name")]
        public NameAttributes Name { get; set; } // Ссылка на NameAttributes
    }
}
