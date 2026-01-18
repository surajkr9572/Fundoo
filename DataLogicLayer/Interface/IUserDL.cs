using ModelLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLogicLayer.Interface
{
    public interface IUserDL
    {
        Task<User> CreateAsync(User user);
        Task<User> GetUserByEmailAsync(string email);
        Task<bool>EmailExistsAsync(string email,int ? excludeUserId = null);
        Task<User>GetUserByIdAsync(int UserId);
        Task<User> UpdateUserByAsync(User user);
    }
}
