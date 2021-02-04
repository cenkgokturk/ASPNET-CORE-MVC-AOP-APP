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
    public class UserProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Profile()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Profile(UserProfile ur)
        {
            String connection = "Data Source=DESKTOP-II1M7LK;Initial Catalog=AccountDb;Integrated Security=True";
            using (SqlConnection sqlconn = new SqlConnection(connection))
            {
                string sqlquery = "select '" + ur.Username + "', '" + ur.Usermail + "' from AccountInfo where Usermail = cenkgokturk06@gmail.com'" ;
                using (SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn))
                {
                    sqlconn.Open();
                    SqlDataReader reader = sqlcomm.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine("{0},", reader.GetString(0));
                            ViewData["User-name"] = reader.GetString(0);
                            ViewData["User-mail"] = reader.GetString(1);
                        }
                    }
                    else
                    {
                        ViewData["Message"] = "No user with this email address has been found";
                        return View();
                    }
                    reader.Close();
                }
            }
            return View(ur);
        }
    }
}
