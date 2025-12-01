using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataInfrastructure.Repository;

using Domain.Models;

namespace DataInfrastructure.Interfaces
{
    public interface IReviewRepository : IRepository<Review>
    {
    }
}
