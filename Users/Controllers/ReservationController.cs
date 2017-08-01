using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Users.Models;

namespace ApiControllers.Controllers
{
    [Route("api/[controller]")]
    public class ReservationController : Controller
    {
        private string userId;

        public ReservationController(IHttpContextAccessor httpContextAccessor)
        {
            userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
        

        [HttpGet("SayHello")]
       [Authorize]
        public JsonResult SayHello()
       {
           
          return Json(String.Format("Hello {0} {1}",userId, DateTime.Now.ToString()));
       }
    }
}
