using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLayer.Entities
{
    public class Note
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NotesId { get; set; }   

        [Required(ErrorMessage = "Title is required")]
        [MaxLength(255,ErrorMessage="Title cnnot exceed 255 character")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = null!; 

        public DateTime? Reminder { get; set; }

        [MaxLength(7)]
        public string Colour { get; set; } ="#FFFFFF";

        public string Image { get; set; }   

        public bool IsArchive { get; set; }
        public bool IsPin { get; set; }
        public bool IsTrash { get; set; } 

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; }
       
       
        public int UserId { get; set; }

        public User User { get; set; }
        public ICollection<Label> Labels {  get; set; }
        public ICollection<Collaborator> Collaborators { get; set; }
    }
}
