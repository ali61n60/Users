using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Users.Models;

namespace Users.Controllers
{
    [Authorize]
    public class DocumentController : Controller
    {
        private IAuthorizationService authService;
        public DocumentController(IAuthorizationService auth)
        {
            authService = auth;
        }


        private ProtectedDocument[] docs = new ProtectedDocument[] {
            new ProtectedDocument { Title = "Q3 Budget", Author = "Alice",
                Editor = "Joe"},
            new ProtectedDocument { Title = "Project Plan", Author = "Bob",
                Editor = "Alice"}
        };
        public ViewResult Index() => View(docs);
       

        public async Task<IActionResult> Edit(string title)
        {
            ProtectedDocument doc = docs.FirstOrDefault(d => d.Title == title);
            bool authorized = await authService.AuthorizeAsync(User,doc, "AuthorsAndEditors");
            if (authorized)
            {
                return View("Index", doc);
            }
            else
            {
                return new ChallengeResult();
            }
        }
    }
}