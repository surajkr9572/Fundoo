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
        public async Task<IActionResult> Login([FromBody] UserRequestDto userDto)
        {
            try
            {
                var result = await _userBL.LoginUserAsync(userDto.Email, userDto.Password);
                return Ok(new
                {
                    token = result.token,
                    userId = result.UserId,
                    email = result.Email
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid login attempt");
                return Unauthorized(new { message = ex.Message });
            }
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
        public async Task<IActionResult> ForgotPassword([FromBody] UserRequestDto userDto)
        {
            try
            {
                await _userBL.ForgetPasswordAsync(userDto.Email, userDto.Password);
                return Ok(new { message = "Password updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in forgot password");
                return BadRequest(new { message = ex.Message });
            }
        }

        // Change password
        [HttpPost("change-password/{userId}")]
        public async Task<IActionResult> ChangePassword(int userId, [FromBody] ChangePasswordDto dto)
        {
            try
            {
                await _userBL.ChangePasswordAsync(userId, dto.OldPassword, dto.NewPassword);
                return Ok(new { message = "Password changed successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in change password");
                return BadRequest(new { message = ex.Message });
            }
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
