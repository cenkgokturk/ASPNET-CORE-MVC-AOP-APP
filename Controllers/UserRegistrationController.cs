﻿using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.Data.SqlClient;
using ASPNETAOP.Models;
using Microsoft.Extensions.Configuration;

namespace ASPNETAOP.Controllers
{
    public class UserRegistrationController : Controller
    {
        private IConfiguration _configuration;
        public UserRegistrationController(IConfiguration Configuration) { _configuration = Configuration; }

        [Route("Home/Index")]
        public IActionResult Index()
        {
            //Necessary to prevent sessionID from changing with every request
            HttpContext.Session.Set("What", new byte[] { 1, 2, 3, 4, 5 });
            return View();
        }

        public IActionResult Create()
        {
            //Necessary to prevent sessionID from changing with every request
            HttpContext.Session.Set("What", new byte[] { 1, 2, 3, 4, 5 });
            return View();
        }

        private int GetUsedID(String Usermail)
        {
            Console.WriteLine("Get UserID Entry");
            int UserID = -1;

            String connection = _configuration.GetConnectionString("localDatabase");
            using (SqlConnection sqlconn = new SqlConnection(connection))
            {
                string sqlquery = "select UserID  from AccountInfo where Usermail = '" + Usermail + "' ";
                using (SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn))
                {
                    sqlconn.Open();
                    SqlDataReader reader = sqlcomm.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            UserID = reader.GetInt32(0);
                        }
                    }

                    sqlconn.Close();
                }
            }

            Console.WriteLine("Get UserID Close");

            return UserID;
        }

        private void AddUserRole(int UserID)
        {
            String connection = _configuration.GetConnectionString("localDatabase");
            using (SqlConnection sqlconn = new SqlConnection(connection))
            {
                string sqlquery = "insert into UserRoles(UserID, Roleid) values ('" + UserID + "', 2)";
                using (SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn))
                {
                    sqlconn.Open();
                    sqlcomm.ExecuteNonQuery();
                }
            }

            Console.WriteLine("UserROle added");
        }

        [HttpPost]
        public IActionResult Create(UserRegister ur)
        {
            //Necessary to prevent sessionID from changing with every request
            HttpContext.Session.Set("What", new byte[] { 1, 2, 3, 4, 5 });

            //Add a new user to the database
            String connection = _configuration.GetConnectionString("localDatabase");
            using (SqlConnection sqlconn = new SqlConnection(connection))
            {
                string sqlquery = "insert into AccountInfo(Username, Usermail, Userpassword) values ('" + ur.Username + "', '" + ur.Usermail + "', '" + ur.Userpassword + "' )";
                using (SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn))
                {
                    sqlconn.Open();
                    sqlcomm.ExecuteNonQuery();

                    ViewData["Message"] = "New User "+ur.Username+ " is saved successfully";

                    sqlconn.Close();
                }
            }

            //retrieve the UserID of the newly created user
            int UserID = GetUsedID(ur.Usermail);

            Console.WriteLine("Add UserROles");
            //define a standart permission
            AddUserRole(UserID);

            return View(ur);
        }

    }
}
