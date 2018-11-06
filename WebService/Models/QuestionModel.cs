using System;
using System.Collections.Generic;

namespace WebService.Models
{
    public class QuestionModel : PostModel
    {
        public DateTime? ClosedDate { get; set; }
        public List<TagModel> Tags { get; set; }
        public List<AnswerListModel> Answers { get; set; }
    }
}
