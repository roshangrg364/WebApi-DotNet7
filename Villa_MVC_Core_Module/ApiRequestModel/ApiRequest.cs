namespace Villa_MVC_Core_Module.ApiRequestModel
{
    public class ApiRequest
    {
        public ApiType Method { get; set; } = ApiType.GET;
        public string Url { get; set; }
        public object Data { get; set; }
        public string Token { get; set; }
    }
    public class SessionModel {
        public static string Token { get; set; } = "JWTTOKEN";
        public static string RefreshToken { get; set; } = "REFRESHTOKEN";
    }

    public enum ApiType
    {
        GET,
        POST,
        PUT,
        DELETE
    }

}
