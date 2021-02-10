using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data;
using Microsoft.Data.SqlClient;
using ASPNETAOP.Models;
using ASPNETAOP.Aspect;
using ASPNETAOP;
using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace ASPNETAOP.Controllers
{
    public class UserLoginController : Controller
    {
        private IConfiguration _configuration;
        public UserLoginController(IConfiguration Configuration) { _configuration = Configuration; }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public void SaveCookie(UserLogin ur)
        {
            String connection = _configuration.GetConnectionString("localDatabase");
            using (SqlConnection sqlconn = new SqlConnection(connection))
            {
                DateTime thisDay = DateTime.Now;
                //  30/3/2020 12:00 AM
                //0 - Logged Out & 1 - Logged in
                string sqlQuerySession = "insert into AccountSessions(Usermail, LoginDate, IsLoggedIn) values ('" + ur.Usermail + "', '" + thisDay.ToString("g") + "', 1 )";
                using (SqlCommand sqlcommCookie = new SqlCommand(sqlQuerySession, sqlconn))
                {
                    sqlconn.Open();
                    sqlcommCookie.ExecuteNonQuery();


                    Console.WriteLine("Cookie has been saved");
                }
            }
        }

        [HttpPost]
        public IActionResult Login(UserLogin ur)
        {

            Console.WriteLine("Login: " + ur.Usermail);

            String connection = _configuration.GetConnectionString("localDatabase");

            using (SqlConnection sqlconn = new SqlConnection(connection))
            {
                string sqlquery = "select Userpassword, UserID, Username  from AccountInfo where Usermail = '" + ur.Usermail + "' ";
                using (SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn))
                {
                    sqlconn.Open();
                    SqlDataReader reader = sqlcomm.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine("{0}, {1}", reader.GetString(0), ur.Userpassword);
                            if (reader.GetString(0).Equals(ur.Userpassword)){
                                Console.WriteLine("True");
                                ViewData["Message"] = "Welcome: " + ur.Usermail;

                                Models.CurrentUser.currentUser.CurrentUserInfo[0] = reader.GetInt32(1).ToString();    //UserID
                                Models.CurrentUser.currentUser.CurrentUserInfo[1] = reader.GetString(2);    //Username
                                Models.CurrentUser.currentUser.CurrentUserInfo[2] = ur.Usermail;            


                                reader.Close();
                                sqlconn.Close();
                                SaveCookie(ur);

                                ViewData["Message"] = "Successfully logged in";
                                reader.Close();
                                return RedirectToAction("Profile","UserProfile", new { ur });
                            }
                            else
                            {
                                ViewData["Message"] = "Incorrect password";
                            }
                        }
                    }
                    else
                    {
                        ViewData["Message"] = "No user with this email address has been found";
                        reader.Close();
                        return RedirectToAction("Create", "UserRegistration");
                    }
                    reader.Close();
                }

            }

            return View(ur);
        }
    }
}
