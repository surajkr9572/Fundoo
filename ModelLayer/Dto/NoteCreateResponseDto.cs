using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Dto
{
    public class NoteCreateResponseDto
    {
        public int NotesId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
        public string Colour { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
    }
}
