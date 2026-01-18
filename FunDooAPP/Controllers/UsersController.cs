using BessinessLogicLayer.Interface;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Dto;
using ModelLayer.DTOs;

namespace FunDooAPP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserBL _userBL;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserBL userBL, ILogger<UsersController> logger)
        {
            _userBL = userBL;
            _logger = logger;
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
            await _userBL.ForgetPasswordAsync(dto.Email, dto.NewPassword);
            return Ok(new { message = "Password updated successfully" });
        }

        // Change password
        [HttpPost("change-password/{userId}")]
        public async Task<IActionResult> ChangePassword(int userId, [FromBody] ChangePasswordDto dto)
        {
            await _userBL.ChangePasswordAsync(userId, dto.OldPassword, dto.NewPassword);
            return Ok(new { message = "Password changed successfully" });
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
