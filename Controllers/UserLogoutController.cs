using ASPNETAOP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ASPNETAOP.Controllers
{
    public class UserLogoutController : Controller
    {
        private IConfiguration _configuration;
        public UserLogoutController(IConfiguration Configuration) { _configuration = Configuration; }


        public IActionResult Logout()
        {
            HttpContext.Session.Set("What", new byte[] { 1, 2, 3, 4, 5 });
            //Change IsLoggedIn to 0 in AccountSessions table
            String connection = _configuration.GetConnectionString("localDatabase");
            using (SqlConnection sqlconn = new SqlConnection(connection))
            {
                string sqlquery = "UPDATE AccountSessions SET IsLoggedIn = 0 WHERE IsLoggedIn = 1;";
                using (SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn))
                {
                    sqlconn.Open();
                    sqlcomm.ExecuteNonQuery();
                }
            }
            
            //remove the records of the currently logged in user from the global currentUserInfo array
            for(int i=0; i<3; i++)
            {
                Models.CurrentUser.currentUser.CurrentUserInfo[i] = null;
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("ttps://localhost:44316/api/");


                foreach (Pair pair in SessionList.listObject.Pair)
                {
                    if (HttpContext.Session.Id.Equals(pair.getSessionID()))
                    {
                        var deleteTask = client.DeleteAsync("SessionItems/" + pair.getRequestID());
                        deleteTask.Wait();

                        var result = deleteTask.Result;

                        if (!result.IsSuccessStatusCode) {
                           
                        }
                    }
                }


            }

            return RedirectToAction("Login", "UserLogin");
        }
    }
}
