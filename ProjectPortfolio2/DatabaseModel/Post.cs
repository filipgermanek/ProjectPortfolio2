using System;
using System.Collections.Generic;
using ProjectPortfolio2.DatabaseModel;

namespace ProjectPortfolio2
{
    public abstract class Post
    {
        public int Id { get; set; }
        public int? Score { get; set; }
        public string Body{ get; set; }
        public DateTime? CreationDate { get; set; }
        public int OwnerId { get; set; }
        public int Type { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
