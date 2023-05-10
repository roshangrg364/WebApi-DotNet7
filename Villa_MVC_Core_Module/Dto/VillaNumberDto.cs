using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Villa_MVC_Core_Module.Dto
{
    public class VillaNumberCreateDto
    {
        public string VillaNo { get;  set; }
        public string? Details { get; set; }
        public int VillaId { get; set; }
    }

    public class VillaNumberUpdateDto : VillaNumberCreateDto
    {
        public int Id { get; set; }
    }

    public class VillaNumberResponseDto {
        public int Id { get; set; }
        public string VillaNo { get;  set; }
        public string Details { get; set; }
        public VillaResponseDto Villa { get; set; }
    }

}
