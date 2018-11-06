using System;
namespace WebService.Models
{
    public class UserMarkedPostModel
    {
        int UserId { get; set; }
        int PostId { get; set; }
        public string Url { get; set; }
    }
}
