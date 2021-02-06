using Microsoft.Data.SqlClient;
using PostSharp.Aspects;
using PostSharp.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAOP.Aspect
{
    [PSerializable]
    public sealed class IsAuthenticatedAttribute : OnMethodBoundaryAspect
    {
        public override void OnEntry(MethodExecutionArgs args)
        {
            Models.UserLogin ur = (Models.UserLogin) args.Arguments[0];
            Console.WriteLine("Method Entry");
            Console.WriteLine("{0} & {1} IS THE INFO", ur.Usermail, ur.Userpassword);

            String connection = "Data Source=DESKTOP-II1M7LK;Initial Catalog=AccountDb;Integrated Security=True";
            using (SqlConnection sqlconn = new SqlConnection(connection))
            {
                string sqlquery = "select Userpassword from AccountInfo where Usermail = '" + ur.Usermail + "' ";
                using (SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn))
                {
                    sqlconn.Open();
                    SqlDataReader reader = sqlcomm.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine("{0}, {1}", reader.GetString(0), ur.Userpassword);
                            if (reader.GetString(0).Equals(ur.Userpassword))
                            {
                               //successful login

                            }
                            else
                            {
                                //incorrect password
                            }
                        }
                    }
                    else
                    {
                        //no record has been found
                    }
                    reader.Close();
                }
            }
        }
    }
}
