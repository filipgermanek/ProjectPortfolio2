using System;
using System.Collections.Generic;

namespace ProjectPortfolio2.DatabaseModel
{
    public class Question : Post
    {
        public DateTime? ClosedDate { get; set; }
        public List<PostTag> PostTags { get; set; }
        public List<Answer> Answers { get; set; }
        public string Title { get; set; }
    }
}
