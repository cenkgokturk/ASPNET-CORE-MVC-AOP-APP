using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data;
using Microsoft.Data.SqlClient;
using ASPNETAOP.Models;

namespace ASPNETAOP.Controllers
{
    public class UserRegistrationController : Controller
    {
        [Route("Home/Index")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(UserRegister ur)
        {
            String connection = "Data Source=DESKTOP-II1M7LK;Initial Catalog=AccountDb;Integrated Security=True";
            using(SqlConnection sqlconn = new SqlConnection(connection))
            {
                string sqlquery = "insert into AccountInfo(Username, Usermail, Userpassword) values ('" + ur.Username + "', '" + ur.Usermail + "', '" + ur.Userpassword + "' )";
                using (SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn))
                {
                    sqlconn.Open();
                    sqlcomm.ExecuteNonQuery();
                    ViewData["Message"] = "New User "+ur.Username+ " is saved successfully";
                }
            }

            return View(ur);
        }
    }
}
