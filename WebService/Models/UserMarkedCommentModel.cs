using System;
namespace WebService.Models
{
    public class UserMarkedCommentModel
    {
        int UserId { get; set; }
        int CommentId { get; set; }
        public string AnnotationText { get; set; }
        public string Url { get; set; }
    }
}
