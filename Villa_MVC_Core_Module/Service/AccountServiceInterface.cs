using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Villa_MVC_Core_Module.ApiResponseModel;
using Villa_MVC_Core_Module.Dto;

namespace Villa_MVC_Core_Module.Service
{
    public interface AccountServiceInterface
    {
        Task<ResponseModel> Create(UserCreateDto dto);
        Task<ResponseModel> Login(LoginDto dto);

    }
}
