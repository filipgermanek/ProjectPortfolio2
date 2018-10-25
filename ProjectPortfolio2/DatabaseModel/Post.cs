using System;


namespace ProjectPortfolio2
{
    public class Post
    {
        public int PostId { get; set; }
        public int? Score { get; set; }
        public string Body{ get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        public string Title { get; set; }
        public int? ParentId { get; set; }
        public bool Accepted { get; set; }
        public int OwnerId { get; set; }
    }
}
