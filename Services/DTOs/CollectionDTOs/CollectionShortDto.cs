using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Models;

namespace Services.DTOs.CollectionDTOs
{
    public class CollectionShortDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public bool IsPublic { get; set; }
        public DateTime CreationTime { get; set; }
        public SystemCollectionType? SystemCollectionType { get; set; }
    }
}
