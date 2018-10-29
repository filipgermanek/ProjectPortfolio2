using Microsoft.AspNetCore.Mvc;
using ProjectPortfolio2;
using ProjectPortfolio2.DatabaseModel;
using System;
using System.Linq;
using WebService.Models;
using System.Diagnostics.Contracts;

namespace WebService.Controllers
{
    [Route("api/comments")]
    [ApiController]
    public class CommentsController :Controller
    {
        private readonly IDataService _dataService;
        public CommentsController(IDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet(Name = nameof(GetComments))]
        public IActionResult GetComments()
        {
            Contract.Ensures(Contract.Result<IActionResult>() != null);
            var comments = _dataService.GetComments().Select(CreateCommentModel);
            var result = new
            {
                Items = comments
            };
            return Ok(result);
        }
        [HttpGet("{id}", Name = nameof(GetComment))]
        public IActionResult GetComment(int id)
        {
            Contract.Ensures(Contract.Result<IActionResult>() != null);
            var comment = _dataService.GetComment(id);
            if (comment== null) return NotFound();
            var model = CreateCommentModel(comment);
            return Ok(model);
        }

        private CommentModel CreateCommentModel(Comment comment)
        {
            var model = new CommentModel
            {
               Score = comment.Score,
               Text = comment.Text,
               CreationDate = comment.CreationDate

            };
            return model;
        }

        
    }
}
