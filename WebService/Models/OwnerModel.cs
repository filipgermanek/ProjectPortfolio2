using System;
namespace WebService.Models
{
    public class OwnerModel
    {
        public string DisplayName { get; set; }
        public string Location { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? Age { get; set; }
    }
}
