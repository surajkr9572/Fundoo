using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Dto
{

    public class NoteDetailsDto
    {
        public int NoteId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Colour { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ? Reminder {  get; set; }
        public bool IsPin { get; set; }
        public bool IsArchive { get; set; }
        public bool IsTrash { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public ICollection<LabelResponseDto> Labels { get; set; }
        public ICollection<CollaboratorResponseDto> Collaborators { get; set; }
    }
}

