using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Models;

using Newtonsoft.Json;

namespace Services.Respones.Tags
{
    public class TagsListResponse
    {
        [JsonProperty("data")]
        public List<TagResponse> Data { get; set; }
    }
}
