using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreModule.Src
{
    public interface UserServiceInterface
    {
        Task<bool> IsValidUser(string username);
        Task<UserResponseDto> Create(UserCreateDto dto);
        Task<User> GetByuserName(string username);
        Task<string> UpdateRefreshToken(string userId);

    }
}
