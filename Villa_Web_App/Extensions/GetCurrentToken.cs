
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Villa_MVC_Core_Module.Service;
using Villa_Web_App.Models;

namespace Villa_Web_App.Extensions
{
    public static class GetCurrentToken
    {

        public static string GetToken(this Controller controller)
        {
            
            var token = controller.HttpContext.User.FindFirst("Token")?.Value;
            return token;
        }

       
    }
}
