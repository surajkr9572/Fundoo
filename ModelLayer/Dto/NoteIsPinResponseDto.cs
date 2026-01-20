using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Dto
{
    public class NoteIsPinResponseDto
    {
        public int NotesId { get; set; }
        public bool IsPin { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
}
