using System;
namespace ProjectPortfolio2.DatabaseModel
{
    public class SearchHistory
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Searchtext { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
