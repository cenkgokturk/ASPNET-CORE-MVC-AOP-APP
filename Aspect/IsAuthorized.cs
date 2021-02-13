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
                //string sqlquery = "SELECT UR.Roleid FROM AccountSessions AcS, UserRoles UR, AccountInfo AI WHERE AcS.IsLoggedIn = 1 AND AI.Usermail = Acs.Usermail AND AI.UserID = UR.UserID;";
                string sqlquery = "SELECT Roleid FROM UserRoles UR WHERE UserID = '" + Models.CurrentUser.currentUser.CurrentUserInfo[0] + "';";
                using (SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn))
                {
                    sqlconn.Open();
                    SqlDataReader reader = sqlcomm.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            if (reader.GetInt32(0) == 1) { }
                            else
                            {
                                throw new UserPermissionNotEnoughException();
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

    public class UserPermissionNotEnoughException : Exception
    {
        public UserPermissionNotEnoughException()
        {

        }

        public UserPermissionNotEnoughException(String message) : base(message)
        {

        }
    }
}
