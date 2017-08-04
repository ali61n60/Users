using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;


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

        [HttpPost]
        [Authorize]
        public IActionResult SayHelloWithParameter([FromBody] MethodParam methodParam)
        {
            
            if (!ModelState.IsValid)
                return BadRequest();
            return Json(String.Format("Hello {0}. your phone is {1}", methodParam.Name,methodParam.Phone));
        }


    }

    
}
