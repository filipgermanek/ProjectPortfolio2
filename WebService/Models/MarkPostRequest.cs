using System;
namespace WebService.Models
{
    public class MarkPostRequest
    {
        public int UserId { get; set; }
        public int PostId { get; set; }
        public string AnnotationText { get; set; }
    }
}
