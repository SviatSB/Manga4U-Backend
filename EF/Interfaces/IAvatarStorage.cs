using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.Interfaces
{
    public interface IAvatarStorage
    {
        Task<string> UploadAsync(Stream fileStream, string fileName, string contentType);
    }
}
