using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Dto
{
    public class NoteUpdateDto
    {
        [MaxLength(255, ErrorMessage = "Title cannot exceed 255 characters")]
        public string Title { get; set; }

        public string Description { get; set; }

        public string Colour { get; set; }

        public DateTime? Reminder { get; set; }
    }
}
