using BessinessLogicLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Dto;
using ModelLayer.DTOs;
using System.Security.Claims;

namespace FunDooAPP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserBL _userBL;
        private readonly ILogger<UsersController> _logger;
        private readonly IEmailSender _emailSender;

        public UsersController(IUserBL userBL, ILogger<UsersController> logger,IEmailSender emailsender)
        {
            _userBL = userBL;
            _logger = logger;
            _emailSender = emailsender;
        }

        // Register user
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRequestDto userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _userBL.RegisterUserAsync(userDto);
                await _emailSender.SendEmailAsync(result.Email, "Registration Successful", $"<h3>Hello {result.Email},</h3><p>You have registered successfully.</p>");
                return Created("", new
                {
                    userId = result.UserId,
                    email = result.Email,
                    message = "User registered successfully"
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Email already exists");
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registering user");
                return StatusCode(500, new { message = "An error occurred while creating user" });
            }
        }

        // Login user
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userBL.LoginUserAsync(userDto);

            return Ok(new
            {
                token=result.Token,
                userId = result.UserId,
                email = result.Email
            });
        }
        // Update user
        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser(int userId, [FromBody] UserRequestDto userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _userBL.UpdateUserAsync(userId, userDto);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"User {userId} not found");
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, $"Email conflict for user {userId}");
                return Conflict(new { message = ex.Message });
            }
        }

        // Forgot password
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {

            await _userBL.ForgetPasswordAsync(dto.Email);
            return Ok(new { message = "Email Send Successfully" });
        }

        // Change password
        [Authorize]
        [HttpPost("change-password/{userId}")]
        public async Task<IActionResult> ChangePassword(int userId, [FromBody] ChangePasswordDto dto)
        {
            await _userBL.ChangePasswordAsync(userId, dto.OldPassword, dto.NewPassword);
            return Ok(new { message = "Password changed successfully" });
        }
        //Reset password
        [Authorize]
        [HttpPost("Reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            if (dto.NewPassword != dto.confirmPassword)
            {
                return BadRequest(new {message="Password do not match"});
            }
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (userId == null) return BadRequest(new { message = "User not found" });
            await _userBL.ResetPasswordAsync(userId,dto.NewPassword);
            return Ok(new { message = "Reset Password Successfully" });
        }
        // Get user by ID
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(int userId)
        {
            try
            {
                var result = await _userBL.GetUserByIdAsync(userId);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"User {userId} not found");
                return NotFound(new { message = ex.Message });
            }
        }

       
    }
}
