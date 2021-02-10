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
            //Change IsLoggedIn to 0 in AccountSessions table
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
            
            //remove the records of the currently logged in user from the global currentUserInfo array
            for(int i=0; i<4; i++)
            {
                Models.CurrentUser.currentUser.CurrentUserInfo[i] = null;
            }

            return RedirectToAction("Login", "UserLogin");
        }
    }
}
