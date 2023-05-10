using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreModule.Src
{
    public class VillaCreateDto
    {
      
        public string Name { get; set; }
        public string? Details { get; set; }
        public decimal Rate { get; set; }
        public decimal? Area { get; set; }
        public string? Occupancy { get; set; }
        public string? Amenity { get; set; }
        public string? ImageUrl { get; set; }
    }

    public class VillaUpdateDto : VillaCreateDto
    {

        public int Id { get; set; }
    }


    public class VillaResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Details { get; set; }
        public decimal Rate { get; set; }
        public decimal? Area { get; set; }
        public string? Occupancy { get; set; }
        public string? Amenity { get; set; }
        public string? ImageUrl { get; set; }
    }


}
