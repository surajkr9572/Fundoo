using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Dto
{
    public class NoteArchiveResponseDto
    {
        public int NotesId { get; set; }
        public bool IsArchive { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
