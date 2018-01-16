using System;
using System.Linq;

namespace Allocator.DAL
{
    public class DbContextSeedData
    {
        public void Seed()
        {
            var dbContext = (new DbContextFactory()).CreateDbContext();

#if (SEED)
            this.initHiLoMasters(dbContext as HiLoDbContext);
#endif
        }

        private void initHiLoMasters(AllocatorDbContext dbContext)
        {
            try
            {
                if (!dbContext.HiLoMasters.Any())//Only initialize when the table is empty
                {
                    //dbContext.SvYearEnds.Add(new SvYearEnd { EmpNo = 8324, IsAttend = true, Name = "JB" });
                    //dbContext.SaveChanges();
                }
            }
            catch (Exception)
            {
                //...
            }
            
        }
    }
}