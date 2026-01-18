using DataLogicLayer.Data;
using DataLogicLayer.Interface;
using Microsoft.EntityFrameworkCore;
using ModelLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLogicLayer.Repository
{
    public class UserDL : IUserDL
    {
        private readonly ApplicationDbContext context;

        public UserDL(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<User> CreateAsync(User user)
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();
            return user;
        }
        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await context.Users.FirstOrDefaultAsync(e=>e.Email.ToLower()==email.ToLower());
        }
        public async Task<bool> EmailExistsAsync(string email,int ? excludeUserId=null)
        {
            var userEmail = context.Users.Where(e => e.Email.ToLower() == email.ToLower());
            if (excludeUserId.HasValue)
            {
                userEmail=userEmail.Where(u=>u.UserId != excludeUserId.Value);
            }
            return await userEmail.AnyAsync();
        }
        public async Task<User> GetUserByIdAsync(int UserId)
        {
            return await context.Users.FirstOrDefaultAsync(e => e.UserId == UserId);
        }
        public async Task<User> UpdateUserByAsync(User user)
        {
            var existuser = await context.Users.FindAsync(user.UserId);
            if (existuser == null) return null;
            context.Users.Update(user);   
            await context.SaveChangesAsync();
            return user;
        }
    }
}
