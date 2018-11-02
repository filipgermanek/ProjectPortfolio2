using System;
using Microsoft.AspNetCore.Mvc;
using ProjectPortfolio2.DatabaseModel;
using WebService.Models;
using System.Linq;

namespace WebService.Controllers
{
    [Route("api/posts/{postId}/tags")]
    [ApiController]
    public class TagsController : Controller
    {
        private readonly IDataService _dataService;
        public TagsController(IDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet(Name = nameof(GetTagsByQuestionId))]
        public IActionResult GetTagsByQuestionId(int postId)
        {
            var tags = _dataService.GetTagsByQuestionId(postId).Select(x => CreateTagModel(x, true));
            var result = new
            {
                Items = tags
            };
            return Ok(result);
        }

        [HttpGet("{id}", Name = nameof(GetTagById))]
        public IActionResult GetTagById(int id)
        {
            var tag = _dataService.GetTag(id);
            if (tag == null) return NotFound();
            var model = CreateTagModel(tag, false);
            return Ok(model);
        }

        public TagModel CreateTagModel(Tag tag, bool isListModel)
        {
            var model = new TagModel
            {
                Name = tag.Name
            };
            if (isListModel)
            {
                model.Url = Url.Link(nameof(GetTagById), new { id = tag.Id });
            }
            return model;
        }
    }
}
