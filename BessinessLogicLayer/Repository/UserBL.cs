using AutoMapper;
using BessinessLogicLayer.Interface;
using DataLogicLayer.Repository;
using ModelLayer.DTOs;
using BCrypt.Net;
using ModelLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLogicLayer.Interface;
using ModelLayer.Dto;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace BessinessLogicLayer.Repository
{
    public class UserBL: IUserBL
    {
        private readonly IUserDL _userdl;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;
        private readonly IEmailSender _emailSender;

        public UserBL(IUserDL userdl, IMapper mapper, IJwtService jwtService, IEmailSender emailsender)
        {
            _userdl = userdl;
            _mapper = mapper;
            _jwtService = jwtService;
            _emailSender = emailsender;
        }

        public async Task<LoginResponseDto> LoginUserAsync(LoginRequestDto loginRequestDto)
        {
            var user = await _userdl.GetUserByEmailAsync(loginRequestDto.Email);

            if (user == null)
                throw new UnauthorizedAccessException("Invalid email or password");

            if (!BCrypt.Net.BCrypt.Verify(loginRequestDto.Password, user.Password))
                throw new UnauthorizedAccessException("Invalid email or password");
            var jwtToken = _jwtService.GenerateToken(user);

            return new LoginResponseDto
            {
                UserId = user.UserId,
                Email = user.Email,
                Token = jwtToken
            };
        }

        public async Task<UserResponseDto> RegisterUserAsync(UserRequestDto userRequestDto)
        {
            var emailExists = await _userdl.EmailExistsAsync(userRequestDto.Email);
            if (emailExists)
                throw new InvalidOperationException($"Email {userRequestDto.Email} already exists");

            var user = _mapper.Map<User>(userRequestDto);
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            user.CreatedAt = DateTime.UtcNow;
            user.ChangedAt = DateTime.UtcNow;

            var createdUser = await _userdl.CreateAsync(user);
            return _mapper.Map<UserResponseDto>(createdUser);
        }

        public async Task<UserResponseDto> UpdateUserAsync(int userId, UserRequestDto userRequestDto)
        {
            var existingUser = await _userdl.GetUserByIdAsync(userId);
            if (existingUser == null)
                throw new KeyNotFoundException($"User {userId} not found");

            if (userRequestDto.Email != existingUser.Email)
            {
                var emailExists = await _userdl.EmailExistsAsync(userRequestDto.Email, userId);
                if (emailExists)
                    throw new InvalidOperationException($"Email '{userRequestDto.Email}' is already in use");
            }

            _mapper.Map(userRequestDto, existingUser);

            // Only update password if provided
            if (!string.IsNullOrWhiteSpace(userRequestDto.Password))
            {
                existingUser.Password = BCrypt.Net.BCrypt.HashPassword(userRequestDto.Password);
            }

            existingUser.ChangedAt = DateTime.UtcNow;

            var updatedUser = await _userdl.UpdateUserByAsync(existingUser);
            return _mapper.Map<UserResponseDto>(updatedUser);
        }

        public async Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword)
        {
            var user = await _userdl.GetUserByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("User not found");

            if (!BCrypt.Net.BCrypt.Verify(oldPassword, user.Password))
                throw new UnauthorizedAccessException("Old password is incorrect");

            user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
            user.ChangedAt = DateTime.UtcNow;

            await _userdl.UpdateUserByAsync(user);
            return true;
        }

        public async Task<bool> ForgetPasswordAsync(string email)
        {
            var user = await _userdl.GetUserByEmailAsync(email);
            if (user == null)
                throw new KeyNotFoundException("User not found");

            var jwtToken = _jwtService.GenerateToken(user);
            await _emailSender.SendEmailAsync(user.Email, "Forget Password", $"<h3>Hello {user.Email} Your Generated token: ,</h3><p>{jwtToken}</p>");
            return true;
        }

        public async Task<bool> ResetPasswordAsync(int userId, string newPassword)
        {
            var user=await _userdl.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }
            user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
            user.ChangedAt = DateTime.UtcNow;
           await  _userdl.UpdateUserByAsync(user);
            return true;
        }
        public async Task<UserResponseDto> GetUserByIdAsync(int userId)
        {

            var user = await _userdl.GetUserByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException($"User {userId} not found");

            return _mapper.Map<UserResponseDto>(user);
        }
    }
}
