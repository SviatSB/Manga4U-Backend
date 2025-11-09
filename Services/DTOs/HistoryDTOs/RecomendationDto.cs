using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOs.HistoryDTOs
{
    public class RecomendationDto
    {
        public string? Genre { get; set; }
        public string? GenreId {get;set;}
        public int Number {get;set;}
    }
}
