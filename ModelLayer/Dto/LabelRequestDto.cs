using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Dto
{
    public class LabelRequestDto
    {
        [Required(ErrorMessage = "Label Name is required")]
        [MaxLength(100, ErrorMessage = "Label Name cannot exceed 100 characters")]
        public string LabelName { get; set; } = null!;
        public int? NotesId { get; set; }

    }
}
