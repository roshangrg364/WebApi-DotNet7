using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Villa_MVC_Core_Module
{
    public static class URLDatas
    {
        private static string _currentVersion = "v1";
        public static string BaseUrl { get; set; } = "https://localhost:44307/";  // please replace by the port used by villa api in case it dont match
        public static string VillaBaseUrl { get; set; } =BaseUrl +$"api/{_currentVersion}/villaApi/";
        public static string VillaNumberBaseUrl { get; set; } =BaseUrl + $"api/{_currentVersion}/VillaNumberApi/";
        public static string AccountApiBaseUrl { get; set; } =BaseUrl + $"api/{_currentVersion}/AccountApi/";
    }
}
