using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Allocator.DAL.Models;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using Allocator.Service;
using Microsoft.AspNetCore.Hosting;
using Allocator.DAL;
using Allocator.Domain.Models;

namespace Allocator.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class AllocatorController : BaseController
    {
        private readonly IHostingEnvironment _env = null;
        private readonly IAllocatorGetValProvider getValProvider = null;

        public AllocatorController(IHostingEnvironment env, IAllocatorGetValProvider getVal)
        {
            this._env = env;
            this.getValProvider = getVal;
        }  
        
        // GET api/hilo/keyName
        [Route("GetNext/{key}")]
        public async Task<Sequence> GetNext(String key)
        {
            if (String.IsNullOrEmpty(key))
            {
                throw new HttpRequestException("The key should not be NULL!");
            }
            else
            {
                using(var dbFactory = new DbContextFactory(this._env.EnvironmentName))
                using(var allocatorMng = new AllocatorManager(dbFactory))
                {
                    var seq = allocatorMng.GetNextVal(key, this.getValProvider);
                    base._logger.Trace($"{key} Get {seq.Value}");
                    return seq;
                }
            }
        }


        [Route("Create")]
        [HttpPost]
        public async Task<HttpResponseMessage> Create([FromBody]HiLo hilo)
        {
            if (hilo == null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            else
            {
                using(var dbFactory = new DbContextFactory(this._env.EnvironmentName))
                using(var allocatorMng = new AllocatorManager(dbFactory))
                {
                    bool isKeyExist = false;
                    allocatorMng.CreateHiLoInstance(hilo, out isKeyExist);
                }
                // this.CreateHiLo(hilo);
                return new HttpResponseMessage(HttpStatusCode.Created);
            }
        }
    }
}
