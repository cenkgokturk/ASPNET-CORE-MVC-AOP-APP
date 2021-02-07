using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAOP.Controllers
{
    public class UserLogoutController : Controller
    {
        public IActionResult Logout()
        {
            String connection = "Data Source=DESKTOP-II1M7LK;Initial Catalog=AccountDb;Integrated Security=True";
            using (SqlConnection sqlconn = new SqlConnection(connection))
            {
                string sqlquery = "UPDATE AccountSessions SET IsLoggedIn = 0 WHERE IsLoggedIn = 1;";
                using (SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn))
                {
                    sqlconn.Open();
                    sqlcomm.ExecuteNonQuery();
                }
            }
            return RedirectToAction("Login", "UserLogin");
        }
    }
}
