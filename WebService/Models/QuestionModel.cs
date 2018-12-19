using System;
using System.Collections.Generic;

namespace WebService.Models
{
    public class QuestionModel : PostModel
    {
        public int Id { get; set; }
        public DateTime? ClosedDate { get; set; }
        public bool IsAnnotated { get; set; }
        public String AnnotationText { get; set; }
        public List<TagModel> Tags { get; set; }
        public List<AnswerListModel> Answers { get; set; }
    }
}
