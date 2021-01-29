using AuthServer.Core.DTOs;
using AuthServer.Core.Entity;
using AuthServer.Core.Services;
using AuthServer.Service.Mapping;
using Microsoft.AspNetCore.Identity;
using SharedLibrary.DTO;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AuthServer.Service.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        public UserService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<Response<AppUserDTO>> CreateUserAsync(CreateUserDTO user)
        {
            var userInfo = new AppUser
            {
                Email = user.Email,
                UserName = user.UserName
            };

            var result = await _userManager.CreateAsync(userInfo, user.Password);
            if(!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description).ToList();
                return Response<AppUserDTO>.Fail(new ErrorDTO(errors, true), (int)HttpStatusCode.BadRequest);
            }
            return Response<AppUserDTO>.Success(ObjectMapper.Mapper.Map<AppUserDTO>(user), (int)HttpStatusCode.OK);
        }

        public async Task<Response<AppUserDTO>> GetUserByNameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                return Response<AppUserDTO>.Fail("Username bulunamadı", (int)HttpStatusCode.NotFound, true);
            return Response<AppUserDTO>.Success(ObjectMapper.Mapper.Map<AppUserDTO>(user), (int)HttpStatusCode.OK);
        }
    }
}
