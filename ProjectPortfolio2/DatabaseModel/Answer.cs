using System;
namespace ProjectPortfolio2.DatabaseModel
{
    public class Answer : Post
    {
        public int QuestionId { get; set; }
        public bool Accepted { get; set; }
    }
}
