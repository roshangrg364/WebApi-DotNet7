using CoreModule.Base;
using CoreModule.DbContextConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreModule.Src
{
    public class VillaNumberRepository:BaseRepository<VillaNumber>,VillaNumberRepositoryInterface
    {
        public VillaNumberRepository(ApplicationDbContext context):base(context)
        {
            
        }
    }
}
