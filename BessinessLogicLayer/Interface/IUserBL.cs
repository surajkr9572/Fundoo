using ModelLayer.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BessinessLogicLayer.Interface
{
    public interface IUserBL
    {
        Task<LoginResponseDto>LoginUserAsync(LoginRequestDto loginRequestDto);
        Task<UserResponseDto> RegisterUserAsync(UserRequestDto userRequestDto);
        Task<UserResponseDto> UpdateUserAsync(int UserId,UserRequestDto userRequestDto);


        Task<bool>ChangePasswordAsync(int UserId,string oldPassword,string newPassword);
        Task<bool> ForgetPasswordAsync(string email);

        Task<UserResponseDto> GetUserByIdAsync(int  UserId);
        Task<bool> ResetPasswordAsync(int userId, string newPassword);

    }
}
