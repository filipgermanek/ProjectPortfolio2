﻿using Microsoft.AspNetCore.Mvc;
using ProjectPortfolio2;
using ProjectPortfolio2.DatabaseModel;
using System;
using System.Linq;
using WebService.Models;
using System.Diagnostics.Contracts;

namespace WebService.Controllers
{
    [Route("api/posts/{postId}/comments")]
    [ApiController]
    public class CommentsController :Controller
    {
        private readonly IDataService _dataService;
        public CommentsController(IDataService dataService)
        {
            _dataService = dataService;
        }


        [HttpGet(Name = nameof(GetCommentsByQuestionId))]
        public IActionResult GetCommentsByQuestionId(int postId)
        {
            Contract.Ensures(Contract.Result<IActionResult>() != null);
            var comments = _dataService.GetCommentsByQuestionId(postId).Select(CreateCommentListModel);
            var result = new
            {
                Items = comments
            };
            return Ok(result);
        }
        [HttpGet("{id}", Name = nameof(GetCommentForQuestion))]
        public IActionResult GetCommentForQuestion(int id)
        {
            Contract.Ensures(Contract.Result<IActionResult>() != null);
            var comment = _dataService.GetCommentForQuestion(id);
            if (comment== null) return NotFound();
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
            model.Url = Url.Link(nameof(GetCommentForQuestion), new { id = comment.Id });
            return model;
        }
    }
}
