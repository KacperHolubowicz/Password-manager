using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO.User
{
    public class UserPostDTO
    {
        public string Username { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public byte[] Password { get; set; }
        public byte[] MasterPassword { get; set; }
    }
}
