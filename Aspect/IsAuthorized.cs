﻿using ASPNETAOP.Models;
using ASPNETAOP.Controllers;
using Microsoft.Data.SqlClient;
using PostSharp.Aspects;
using PostSharp.Serialization;
using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using ASPNETAOP.Session;

namespace ASPNETAOP.Aspect
{

    [PSerializable]
    public sealed class IsAuthorizedAttribute : OnMethodBoundaryAspect
    {
        public override void OnEntry(MethodExecutionArgs args)
        {
            String sessionID = AppHttpContext.Current.Session.Id;

            //Get the current user from WebApi
            foreach (Pair pair in SessionList.listObject.Pair)
            {
                if (sessionID.Equals(pair.getSessionID()))
                {
                    HttpClient client = new HttpClient();
                    String connectionString = "https://localhost:44316/api/SessionItems/" + pair.getRequestID();
                    Task<SessionItem> userSession = GetJsonHttpClient(connectionString, client); ;

                    //check if the current user has an admin role
                    if (userSession.Result.Roleid != 1) throw new UserPermissionNotEnoughException();
                }
            }

            /*
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
            */
        }

        private static async Task<SessionItem> GetJsonHttpClient(string uri, HttpClient httpClient)
        {
            try
            {
                return await httpClient.GetFromJsonAsync<SessionItem>(uri);
            }
            catch (HttpRequestException) // Non success
            {
                Console.WriteLine("An error occurred.");
            }
            catch (NotSupportedException) // When content type is not valid
            {
                Console.WriteLine("The content type is not supported.");
            }
            catch (JsonException) // Invalid JSON
            {
                Console.WriteLine("Invalid JSON.");
            }

            return null;
        }
    }

    public class UserPermissionNotEnoughException : Exception
    {
        public UserPermissionNotEnoughException(){}

        public UserPermissionNotEnoughException(String message) : base(message){}
    }
}
