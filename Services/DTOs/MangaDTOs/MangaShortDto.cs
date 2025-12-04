using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Models;

namespace Services.DTOs.MangaDTOs
{
    public class MangaShortDto
    {
        public string Name { get; set; } = null!; //not null
        public string ExternalId { get; set; } = null!;
    }
}
