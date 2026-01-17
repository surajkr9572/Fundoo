using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Entities
{
    public class Collaborator
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CollaboratorId { get; set; }

        [Required(ErrorMessage="Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [MaxLength(255,ErrorMessage="Email cannot exceed 255 character")]
        public string Email { get;set;  }
        public int UserId { get; set; }

        public int NoteId { get; set; }
        public User User { get;set; }
        public Note Note { get;set; }

    }
}
