using Microsoft.Data.SqlClient;
using PostSharp.Aspects;
using PostSharp.Serialization;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAOP.Aspect
{

    [PSerializable]
    public sealed class IsAuthorizedAttribute : OnMethodBoundaryAspect
    {
        public override void OnEntry(MethodExecutionArgs args)
        {

            String connection = "Data Source=DESKTOP-II1M7LK;Initial Catalog=AccountDb;Integrated Security=True";
            using (SqlConnection sqlconn = new SqlConnection(connection))
            {
                string sqlquery = "SELECT Usermail FROM AccountSessions WHERE IsLoggedIn = 1;";
                using (SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn))
                {
                    sqlconn.Open();
                    SqlDataReader reader = sqlcomm.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            String mail = reader.GetString(0);
                            Console.WriteLine("Authorized: " + mail);
                            if (mail.Equals("admin@admin.com")) { }
                            else
                            {
                                throw new Exception("You don't have the necessary permission");
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("No user has been found");
                    }
                    reader.Close();
                }
            }

        }
    }
}
