using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAOP.Controllers
{
    public class UserSessionController : Controller
    {
        private IConfiguration _configuration;
        public UserSessionController(IConfiguration Configuration) { _configuration = Configuration; }

        public UserSessionController() { }

        public IActionResult GetCurrentSessionID(){
            //Necessary to prevent sessionID from changing with every request
            HttpContext.Session.Set("What", new byte[] { 1, 2, 3, 4, 5 });

            return Content(HttpContext.Session.Id);
        }
    }
}
