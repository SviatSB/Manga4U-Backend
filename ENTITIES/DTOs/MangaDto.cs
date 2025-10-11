using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.DTOs
{
    public class MangaDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string ExternalId { get; set; } = null!;
        
        // Optional: Include related data for display purposes
        public List<string>? Genres { get; set; }
        public int ReviewsCount { get; set; }
        public double AverageRating { get; set; }
    }
}
