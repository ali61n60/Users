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
       [Authorize]
        public JsonResult SayHello()
       {
           return Json(String.Format("Hello World From Ali {0}", DateTime.Now.ToString()));
       }
    }
}
