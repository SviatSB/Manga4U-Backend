using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Models;

using Services.DTOs.MangaDTOs;

namespace Services.DTOs.CollectionDTOs
{
    public class CollectionFullDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public bool IsPublic { get; set; }
        public DateTime CreationTime { get; set; }
        public SystemCollectionType? SystemCollectionType { get; set; }
        public long UserId { get; set; }
        public ICollection<MangaShortDto> Mangas { get; set; } = new List<MangaShortDto>();
    }
}
