using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebService.Models
{
    public class CommentListModel
    {
        public String Url { get; set; }
        public int? Score { get; set; } // ? means that the value can be null - not needed on strings
        public string Text { get; set; }
    }
}