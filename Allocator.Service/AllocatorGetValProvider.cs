using System;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Transactions;
using Allocator.DAL;
using Allocator.DAL.Models;
using Allocator.DAL.Service;
using Microsoft.EntityFrameworkCore;

namespace Allocator.Service
{
    /// <summary>
    /// Singleton provider for 
    /// </summary>
    public sealed class AllocatorGetValProvider(DbContextFactory dbFactory) : IAllocatorGetValProvider
    {
        private readonly DbContextFactory _dbFactory = dbFactory;
        private string _key = string.Empty; //紀錄Key Name
        private long _minHiVal = 0;
        private long _maxHiVal = 0;

        private const long INTERVAL = 10; //minHi~maxHi
        private static readonly object _block = new();

        /// <summary>
        /// 取號
        /// </summary>
        /// <returns></returns>
        public long GetNextVal(string key)
        {
            lock (_block)
            {
                if (!key.Equals(_key))
                {
                    //當Singleton被重新建立時(例如AP重啟)，強制跳號
                    setMinMaxHi(key: key, isForceReset: true);
                    _key = key;
                }
                else
                {
                    if (_minHiVal < _maxHiVal)
                    {
                        _minHiVal++;
                    }
                    else
                    {
                        setMinMaxHi(key: key, isForceReset: true);
                    }
                }

                return _minHiVal;
            }
        }

        /// <summary>
        /// 取得NEXT HI
        /// </summary>
        private void setMinMaxHi(string key, bool isForceReset = false)
        {
            try
            {
                //設定 TransactionScope的 Option
                TransactionOptions transOptions = new()
                {
                    IsolationLevel = System.Transactions.IsolationLevel.Serializable,
                    Timeout = new TimeSpan(0, 0, 1) //timeout : 1 min
                };

                using var dbContext = _dbFactory.CreateDbContext();
                using var dbContextTransaction = dbContext.Database.BeginTransaction();
                using var hlService = new HiLoService<DAL.Models.HiLo>(dbContext);
                
                long dbNextHi = 0;
                long dbMaxVal = 0;

                #region Get current HiLo from database
                var hilo = hlService.Get(x => x.Key.Equals(key)).FirstOrDefault();
                if (hilo != null)
                {
                    dbNextHi = hilo.NextHi;
                    dbMaxVal = hilo.MaxValue;
                }
                else
                {
                    throw new Exception("The key is not exist in HiLo master table!");
                }
                #endregion

                #region 設定Singleton可用的minHi/maxHi value
                //this.setMinMaxHiValStrategy(dbNextHi, dbMaxVal);
                if (isForceReset || (_minHiVal + 1) > dbMaxVal)
                {
                    //重新設定新Range
                    _minHiVal = dbNextHi + INTERVAL;
                    _maxHiVal = dbMaxVal + INTERVAL;

                    hilo.NextHi = _minHiVal;
                    hilo.MaxValue = _maxHiVal;
                    hlService.Update(hilo);
                }
                else
                {
                    _maxHiVal = dbMaxVal;
                }
                #endregion

                dbContextTransaction.Commit();
            }
            catch (Exception)
            {
                throw;
            }
        }


        public void Dispose()
        {
            _dbFactory.Dispose();
        }

    }
}