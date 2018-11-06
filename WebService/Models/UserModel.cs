using System;
using System.Collections.Generic;

namespace WebService.Models
{
    public class UserModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
