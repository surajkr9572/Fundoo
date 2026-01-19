using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BessinessLogicLayer.Helpers
{
    public class EmailSettings
    {
        public required string Server {  get; set; }
        public required int Port { get; set; }
        public required string SenderName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
