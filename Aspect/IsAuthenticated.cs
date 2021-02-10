using Microsoft.Data.SqlClient;
using PostSharp.Aspects;
using PostSharp.Serialization;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPNETAOP.Models;


namespace ASPNETAOP.Aspect
{
   
    [PSerializable]
    public sealed class IsAuthenticatedAttribute : OnMethodBoundaryAspect
    {
        private Models.UserLogin ur;

        public override void OnEntry(MethodExecutionArgs args)
        {
            Console.WriteLine("Method Entry");

            if (Models.CurrentUser.currentUser.CurrentUserInfo[0] == null) throw new UserNotLoggedInException("You are not logged in");

            /*
            String connection = "Data Source=DESKTOP-II1M7LK;Initial Catalog=AccountDb;Integrated Security=True";
            using (SqlConnection sqlconn = new SqlConnection(connection))
            {
                string sqlquery = "SELECT I.UserID, I.Username, I.Usermail FROM AccountSessions S, AccountInfo I WHERE S.Usermail = I.Usermail AND S.IsLoggedIn = 1;";
                using (SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn))
                {
                    sqlconn.Open();
                    SqlDataReader reader = sqlcomm.ExecuteReader();
                
                    if (reader.HasRows){
                        
                        
                    }
                    else
                    {
                        throw new Exception();
                    }
                    reader.Close();
                }
            }

            */
        }
    }

    public class UserNotLoggedInException : Exception
    {
        public UserNotLoggedInException() { }

        public UserNotLoggedInException(String message) : base(message) { }
    }
}
