    using System;

namespace ModelLayer.Dto
{
    public class NoteResponseDto
    {
        public int NotesId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; } 

        public DateTime? UpdatedAt { get; set; }

        public string Colour { get; set; }
        public bool IsArchive { get; set; }
        public bool IsPin { get; set; }
        public bool IsTrash { get; set; }

        public DateTime CreatedAt { get; set; }


        
    }
}
