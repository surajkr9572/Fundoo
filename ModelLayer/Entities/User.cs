using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLayer.Entities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

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
        [MaxLength(255,ErrorMessage="Password cannot exceed 255 characters")]
        public string Password { get; set; } = null!; 

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ChangedAt { get; set; }= DateTime.UtcNow;

        public ICollection<Note> Notes { get; set; }
        public ICollection<Label>Labels { get; set; }
        public ICollection<Collaborator> Collaborators { get; set; }
    }
}
