using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOs.ReviewDTOs
{
    public class ReviewPagedDto
    {
        public List<ReviewDto> Items { get; set; } = new List<ReviewDto>();
        public int TotalCount { get; set; }
    }
}
