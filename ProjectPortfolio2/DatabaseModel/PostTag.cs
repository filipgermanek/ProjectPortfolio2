using System;
namespace ProjectPortfolio2.DatabaseModel
{
    public class PostTag
    {
        public int PostId { get; set; }
        public int QuestionId { get; set; }
        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
