using System;
using Microsoft.AspNetCore.Mvc;
using ProjectPortfolio2.DatabaseModel;
using System.Linq;
using WebService.Models;
using ProjectPortfolio2;

namespace WebService.Controllers
{
    [Route("api/posts")]
    [ApiController]
    public class QuestionsController : Controller
    {
        private readonly IDataService _dataService;
        public QuestionsController(IDataService dataService)
        {
            _dataService = dataService;
        }
        [HttpGet(Name = nameof(GetQuestions))]
        public IActionResult GetQuestions()
        {
            var posts = _dataService.GetQuestions().Select(CreateQuestionListModel);
            var result = new
            {
                Items = posts
            };
            return Ok(result);
        }

        [HttpGet("{id}", Name = nameof(GetQuestionById))]
        public IActionResult GetQuestionById(int id)
        {
            var post = _dataService.GetQuestionById(id);
            if (post == null) return NotFound();
            if (post.Type == 1) {
                var model = CreateQuestionModel(post);
                return Ok(model);
            }
            return NotFound();
            //TODO return not found?
        }

        QuestionModel CreateQuestionModel(Question question)
        {
            AnswersController answersController = new AnswersController(_dataService);
            var model = new QuestionModel
            {
                Title = question.Title,
                Score = question.Score,
                Body = question.Body,
                CreationDate = question.CreationDate,
                Answers = question.Answers.Select(answersController.CreateAnswerListModel).ToList()
            };
            return model;
        }

        QuestionListModel CreateQuestionListModel(Question question)
        {
            var model = new QuestionListModel
            {
                Title = question.Title,
                Score = question.Score,
                CreationDate = question.CreationDate,
            };
            model.Url = Url.Link(nameof(GetQuestionById), new { id = question.Id });
            return model;
        }
        //AnswerListModel CreateAnswerListModel(Answer answer)
        //{
        //    Console.WriteLine("Creating answer model: " + answer.Body);
        //    var model = new AnswerListModel
        //    {
        //        Title = answer.Title,
        //        Score = answer.Score
        //    };
        //    AnswersController answersController = new AnswersController(_dataService);
        //    model.Url = Url.Link(nameof(answersController.GetAnswerById), new { id = answer.Id });
        //    return model;
        //}

}
}
