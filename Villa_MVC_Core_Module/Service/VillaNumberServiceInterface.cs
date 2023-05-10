using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Villa_MVC_Core_Module.ApiResponseModel;
using Villa_MVC_Core_Module.Dto;

namespace Villa_MVC_Core_Module.Service
{
    public interface VillaNumberServiceInterface
    {
        Task<ResponseModel> Create(VillaNumberCreateDto dto, string token);
        Task<ResponseModel> Update(VillaNumberUpdateDto dto, string token);
        Task<ResponseModel> Delete(int id, string token);
        Task<ResponseModel> GetById(int id, string token);
        Task<ResponseModel> GetAllVillaNumbers(string token);
    }
}
