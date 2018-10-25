using System;
namespace ProjectPortfolio2.DatabaseModel
{
    public class Answer : Post
    {
        public int ParentId { get; set; }
        public bool Accepted { get; set; }
    }
}
