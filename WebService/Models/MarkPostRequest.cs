using System;
namespace WebService.Models
{
    public class MarkPostRequest
    {
        int UserId { get; set; }
        int PostId { get; set; }
        string Annotation { get; set; }
    }
}
