using System;
using System.Collections.Generic;

namespace ProjectPortfolio2.DatabaseModel
{
    public class Question : Post
    {
        public DateTime? ClosedDate { get; set; }
        public List<Answer> Answers { get; set; }
    }
}
