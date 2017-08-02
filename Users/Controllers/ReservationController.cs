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
    [Route("api/[controller]/[action]")]
    public class ReservationController : Controller
    {
        private string userId;

        public ReservationController(IHttpContextAccessor httpContextAccessor)
        {
            userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }


        [HttpGet]
        [Authorize]
        public JsonResult SayHello()
        {

            return Json(String.Format("Hello {0} {1}", userId, DateTime.Now.ToString()));
        }

        [HttpGet]
        [Authorize]
        public JsonResult SayHelloWithParameter(MethodParam methodParam)
        {

            return Json(String.Format("Hello {0}. your phone is {1}", methodParam.Name,methodParam.Phone));
        }


    }

    public class MethodParam
    {
        public string Name { get; set; }
        public string Phone { get; set; }
    }
}
