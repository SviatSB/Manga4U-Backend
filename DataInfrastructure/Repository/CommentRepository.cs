using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataInfrastructure.Interfaces;

using Domain.Models;

namespace DataInfrastructure.Repository
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        public CommentRepository(MyDbContext myDbContext) : base(myDbContext) { }
    }
}
