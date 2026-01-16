using System.ComponentModel.DataAnnotations;

namespace ModelLayer.DTOs
{
    public class UserRequestDto
    {
        [Required(ErrorMessage = "First Name is required")]
        [MaxLength(100, ErrorMessage = "First Name cannot exceed 100 characters")]
        [MinLength(2, ErrorMessage = "First Name must be at least 2 characters")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Last Name is required")]
        [MaxLength(100, ErrorMessage = "Last Name cannot exceed 100 characters")]
        [MinLength(2, ErrorMessage = "Last Name must be at least 2 characters")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [MaxLength(255, ErrorMessage = "Email cannot exceed 255 characters")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        [MaxLength(255, ErrorMessage = "Password cannot exceed 255 characters")]
        public string Password { get; set; } = null!;
    }
}
