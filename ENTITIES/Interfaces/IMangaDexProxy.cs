using ENTITIES.Results;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.Interfaces
{
    public interface IMangaDexProxy
    {
        Task<ProxyResult> GetAsync(string path, IQueryCollection query);
    }
}
