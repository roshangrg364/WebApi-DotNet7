using CoreModule.Base;
using CoreModule.DbContextConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreModule.Src
{
    public class VillaRepository:BaseRepository<Villa>, VillaRepositoryInterface
    {
        public VillaRepository(ApplicationDbContext context):base(context)
        {
            
        }
    }
}
