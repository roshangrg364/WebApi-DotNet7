using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreModule.Src
{
    public class VillaNumber
    {
       protected VillaNumber() { }
        public VillaNumber(string villaNo,Villa villa)
        {
            Villa = villa;
            VillaNo = villaNo;
            CreatedDate = DateTime.Now;
            UpdatedDate = DateTime.Now;
        }

        public void Update(string villaNo)
        {
            VillaNo = villaNo;
            UpdatedDate = DateTime.Now;
        }
        public int Id { get; set; }
        public string VillaNo{ get;protected set; }
        public string? Details { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int VillaId { get; protected set; }
        public Villa Villa { get; protected set; }
    }
}
