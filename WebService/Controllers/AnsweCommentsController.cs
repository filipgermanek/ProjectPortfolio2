using System;
using System.Diagnostics.Contracts;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ProjectPortfolio2;
using ProjectPortfolio2.DatabaseModel;
using WebService.Models;

namespace WebService.Controllers
{
    [Route("api/posts/{postId}/answers/{answerId}/comments")]
    [ApiController]
    public class AnsweCommentsController : Controller
    {
        private readonly IDataService _dataService;
        public AnsweCommentsController(IDataService dataService)
        {
            _dataService = dataService;
        }
        [HttpGet(Name = nameof(GetCommentsByAnswerId))]
        public IActionResult GetCommentsByAnswerId(int postId)
        {
            Contract.Ensures(Contract.Result<IActionResult>() != null);
            var comments = _dataService.GetCommentsByAnswerId(postId).Select(CreateCommentListModel);
            var result = new
            {
                Items = comments
            };
            return Ok(result);
        }
        [HttpGet("{id}", Name = nameof(GetCommentForAnswer))]
        public IActionResult GetCommentForAnswer(int id)
        {
            Contract.Ensures(Contract.Result<IActionResult>() != null);
            var comment = _dataService.GetCommentForAnswer(id);
            if (comment == null) return NotFound();
            var model = CreateCommentModel(comment);
            return Ok(model);
        }

        private CommentModel CreateCommentModel(Comment comment)
        {
            var model = new CommentModel
            {
                //Id = comment.Id,
                Score = comment.Score,
                Text = comment.Text,
                CreationDate = comment.CreationDate


            };
            return model;
        }

        private CommentListModel CreateCommentListModel(Comment comment)
        {
            var model = new CommentListModel
            {
                Score = comment.Score
            };
            model.Url = Url.Link(nameof(GetCommentForAnswer), new { id = comment.Id });
            return model;
        }
    }
}
