using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace CoreModule.Src
{
    public class Villa
    {
        protected Villa() { }
        public Villa(string name)
        {
            Name = name;
            CreatedDate = DateTime.Now;
            UpdatedDate= DateTime.Now;  
        }
        public void Update(string name)
        {
            Name    = name;
            UpdatedDate = DateTime.Now;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Details { get; set; }
        public decimal Rate { get; set; }
        public decimal? Area { get; set; }
        public string? Occupancy { get; set; }
        public string? Amenity { get; set; }
        public string? ImageUrl { get; set; }
        public  DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public ICollection<VillaNumber> VillaNumbers { get; set; } = new List<VillaNumber>();
    }
}
