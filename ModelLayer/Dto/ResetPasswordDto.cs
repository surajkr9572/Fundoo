using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Dto
{
    public class ResetPasswordDto
    {
        public string NewPassword { get; set; }
        public string confirmPassword { get; set; }
    }
}
