using ModelLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BessinessLogicLayer.Interface
{
    public interface IUserBL
    {
        Task<UserResponseDto>LoginUserAsync(string email,string password);
        Task<UserResponseDto> RegisterUserAsync(UserRequestDto userRequestDto);
        Task<UserResponseDto> UpdateUserAsync(int UserId,UserRequestDto userRequestDto);


        Task<bool>ChangePasswordAsync(int UserId,string oldPassword,string newPassword);
        Task<bool> ForgetPasswordAsync(string email,string newPassword);

        Task<UserResponseDto> GetUserByIdAsync(int  UserId);


    }
}
