using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPNETAOP.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using ASPNETAOP.Aspect;

namespace ASPNETAOP.Controllers
{
    public class SessionController : Controller
    {
        private IConfiguration _configuration;

        public SessionController() { }

        public SessionController(IConfiguration Configuration) { _configuration = Configuration; }

        public SessionController sc = new SessionController();

        public IActionResult Index()
        {
            return View();
        }
        
        public String getSessionID()
        {
            return HttpContext.Session.Id;
        }
    }
}
