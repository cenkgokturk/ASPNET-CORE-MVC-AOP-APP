using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPNETAOP.Aspect;
using Microsoft.Data.SqlClient;
using ASPNETAOP.Models;
using System.Runtime.InteropServices;

namespace ASPNETAOP.Controllers
{
    [Guid("45EADA4A-CFB8-46A9-8DDB-5A1ACCC89D2A")]
    public class AdminPageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [IsAuthenticated]
        [IsAuthorized]
        public IActionResult UserList()
        {

            String connection = "Data Source=DESKTOP-II1M7LK;Initial Catalog=AccountDb;Integrated Security=True";

            var model = new List<AdminPage>();
            using (SqlConnection sqlconn = new SqlConnection(connection))
            {
                string sqlquery = "select Username, Usermail from AccountInfo";
                using (SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn))
                {
                    sqlconn.Open();
                    SqlDataReader reader = sqlcomm.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var user = new AdminPage();
                            user.Username = (string)reader["Username"];
                            user.Usermail = (string)reader["Usermail"];

                            model.Add(user);
                        }
                        reader.Close();
                    }
                }
            }

            return View(model);
        }
    }
}
