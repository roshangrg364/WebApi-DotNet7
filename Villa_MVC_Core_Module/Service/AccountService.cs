using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Villa_MVC_Core_Module.ApiRequestModel;
using Villa_MVC_Core_Module.ApiResponseModel;
using Villa_MVC_Core_Module.Base;
using Villa_MVC_Core_Module.Dto;

namespace Villa_MVC_Core_Module.Service
{
    public class AccountService : BaseService, AccountServiceInterface
    {
        private readonly IHttpClientFactory _httpClient;
        private string _baseUrl = URLDatas.AccountApiBaseUrl;
        public AccountService(IHttpClientFactory httpClient) : base(httpClient)
        {
        }
        public async Task<ResponseModel> Create(UserCreateDto dto)
        {
            var response = await SendAsync(new ApiRequest
            {
                Method = ApiType.POST,
                Data = dto,
                Url = _baseUrl +"Register"
            });
            return response;

        }

        public async Task<ResponseModel> Login(LoginDto dto)
        {
            var response = await SendAsync(new ApiRequest
            {
                Method = ApiType.POST,
                Data = dto,
                Url = _baseUrl +"Login"
            });
            return response;
        }
    }
}
