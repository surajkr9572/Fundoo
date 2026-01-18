using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Dto
{
    public class NotePinDto
    {
        [Required]
        public bool IsPin { get; set; }
    }
}
