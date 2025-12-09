using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Models;

using Services.Results.Base;

namespace Services.Results.Custom
{
    public class CommentPagedResult : PagedResult<Comment>
    {
        public Dictionary<long,int> ReplyCounts { get; set; } = new Dictionary<long,int>();
    }
}
