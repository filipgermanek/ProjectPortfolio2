using System;
using Microsoft.AspNetCore.Mvc;
using ProjectPortfolio2.DatabaseModel;
using WebService.Models;
using System.Linq;

namespace WebService.Controllers
{
    [Route("api/posts/{postId}/answers")]
    [ApiController]
    public class AnswersController : Controller
    {
        readonly IDataService _dataService;
        public AnswersController(IDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet(Name = nameof(GetAnswersByQuestionid))]
        public IActionResult GetAnswersByQuestionid(int postId)
        {
            var answers = _dataService.GetAnswersByQuestionId(postId).Select(CreateAnswerListModel);
            var result = new
            {
                Items = answers
            };
            return Ok(result);
        }

        [HttpGet("{id}", Name = nameof(GetAnswerById))]
        public IActionResult GetAnswerById(int id)
        {
            var answer = _dataService.GetAnswer(id);
            if (answer == null) return NotFound();
            if (answer.Type == 2)
            {
                var model = CreateAnswerModel(answer);
                return Ok(model);
            }
            return NotFound();
        }

        public AnswerListModel CreateAnswerListModel(Answer answer)
        {
            var model = new AnswerListModel
            {
                Title = answer.Title,
                Score = answer.Score
            };
            model.Url = Url.Link(nameof(GetAnswerById), new { id = answer.Id });
            return model;
        }

        AnswerModel CreateAnswerModel(Answer answer)
        {
            var model = new AnswerModel
            {
                Title = answer.Title,
                Score = answer.Score,
                Body = answer.Body,
                CreationDate = answer.CreationDate
            };
            return model;
        }

    }
}
