using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Text;
using Villa_MVC_Core_Module.ApiRequestModel;
using Villa_MVC_Core_Module.ApiResponseModel;
using Villa_MVC_Core_Module.Dto;

namespace Villa_MVC_Core_Module.Base
{
    public class BaseService : BaseServiceInterface
    {
        public ResponseModel ApiResponse { get; set; }
        private readonly IHttpClientFactory httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BaseService(IHttpClientFactory httpClient,
            IHttpContextAccessor httpContextAccessor)
        {
            ApiResponse = new();
            this.httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseModel> SendAsync(ApiRequest request)
        {
            try
            {
                var client = httpClient.CreateClient("VillaApi");
                var message = GenerateRequestMessage(request, client, request.Token);
                var apiResponse = await client.SendAsync(message);
                var apiContent = await apiResponse.Content.ReadAsStringAsync();
                var returnModel = JsonConvert.DeserializeObject<ResponseModel>(apiContent);
                if (returnModel.IsTokenExpired)
                {
                    //get token and refresh token from session
                    byte[] acessToken, refreshToken;
                    GetTokenAndRefreshTokenFromSession(out acessToken, out refreshToken);

                    HttpRequestMessage newMessage = RefreshTokenRequestMessage(client, acessToken, refreshToken);
                    var newTokenResponse = await client.SendAsync(newMessage);
                    var newContent = await newTokenResponse.Content.ReadAsStringAsync();
                    var newModel = JsonConvert.DeserializeObject<ResponseModel>(newContent);
                    var newTokenData = JsonConvert.DeserializeObject<TokenModel>(newModel.Data.ToString());

                    SetTokenAndRefreshTokenToSession(newTokenData);

                    HttpRequestMessage reSendingMessage = GenerateRequestMessage(request, client, newTokenData.AccessToken);
                    apiResponse = await client.SendAsync(reSendingMessage);
                    apiContent = await apiResponse.Content.ReadAsStringAsync();
                    returnModel = JsonConvert.DeserializeObject<ResponseModel>(apiContent);
                }
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

        private void SetTokenAndRefreshTokenToSession(TokenModel? newTokenData)
        {
            _httpContextAccessor.HttpContext.Session.Set(SessionModel.Token, Encoding.ASCII.GetBytes(newTokenData.AccessToken));
            _httpContextAccessor.HttpContext.Session.Set(SessionModel.RefreshToken, Encoding.ASCII.GetBytes(newTokenData.RefreshToken));
        }

        private void GetTokenAndRefreshTokenFromSession(out byte[] acessToken, out byte[] refreshToken)
        {
            _httpContextAccessor.HttpContext.Session.TryGetValue(SessionModel.Token, out acessToken);

            _httpContextAccessor.HttpContext.Session.TryGetValue(SessionModel.RefreshToken, out refreshToken);
        }

        private static HttpRequestMessage GenerateRequestMessage(ApiRequest request, HttpClient client, string token)
        {
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

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            return message;
        }

        private static HttpRequestMessage RefreshTokenRequestMessage(HttpClient client, byte[] acessToken, byte[] refreshToken)
        {
            var tokenValue = Encoding.ASCII.GetString(acessToken, 0, acessToken.Length);
            var refreshTokenValue = Encoding.ASCII.GetString(refreshToken, 0, refreshToken.Length);
            var message = new HttpRequestMessage();
            message.RequestUri = new Uri(URLDatas.AccountApiBaseUrl + "Refresh-Token");
            message.Headers.Add("Accept", "application/json");
            message.Method = HttpMethod.Post;
            message.Content = new StringContent(JsonConvert.SerializeObject(new TokenModel
            {
                RefreshToken = refreshTokenValue,
                AccessToken = tokenValue
            }), Encoding.UTF8, "application/json");
            client.DefaultRequestHeaders.Authorization = null;
            return message;
        }
    }
}
