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
            var userSearchHistory = _dataService.GetUserSearchHistory(user.Id).Select(CreateSearchHistoryModel);
            model.SearchHistoryList = userSearchHistory.ToList();
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
    }
}
