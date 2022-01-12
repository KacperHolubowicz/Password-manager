using System.Collections.Generic;

namespace Domain.Models
{
    /// <summary>
    /// Database entity class of User
    /// </summary>
    public class User
    {
        public long ID { get; set; }
        public string Username { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public byte[] Password { get; set; }
        public byte[] MasterPassword { get; set; }
        public List<ServicePassword> ServicePasswords { get; set; }
        public List<Device> Devices { get; set; }
    }
}
