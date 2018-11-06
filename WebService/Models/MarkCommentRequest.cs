using System;
namespace WebService.Models
{
    public class MarkCommentRequest
    {
        public int UserId { get; set; }
        public int CommentId { get; set; }
        public string AnnotationText { get; set; }
    }
}
