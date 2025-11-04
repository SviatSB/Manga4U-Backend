using ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.Interfaces
{
    public interface IJwtTokenGenerator
    {
        public string GenerateToken(User user, IList<string> roles);
    }
}
