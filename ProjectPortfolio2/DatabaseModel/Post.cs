using System;

namespace ProjectPortfolio2
{
    public class Post
    {
        public int Id { get; set; }
        public int? Score { get; set; }
        public string Body{ get; set; }
        public DateTime? CreationDate { get; set; }
        public string Title { get; set; }
        public int OwnerId { get; set; }
        public int Type { get; set; }
        public DateTime? ClosedDate { get; set; }
    }
}
