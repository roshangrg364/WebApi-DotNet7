using System.Net;

namespace Villa_MVC_Core_Module.ApiResponseModel
{
    public class ResponseModel
    {
        public HttpStatusCode Status { get; set; }
        public bool IsSuccess { get; set; }
        public object Data { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}
