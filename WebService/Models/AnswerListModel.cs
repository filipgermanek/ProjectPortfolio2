using System;
namespace WebService.Models
{
    public class AnswerListModel : PostListModel
    {
        public bool Accepted { get; set; }
        public string Body { get; set; }
    }
}
