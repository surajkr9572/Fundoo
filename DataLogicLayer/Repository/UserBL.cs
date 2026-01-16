using BessinessLogicLayer.Interface;
using DataLogicLayer.Data;
using Microsoft.EntityFrameworkCore;
using ModelLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BessinessLogicLayer.Repository
{
    public class UserBL : IUserBL
    {
        private readonly ApplicationDbContext context;

        public UserBL(ApplicationDbContext context)
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
    }
}
