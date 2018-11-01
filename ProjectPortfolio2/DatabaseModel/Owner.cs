using System;
using System.Collections.Generic;
using ProjectPortfolio2.DatabaseModel;

namespace ProjectPortfolio2
{
    public class Owner
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string Location { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? Age { get; set; }
        public List<Question> Questions { get; set; }
    }
}
