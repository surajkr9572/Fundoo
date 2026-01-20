using BessinessLogicLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Dto;
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

        public UsersController(
            IUserBL userBL,
            ILogger<UsersController> logger,
            IEmailSender emailSender)
        {
            _userBL = userBL;
            _logger = logger;
            _emailSender = emailSender;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<UserResponseDto>>> Register(
            [FromBody] UserRequestDto userDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToArray();

                return BadRequest(new ApiResponse<UserResponseDto>(
                    false,
                    "Validation failed",
                    errors));
            }

            try
            {
                var result = await _userBL.RegisterUserAsync(userDto);

                await _emailSender.SendEmailAsync(
                    result.Email,
                    "Registration Successful",
                    $"<h3>Hello {result.Email}</h3><p>You have registered successfully.</p>");

                return Created(string.Empty,
                    new ApiResponse<UserResponseDto>(
                        true,
                        "User registered successfully",
                        result));
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Email already exists");

                return Conflict(new ApiResponse<UserResponseDto>(
                    false,
                    ex.Message,
                    ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registering user");

                return StatusCode(500, new ApiResponse<UserResponseDto>(
                    false,
                    "Internal server error",
                    "Something went wrong"));
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<LoginResponseDto>>> Login(
            [FromBody] LoginRequestDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<UserResponseDto>(
                    false,
                    "Invalid login data",
                    "Invalid email or password"));
            }

            var result = await _userBL.LoginUserAsync(userDto);

            return Ok(new ApiResponse<LoginResponseDto>(
                true,
                "Login successful",
                result));
        }


        [HttpPut("{userId}")]
        public async Task<ActionResult<ApiResponse<UserResponseDto>>> UpdateUser(
            int userId,
            [FromBody] UserRequestDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<UserResponseDto>(
                    false,
                    "Validation failed",
                    "Invalid user data"));
            }

            try
            {
                var result = await _userBL.UpdateUserAsync(userId, userDto);

                return Ok(new ApiResponse<UserResponseDto>(
                    true,
                    "User updated successfully",
                    result));
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "User not found");

                return NotFound(new ApiResponse<UserResponseDto>(
                    false,
                    ex.Message,
                    ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new ApiResponse<UserResponseDto>(
                    false,
                    ex.Message,
                    ex.Message));
            }
        }


        [HttpPost("forgot-password")]
        public async Task<ActionResult<ApiResponse<object>>> ForgotPassword(
            [FromBody] ForgotPasswordDto dto)
        {
            await _userBL.ForgetPasswordAsync(dto.Email);

            return Ok(new ApiResponse<object>(
                true,
                "Password reset email sent successfully",
                null));
        }


        [Authorize]
        [HttpPost("change-password/{userId}")]
        public async Task<ActionResult<ApiResponse<object>>> ChangePassword(
            int userId,
            [FromBody] ChangePasswordDto dto)
        {
            await _userBL.ChangePasswordAsync(
                userId,
                dto.OldPassword,
                dto.NewPassword);

            return Ok(new ApiResponse<object>(
                true,
                "Password changed successfully",
                null));
        }

    
        [Authorize]
        [HttpPost("reset-password")]
        public async Task<ActionResult<ApiResponse<object>>> ResetPassword(
            [FromBody] ResetPasswordDto dto)
        {
            if (dto.NewPassword != dto.confirmPassword)
            {
                return BadRequest(new ApiResponse<object>(
                    false,
                    "Password does not match",
                    "Confirm password mismatch"));
            }

            int userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier));

            await _userBL.ResetPasswordAsync(userId, dto.NewPassword);

            return Ok(new ApiResponse<object>(
                true,
                "Password reset successfully",
                null));
        }

 
        [HttpGet("{userId}")]
        public async Task<ActionResult<ApiResponse<UserResponseDto>>> GetUserById(
            int userId)
        {
            try
            {
                var result = await _userBL.GetUserByIdAsync(userId);

                return Ok(new ApiResponse<UserResponseDto>(
                    true,
                    "User retrieved successfully",
                    result));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponse<UserResponseDto>(
                    false,
                    ex.Message,
                    ex.Message));
            }
        }
    }
}
