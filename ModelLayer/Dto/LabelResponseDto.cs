using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Dto
{
    public class LabelResponseDto
    {
        public int LabelId { get; set; }
        public string LabelName { get; set; }
        public int NotesId { get; set; }     
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
