using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreModule.Src
{
    
    public class UserCreateDto {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }

    public class UserResponseDto {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
    }


}
