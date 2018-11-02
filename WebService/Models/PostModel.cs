using System;
using System.Collections.Generic;

namespace WebService.Models
{
    public abstract class PostModel
    {
        public string Title { get; set; }
        public int? Score { get; set; }
        public string Body { get; set; }
        public DateTime? CreationDate { get; set; }
        public List<CommentListModel> Comments { get; set; }
    }
}
