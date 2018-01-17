using System;
using System.Data.SqlClient;
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
    public sealed class AllocatorGetValProvider : IAllocatorGetValProvider
    {
        private DbContextFactory _dbFactory = null;
        private string _key = String.Empty; //紀錄Key Name
        private Int64 _minHiVal = 0;
        private Int64 _maxHiVal = 0;

        private static Int64 INTERVAL = 10; //minHi~maxHi
        private static object block = new object();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <remarks></remarks>
        public AllocatorGetValProvider(DbContextFactory dbFactory)
        {
            if (dbFactory != null)
                this._dbFactory = dbFactory;
        }


        /// <summary>
        /// 取號
        /// </summary>
        /// <returns></returns>
        public Int64 GetNextVal(String key)
        {
            lock (block)
            {
                if (!key.Equals(this._key))
                {
                    //當Singleton被重新建立時(例如AP重啟)，強制跳號
                    this.setMinMaxHi(key: key, isForceReset: true);
                    this._key = key;
                }
                else
                {
                    if (this._minHiVal < this._maxHiVal)
                    {
                        this._minHiVal++;
                    }
                    else
                    {
                        this.setMinMaxHi(key: key, isForceReset: true);
                    }
                }

                return this._minHiVal;
            }
        }
        /// <summary>
        /// 取得NEXT HI
        /// </summary>
        private void setMinMaxHi(string key, bool isForceReset=false)
        {
            try
            {
                //設定 TransactionScope的 Option
                TransactionOptions transOptions = new TransactionOptions()
                {
                    IsolationLevel = System.Transactions.IsolationLevel.Serializable,
                    Timeout = new TimeSpan(0, 0, 1) //timeout : 1 min
                };

                using (var dbContext = this._dbFactory.CreateDbContext())
                using (var dbContextTransaction = dbContext.Database.BeginTransaction())
                using (var hlService = new HiLoService<DAL.Models.HiLo>(dbContext))
                {
                    Int64 dbNextHi = 0;
                    Int64 dbMaxVal = 0;

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
                    if (isForceReset || (this._minHiVal + 1) > dbMaxVal)
                    {
                        //重新設定新Range
                        this._minHiVal = dbNextHi + INTERVAL;
                        this._maxHiVal = dbMaxVal + INTERVAL;

                        hilo.NextHi = this._minHiVal;
                        hilo.MaxValue = this._maxHiVal;
                        hlService.Update(hilo);
                    }
                    else
                    {
                        this._maxHiVal = dbMaxVal;
                    }
                    #endregion

                    dbContextTransaction.Commit();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public void Dispose()
        {
            if (this._dbFactory != null)
            {
                this._dbFactory.Dispose();
            }
        }

    }
}