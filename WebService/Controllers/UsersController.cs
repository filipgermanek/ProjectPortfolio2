using System;
using Microsoft.AspNetCore.Mvc;
using ProjectPortfolio2.DatabaseModel;
using WebService.Models;
using System.Linq;
using System.Collections.Generic;
using ProjectPortfolio2;

namespace WebService.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IDataService _dataService;
        public UsersController(IDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet(Name = nameof(GetUsers))]
        public IActionResult GetUsers()
        {
            var users = _dataService.GetUsers().Select(CreateUserListModel);
            var result = new
            {
                Items = users
            };
            return Ok(result);
        }

        [HttpGet("{id}", Name = nameof(GetUser))]
        public IActionResult GetUser(int id)
        {
            var user = _dataService.GetUser(id);
            if (user == null) return NotFound();
            var model = CreateUserModel(user);
            return Ok(model);
        }

        [HttpPost(Name = nameof(CreateUser))]
        public IActionResult CreateUser(UserModel userRequest)
        {
            var user = _dataService.CreateUser(userRequest.Email, userRequest.Password, userRequest.Name, userRequest.Location);
            if (user == null) return NotFound();
            var model = CreateUserModel(user);
            return Ok(model);
        }

        [HttpPut("{id}", Name = nameof(UpdateUser))]
        public IActionResult UpdateUser(int id, UserModel userRequest)
        {
            var user = _dataService.UpdateUser(id, userRequest.Email, userRequest.Password, userRequest.Name, userRequest.Location);
            if (user == null) return NotFound();
            var model = CreateUserModel(user);
            return Ok(model);
        }

        [HttpDelete("{id}", Name = nameof(DeleteUser))]
        public IActionResult DeleteUser(int id)
        {
            bool success = _dataService.DeleteUser(id);
            return success ? Ok() : (IActionResult)NotFound();
        }

        [HttpGet("{id}/marked_posts", Name = nameof(GetMarkedQuestions))]
        public IActionResult GetMarkedQuestions(int id)
        {
            var markedQuestions = _dataService.GetMarkedQuestions(id).ToList();
            List<Question> questions = _dataService.GetQuestionForIds(markedQuestions.Select(x => x.PostId).ToList());
            List<UserMarkedPostModel> items = new List<UserMarkedPostModel>();
            questions.ForEach(question =>
            {
                var postMarked = markedQuestions.Find(x => x.PostId.Equals(question.Id));
                var item = CreateUserMarkedPostModel(question, postMarked);
                items.Add(item);
            });
            var result = new {
                Items = items
            };
            return Ok(result);
        }

        [HttpPost("{id}/marked_posts", Name = nameof(MarkQuestion))]
        public IActionResult MarkQuestion(int id, MarkPostRequest markPostRequest)
        {
            var markedPost = _dataService.UserMarkPost(markPostRequest.PostId, id, markPostRequest.AnnotationText);
            if (markedPost == null) {
                return NotFound();
            }
            var result = new
            {
                markedPost.PostId,
                markedPost.UserId,
                markedPost.AnnotationText
            };
            return Ok(result);
        }

        [HttpPut("{id}/marked_posts/{marked_post_id}", Name = nameof(UpdateMarkedQuestion))]
        public IActionResult UpdateMarkedQuestion(int id, int marked_post_id, MarkPostRequest markPostRequest)
        {
            var markedPost = _dataService.UserUpdateMarkedPost(marked_post_id, id, markPostRequest.AnnotationText);
            return markedPost == null ? NotFound() : (IActionResult)Ok(markedPost);
        }

        [HttpDelete("{id}/marked_posts/{marked_post_id}", Name = nameof(UnmarkQuestion))]
        public IActionResult UnmarkQuestion(int id, int marked_post_id)
        {
            var success = _dataService.UserUnmarkPost(marked_post_id, id);
            return success ? Ok() : (IActionResult)NotFound();
        }

        [HttpGet("{id}/marked_comments", Name = nameof(GetMarkedComments))]
        public IActionResult GetMarkedComments(int id)
        {
            var markedComments = _dataService.GetMarkedComments(id).Select(CreateUserMarkedCommentModel);
            var result = new
            {
                Items = markedComments
            };
            return Ok(result);
        }

        [HttpPost("{id}/marked_comments", Name = nameof(MarkComment))]
        public IActionResult MarkComment(int id, MarkCommentRequest markCommentRequest)
        {
            var markedComment = _dataService.UserMarkComment(markCommentRequest.CommentId, id, markCommentRequest.AnnotationText);
            if (markedComment == null)
            {
                return NotFound();
            }
            var result = new
            {
                markedComment.CommentId,
                markedComment.UserId,
                markedComment.AnnotationText
            };
            return Ok(result);
        }

        [HttpPut("{id}/marked_comments/{marked_comment_id}", Name = nameof(UpdateMarkedComment))]
        public IActionResult UpdateMarkedComment(int id, int marked_comment_id, MarkCommentRequest markCommentRequest)
        {
            var markedComment = _dataService.UserUpdateCommentAnnotation(marked_comment_id, id, markCommentRequest.AnnotationText);
            if (markedComment == null) {
                return NotFound();
            }
            var result = new
            {
                markedComment.CommentId,
                markedComment.UserId,
                markedComment.AnnotationText
            };
            return Ok(result);
        }

        [HttpDelete("{id}/marked_comments/{marked_comment_id}", Name = nameof(UnmarkComment))]
        public IActionResult UnmarkComment(int id, int marked_comment_id)
        {
            var success = _dataService.UserUnmarkComment(marked_comment_id, id);
            return success ? Ok() : (IActionResult)NotFound();
        }

        [HttpGet("{id}/search_history", Name = nameof(GetUserSearchHistory))]
        public IActionResult GetUserSearchHistory(int id)
        {
            var SearchHistory = _dataService.GetUserSearchHistory(id).Select(CreateSearchHistoryModel);
            if (SearchHistory == null) return NotFound();
            var result = new
            {
                Items = SearchHistory
            };
            return Ok(result);
        }

        UserListModel CreateUserListModel(User user)
        {
            var model = new UserListModel
            {
                Email = user.Email,
                Name = user.Name
            };
            model.Url = Url.Link(nameof(GetUser), new { id = user.Id });
            return model;
        }

        UserModel CreateUserModel(User user)
        {
            var model = new UserModel
            {
                Email = user.Email,
                Password = user.Password,
                Name = user.Name,
                Location = user.Location,
                CreationDate = user.CreationDate,
            };
            return model;
        }

        SearchHistoryModel CreateSearchHistoryModel(SearchHistory searchHistory)
        {
            var model = new SearchHistoryModel
            {
                Searchtext = searchHistory.Searchtext,
                CreationDate = searchHistory.CreationDate
            };
            return model;
        }

        UserMarkedCommentModel CreateUserMarkedCommentModel(CommentMarked commentMarked)
        {
            var model = new UserMarkedCommentModel
            {
                AnnotationText = commentMarked.AnnotationText
            };
            return model;
        }

        UserMarkedPostModel CreateUserMarkedPostModel(Question question, PostMarked postMarked)
        {
            var model = new UserMarkedPostModel
            {
                PostTitle = question.Title,
                PostId = postMarked.PostId, 
                UserId = postMarked.UserId,
                AnnotationText = postMarked.AnnotationText,
                UrlToPost = Url.Link("GetQuestionById", new { id = postMarked.PostId })
            };
            return model;
        }
    }
}
