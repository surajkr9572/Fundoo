using System;
using System.ComponentModel.DataAnnotations;

namespace ModelLayer.DTOs
{
    public class NoteCreateDto
    {
        [Required(ErrorMessage = "Title is required")]
        [MaxLength(255, ErrorMessage = "Title cannot exceed 255 characters")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = null!;

        public DateTime? Reminder { get; set; }

        [MaxLength(7, ErrorMessage = "Colour must be a valid hex code")]
        public string Colour { get; set; } = "#FFFFFF";
        public bool IsPin { get; set; }

        [Required(ErrorMessage = "UserId is required")]
        public int UserId { get; set; }
    }
}

