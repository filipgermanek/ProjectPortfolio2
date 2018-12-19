using System;
namespace WebService.Models
{
    public class UserMarkedPostModel
    {
        public int UserId { get; set; }
        public int PostId { get; set; }
        public string AnnotationText { get; set; }
        public string UrlToPost { get; set; }
        public string PostTitle { get; set; }
    }
}
