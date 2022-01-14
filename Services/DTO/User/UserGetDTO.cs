using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO.User
{
    public class UserGetDTO
    {
        public long ID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }
}
