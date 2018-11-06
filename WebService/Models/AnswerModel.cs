using System;
namespace WebService.Models
{
    public class AnswerModel : PostModel
    {
        public int ParentId { get; set; }
        public bool Accepted { get; set; }
    }
}
