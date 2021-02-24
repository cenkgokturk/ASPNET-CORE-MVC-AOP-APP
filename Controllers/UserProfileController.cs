using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data;
using Microsoft.Data.SqlClient;
using ASPNETAOP.Models;
using ASPNETAOP.Aspect;
using System.Runtime.InteropServices;
using System.Net.Http;
using System.Text.Json;
using System.Net.Http.Json;

namespace ASPNETAOP.Controllers
{
    [Guid("18020B1D-DB0B-4600-9443-8ACA5C6CF4FE")]
    public class UserProfileController : Controller
    {
        public IActionResult Index()
        {
            HttpContext.Session.Set("What", new byte[] { 1, 2, 3, 4, 5 });
            return View();
        }

        [IsAuthenticated]
        public IActionResult Profile()
        {
            HttpContext.Session.Set("What", new byte[] { 1, 2, 3, 4, 5 });
            foreach (Pair pair in SessionList.listObject.Pair)
            {
                Console.WriteLine("ProfileController -> saved id: " + pair.getSessionID() + " &&  current id" + HttpContext.Session.Id);
                if (HttpContext.Session.Id.Equals(pair.getSessionID()))
                {
                    HttpClient client = new HttpClient();
                    Console.WriteLine("SessinObjectCOunt: " + SessionList.listObject.count + "and RequestID: " + pair.getRequestID());
                    String connectionString = "https://localhost:44316/api/SessionItems/" + pair.getRequestID();
                    Task<SessionItem> userSession = GetJsonHttpClient(connectionString, client); ;

                    ViewData["message"] = "User name: " + userSession.Result.Username + "\r\n Mail: " + userSession.Result.Usermail;
                }
            }
            return View();
        }

        [HttpPost]
        public IActionResult Profile(UserLogin ur)
        {
            HttpContext.Session.Set("What", new byte[] { 1, 2, 3, 4, 5 });
            foreach (Pair pair in SessionList.listObject.Pair)
            {
                Console.WriteLine("ProfileController -> saved id: " + pair.getSessionID() + " &&  current id " + HttpContext.Session.Id + " && cookie: ");
                if (HttpContext.Session.Id.Equals(pair.getSessionID()))
                {
                    HttpClient client = new HttpClient();
                    String connectionString = "https://localhost:44316/api/SessionItems/" + SessionList.listObject.count;
                    Task<SessionItem> userSession = GetJsonHttpClient(connectionString, client); ;

                    ViewData["message"] = "User name: " + userSession.Result.Username + "\r\n Mail: " + userSession.Result.Usermail;
                }
            }

            return View(ur);
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
}
