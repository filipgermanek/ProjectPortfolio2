using System;
using System.Collections.Generic;

namespace ProjectPortfolio2.DatabaseModel
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
