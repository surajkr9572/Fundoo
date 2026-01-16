using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLayer.Entities
{
    public class Label
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LabelId { get; set; }   

        [Required(ErrorMessage = "Label Name is required")]
        [MaxLength(100)]
        public string LabelName { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public int UserId { get; set; }

        
        //public int NotesId { get; set; }

        public User User { get; set; } = null!;
        //public Note Note { get; set; } = null!;

        public ICollection<Note>Notes { get; set; }

    }
}
