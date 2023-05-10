using CoreModule.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CoreModule.Src
{
    public class UserService : UserServiceInterface
    {
        private readonly UnitOfWorkServiceInterface _unitOfWork;
        private readonly UserManager<User> _userManager;

        public UserService(UnitOfWorkServiceInterface unitOfWork,UserManager<User>userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public async Task<UserResponseDto> Create(UserCreateDto dto)
        {
            using (var tx = await _unitOfWork.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
            {
                var isUserValid = await IsValidUser(dto.UserName);
                if (!isUserValid) throw new CustomException("Username already exists");
                var user = new User
                {
                    UserName = dto.UserName,
                   FullName = dto.Name
                };
               var response = await _userManager.CreateAsync(user,dto.Password);
                if (!response.Succeeded) throw new CustomException(string.Join("</br>",response.Errors.SelectMany(a=>a.Description).ToList()));
                await _userManager.AddToRoleAsync(user, User.RoleAdmin);
                await _unitOfWork.CompleteAsync();
                tx.Commit();
                var returnDto = new UserResponseDto {
                    Id = user.Id,
                    UserName = user.UserName,
                    Name = user.FullName,
                    Role =(await _userManager.GetRolesAsync(user).ConfigureAwait(false)).FirstOrDefault()
                };
               return returnDto;
            }
        }

        public async Task<User> GetByuserName(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return null;
            return user;
        }

        public async Task<bool> IsValidUser(string username)
        {
            var user = await _userManager.FindByNameAsync(username).ConfigureAwait(false);
          
            if (user == null)
            {
                return true;
            }
            return false;
        }
    }
}
