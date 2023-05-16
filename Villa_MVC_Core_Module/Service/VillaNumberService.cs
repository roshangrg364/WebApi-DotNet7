using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Villa_MVC_Core_Module;
using Villa_MVC_Core_Module.ApiRequestModel;
using Villa_MVC_Core_Module.ApiResponseModel;
using Villa_MVC_Core_Module.Base;
using Villa_MVC_Core_Module.Dto;
using Villa_MVC_Core_Module.Service;

namespace Villa_MVC_Core_Module.Service
{
    public class VillaNumberService :BaseService, VillaNumberServiceInterface
    {

        private readonly IHttpClientFactory _httpClient;
        private string _baseUrl = URLDatas.VillaNumberBaseUrl;
        public VillaNumberService(IHttpClientFactory httpClient, IHttpContextAccessor httpContextAccessor) : base(httpClient,httpContextAccessor)
        {
        }
        public async Task<ResponseModel> Create(VillaNumberCreateDto dto,string token)
        {
            var response = await SendAsync(new ApiRequest
            {
                Method = ApiType.POST,
                Data = dto,
                Url = _baseUrl,
                Token = token
            });
            return response;

        }

        public async Task<ResponseModel> Delete(int id,string token)
        {
            var response = await SendAsync(new ApiRequest
            {
                Method = ApiType.DELETE,
                Url = _baseUrl + id,
                Token = token
            });
            return response;
        }

        public async Task<ResponseModel> GetAllVillaNumbers(string token)
        {
            var response = await SendAsync(new ApiRequest
            {
                Method = ApiType.GET,
                Url = _baseUrl,
                Token = token
            });
            return response;
        }

        public async Task<ResponseModel> GetById(int id,string token)
        {
            var response = await SendAsync(new ApiRequest
            {
                Method = ApiType.GET,
                Url = _baseUrl + id,
                Token = token
            });
            return response;
        }

        public async Task<ResponseModel> Update(VillaNumberUpdateDto dto,string token)
        {
            var response = await SendAsync(new ApiRequest
            {
                Method = ApiType.PUT,
                Data = dto,
                Url = _baseUrl + dto.Id,
                Token = token
            });
            return response;

        }
    }
}
