using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Allocator.DAL.Models;
using Allocator.Service;
using Allocator.DAL;
using Allocator.Domain.Models;
using Microsoft.AspNetCore.Hosting;

namespace Allocator.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class AllocatorController(IWebHostEnvironment env, IAllocatorGetValProvider getValProvider) : BaseController
    {
        // GET api/hilo/keyName
        [Route("GetNext/{key}")]
        public async Task<Sequence> GetNext(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new HttpRequestException("The key should not be NULL!");
            }

            using var dbFactory = new DbContextFactory(env.EnvironmentName);
            using var allocatorMng = new AllocatorManager(dbFactory);
            var seq = allocatorMng.GetNextVal(key, getValProvider);
            _logger.Trace($"{key} Get {seq.Value}");
            return seq;
        }


        [Route("Create")]
        [HttpPost]
        public async Task<HttpResponseMessage> Create([FromBody] HiLo? hilo)
        {
            if (hilo == null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            using var dbFactory = new DbContextFactory(env.EnvironmentName);
            using var allocatorMng = new AllocatorManager(dbFactory);
            allocatorMng.CreateHiLoInstance(hilo, out _);
            return new HttpResponseMessage(HttpStatusCode.Created);
        }
    }
}
