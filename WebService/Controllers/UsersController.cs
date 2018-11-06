using System;
using Microsoft.AspNetCore.Mvc;
using ProjectPortfolio2.DatabaseModel;
using WebService.Models;
using System.Linq;

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
            // maybe someting else needs to be returned is user is null, also handle errors and validation like email already used etc.
            if (user == null) return NotFound();
            var model = CreateUserModel(user);
            return Ok(model);
        }

        [HttpPut("{id}", Name = nameof(UpdateUser))]
        public IActionResult UpdateUser(int id, UserModel userRequest)
        {
            //TODO update user function in dataservice
            return NotFound();
        }

        [HttpDelete("{id}", Name = nameof(DeleteUser))]
        public IActionResult DeleteUser(int id)
        {
            //TODO delete user function in dataservice
            return NotFound();
        }

        [HttpGet("{id}/marked_posts", Name = nameof(GetMarkedQuestions))]
        public IActionResult GetMarkedQuestions(int id)
        {
            //TODO questions marked by user
            return NotFound();
        }

        [HttpPost("{id}/marked_posts", Name = nameof(MarkQuestion))]
        public IActionResult MarkQuestion(MarkPostRequest markPostRequest)
        {
            //TODO create marked_post record
            return NotFound();
        }

        [HttpPut("{id}/marked_posts/{marked_post_id}", Name = nameof(UpdateMarkedQuestion))]
        public IActionResult UpdateMarkedQuestion(int id, int markedPostId, MarkPostRequest markPostRequest)
        {
            //TODO update marked_post
            return NotFound();
        }

        [HttpDelete("{id}/marked_posts/{marked_post_id}", Name = nameof(UnmarkQuestion))]
        public IActionResult UnmarkQuestion(int id, int marked_post_id)
        {
            //TODO delete marked post
            return NotFound();
        }

        [HttpGet("{id}/marked_answers", Name = nameof(GetMarkedAnswers))]
        public IActionResult GetMarkedAnswers(int id)
        {
            //TODO answers marked by user
            return NotFound();
        }

        [HttpGet("{id}/marked_comments", Name = nameof(GetMarkedComments))]
        public IActionResult GetMarkedComments(int id)
        {
            //TODO comments marked by user
            return NotFound();
        }

        [HttpGet("{id}/search_history", Name = nameof(GetUserSearchHistory))]
        public IActionResult GetUserSearchHistory(int id)
        {
            //TODO return user search history
            return NotFound();
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
            //var userSearchHistory = _dataService.GetUserSearchHistory(user.Id).Select(CreateSearchHistoryModel);
            //model.SearchHistoryList = userSearchHistory.ToList();
            //var markedPosts = user.UserMarkedPosts.Select(CreateUserMarkedPostModel).ToList();
            //model.MarkedPosts = markedPosts;
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

        UserMarkedPostModel CreateUserMarkedPostModel(PostMarked postMarked)
        {
            var model = new UserMarkedPostModel
            {
                Url = Url.Link(nameof(QuestionsController.GetQuestionById), new { id = postMarked.PostId})
            };
            return model;
        }
    }
}
