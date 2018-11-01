using System;
namespace WebService.Models
{
    public class PostListModel
    {
        public string Url { get; set; }
        public string Title { get; set; }
        public int? Score { get; set; }
        //public string Body { get; set; }
        public DateTime? CreationDate { get; set; }

    }
}
