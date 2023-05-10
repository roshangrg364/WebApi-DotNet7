using Villa_MVC_Core_Module.ApiRequestModel;
using Villa_MVC_Core_Module.ApiResponseModel;

namespace Villa_MVC_Core_Module.Base
{
    public interface BaseServiceInterface
    {
        ResponseModel ApiResponse { get; set; }
        Task<ResponseModel> SendAsync(ApiRequest request);
    }
}
