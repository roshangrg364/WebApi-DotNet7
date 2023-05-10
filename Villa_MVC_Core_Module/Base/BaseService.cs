using Newtonsoft.Json;
using System.Text;
using Villa_MVC_Core_Module.ApiRequestModel;
using Villa_MVC_Core_Module.ApiResponseModel;

namespace Villa_MVC_Core_Module.Base
{
    public class BaseService : BaseServiceInterface
    {
        public ResponseModel ApiResponse { get; set; }
        private readonly IHttpClientFactory httpClient;
        public BaseService(IHttpClientFactory httpClient)
        {
            ApiResponse = new();
            this.httpClient = httpClient;

        }

        public async Task<ResponseModel> SendAsync(ApiRequest request)
        {
            try
            {
                var client = httpClient.CreateClient("VillaApi");
                var message = new HttpRequestMessage();
                message.RequestUri = new Uri(request.Url);
                message.Headers.Add("Accept", "application/json");
                if (request.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(request.Data), Encoding.UTF8, "application/json");
                }
                message.Method = HttpMethod.Get;
                if (request.Method == ApiType.POST) message.Method = HttpMethod.Post;
                if (request.Method == ApiType.PUT) message.Method = HttpMethod.Put;
                if (request.Method == ApiType.DELETE) message.Method = HttpMethod.Delete;

                if (!string.IsNullOrEmpty(request.Token))
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", request.Token);
                }
                var apiResponse = await client.SendAsync(message);
                var apiContent = await apiResponse.Content.ReadAsStringAsync();
                var returnModel = JsonConvert.DeserializeObject<ResponseModel>(apiContent);
                return returnModel;
            }
            catch (Exception ex)
            {
                var responseModel = new ResponseModel()
                {
                    IsSuccess = false,
                    Errors = new List<string> { ex.Message }
                };
                return responseModel;
            }
        }
    }
}
