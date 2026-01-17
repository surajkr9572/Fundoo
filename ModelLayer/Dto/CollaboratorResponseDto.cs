using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Dto
{
    public class CollaboratorResponseDto
    {
        public int CollaboratorId { get; set; }
        public int NoteId { get; set; }
        public string Email { get; set; }

        public DateTime AddedAt { get; set; }


    }
}
