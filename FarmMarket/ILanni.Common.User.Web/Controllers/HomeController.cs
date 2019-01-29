using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ILanni.Common.User.Web.Models;
using Microsoft.AspNetCore.Identity;
using ILanni.Common.User.Repository;

namespace ILanni.Common.User.Web.Controllers
{
    public class HomeController : Controller
    {
        public async Task< IActionResult> Index()
        {
            
            return View();
        }

        public async Task<IActionResult> About([FromServices] UserRepository repository)
        {
            await repository.UpdateTag(1, "BBBB");

            ViewData["Message"] = "Your application description page.";

            return View();
        }

        [Microsoft.AspNetCore.Authorization.Authorize(Roles ="admin")]
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Info()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Error403()
        {
            return View();
        }
    }
}
