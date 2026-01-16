using System;

namespace ModelLayer.DTOs
{
    public class UserResponseDto
    {
        public int UserId { get; set; }

        public string FirstName { get; set; } 

        public string LastName { get; set; }

        public string Email { get; set; }

        public string token {  get; set; }
    }
}
