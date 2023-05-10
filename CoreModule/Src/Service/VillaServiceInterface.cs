using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreModule.Src
{
    public interface VillaServiceInterface
    {
        Task<VillaResponseDto> Create(VillaCreateDto dto);
        Task<VillaResponseDto> Update(VillaUpdateDto dto);
        Task Delete(int id);
        Task<VillaResponseDto> GetById(int id);
        Task<List<VillaResponseDto>> GetAllVillas();
    }
}
