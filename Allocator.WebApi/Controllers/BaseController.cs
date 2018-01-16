using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace Allocator.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class BaseController : Controller
    {
         /// <summary>
        /// NLog logger
        /// </summary>
        protected Logger _logger = LogManager.GetCurrentClassLogger();
    }
}
