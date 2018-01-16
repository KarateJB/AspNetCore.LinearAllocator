using Allocator.DAL;
using Allocator.DAL.Models;
using Allocator.DAL.Service;
using System;
using System.Linq;

namespace Allocator.Service
{
    public class AllocatorManager : IDisposable
    {
        private DbContextFactory _dbFactory = null;

        public AllocatorManager(DbContextFactory dbFactory)
        {
            this._dbFactory = dbFactory;
        }

        /// <summary>
        /// Create new HiLo instance in database
        /// </summary>
        /// <param name="hl"></param>
        /// <returns></returns>
        public void CreateHiLoInstance(HiLo hl, out bool isKeyExist)
        {
            isKeyExist = false;

            using (var hlService = new HiLoService<HiLo>(this._dbFactory.CreateDbContext()))
            {
                isKeyExist = hlService.GetAll().Any(x => x.Key.Equals(hl.Key));
                if (!isKeyExist)
                {
                    hlService.Add(hl);
                }
                else
                {
                    isKeyExist = true;
                }
            }
        }

        /// <summary>
        /// 取得Next value
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public Domain.Models.Sequence GetNextVal(string key, IAllocatorGetValProvider getValProvider)
        {
            var seq = new Domain.Models.Sequence()
            {
                Key = key,
                Value = getValProvider.GetNextVal(key)
            };
            return seq;
        }

        public void Dispose()
        {
        }

    }
}
