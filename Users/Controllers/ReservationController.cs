using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiControllers.Controllers
{
    [Route("api/[controller]")]
    public class ReservationController : Controller
    {
       [HttpGet("SayHello")]
       //[Authorize]
        public string SayHello()
        {
            return String.Format("Hello World From Ali {0}", DateTime.Now.ToString());
        }
    }
}
