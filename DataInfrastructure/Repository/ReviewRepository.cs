using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataInfrastructure.Interfaces;

using Domain.Models;

namespace DataInfrastructure.Repository
{
    public class ReviewRepository : Repository<Review>, IReviewRepository
    {
        public ReviewRepository(MyDbContext myDbContext) : base(myDbContext) { }
    }
}
