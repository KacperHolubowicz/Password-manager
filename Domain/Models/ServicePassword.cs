﻿namespace Domain.Models
{
    /// <summary>
    /// Database entity class for passwords saved by a user
    /// </summary>
    public class ServicePassword
    {
        public long ID { get; set; }
        public User Owner { get; set; }
        public string Description { get; set; }
        public byte[] Password { get; set; }
    }
}