﻿using System;

namespace ProjectPortfolio2
{
    public class Comment
    {
        public int Id { get; set; }
        public int? Score { get; set; } // ? means that the value can be null - not needed on strings
        public string Text { get; set; }
        public DateTime? CreationDate { get; set; }
        public int PostId { get; set; }
        public int OwnerId { get; set; }
    }

}
