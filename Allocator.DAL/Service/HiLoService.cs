using Allocator.DAL.Models;

namespace Allocator.DAL.Service
{
     public class HiLoService<T> : BaseDalService<T> where T : HiLo
    {

        public HiLoService(AllocatorDbContext dbContext) : base(dbContext)
        {
            
        }
    }
}