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
            String connection = "Data Source=DESKTOP-II1M7LK;Initial Catalog=AccountDb;Integrated Security=True";

            Console.WriteLine("Start of profile");
            using (SqlConnection sqlconn = new SqlConnection(connection))
            {
                string sqlquery = "select Username, Usermail from AccountInfo where Usermail = 'admin@admin.com'";
                using (SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn))
                {
                    sqlconn.Open();
                    SqlDataReader reader = sqlcomm.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine("{0} is the profile section", reader.GetString(0));
                            ViewData["message"] = "User name: " + reader.GetString(0) + "\r\n Mail: " + reader.GetString(1) ;
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
            return View();
        }

        [HttpPost]
        public IActionResult Profile(UserProfile ur)
        {
            String connection = "Data Source=DESKTOP-II1M7LK;Initial Catalog=AccountDb;Integrated Security=True";

            Console.WriteLine("Start of profile");
            using (SqlConnection sqlconn = new SqlConnection(connection))
            {
                string sqlquery = "select Username, Usermail from AccountInfo where Usermail = '" + ur.Usermail + "' ";
                using (SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn))
                {
                    sqlconn.Open();
                    SqlDataReader reader = sqlcomm.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine("{0} is the profile section", reader.GetString(0));
                            ViewData["message"] = reader.GetString(0) + ", " + reader.GetString(1);
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
