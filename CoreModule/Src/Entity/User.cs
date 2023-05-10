using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreModule.Src
{
    public class User:IdentityUser
    {
        public static string RoleUser = "User";
        public static string RoleAdmin = "Admin";
  
        public string FullName { get; set; }
      
    }
}
