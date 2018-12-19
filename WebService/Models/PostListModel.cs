using System;
namespace WebService.Models
{
    public abstract class PostListModel
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public int? Score { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? ParentId { get; set; }
    }
}
