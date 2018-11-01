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

    public class PostsController : Controller
    {
        private readonly IDataService _dataService;
        public PostsController(IDataService dataService)
        {
            _dataService = dataService;
        }
        [HttpGet(Name = nameof(GetPosts))]
        public IActionResult GetPosts()
        {
            var posts = _dataService.GetPosts().Select(CreatePostListModel);
            var result = new
            {
                Items = posts
            };
            return Ok(result);
        }

        [HttpGet("{id}", Name = nameof(GetPostById))]
        public IActionResult GetPostById(int id)
        {
            var post = _dataService.GetPostById(id);
            if (post == null) return NotFound();
            var model = CreatePostModel(post);

            //model.Url = Url.Link(nameof(GetPostById), new { id = post.Id });
           
            return Ok(model);
        }



        private PostModel CreatePostModel(Post post)
        {
            var model = new PostModel
            {
                Title = post.Title,
                Score = post.Score,
                Body = post.Body,
                CreationDate = post.CreationDate
            };
            return model;
        }

        private PostListModel CreatePostListModel(Post post)
        {
            var model = new PostListModel
            {
                Title = post.Title,
                Score = post.Score,
                CreationDate = post.CreationDate,
            };
            model.Url = Url.Link(nameof(GetPostById), new { id = post.Id });
            return model;
        }


    }
}
