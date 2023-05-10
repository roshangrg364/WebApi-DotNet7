using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreModule.Src
{
    public interface VillaNumberServiceInterface
    {
        Task<VillaNumberResponseDto> Create(VillaNumberCreateDto dto);
        Task<VillaNumberResponseDto> Update(VillaNumberUpdateDto dto);
        Task Delete(int id);
        Task<VillaNumberResponseDto> GetById(int id);
        Task<List<VillaNumberResponseDto>> GetAllVillas();
    }
}
