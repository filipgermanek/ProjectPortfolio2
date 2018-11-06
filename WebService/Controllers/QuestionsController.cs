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
        public readonly IDataService _dataService;
        public QuestionsController(IDataService dataService)
        {
            _dataService = dataService;
        }
        //QUESTION ROUTES START
        [HttpGet(Name = nameof(GetQuestions))]
        public IActionResult GetQuestions(int page = 0, int pageSize = 5)
        {
            var posts = _dataService.GetQuestions(page, pageSize).Select(CreateQuestionListModel);

            var numberOfItems = _dataService.GetNumberOfQuestions();
            var numberOfPages = ComputeNumberOfPages(pageSize, numberOfItems);

            var result = new
            {
                NumberOfItems = numberOfItems,
                NumberOfPages = numberOfPages,
                First = CreateLink(0, pageSize),
                Prev = CreateLinkToPrevPage(page, pageSize),
                Next = CreateLinkToNextPage(page, pageSize, numberOfPages),
                Last = CreateLink(numberOfPages - 1, pageSize),
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
               // var tags = _dataService.GetTagsByQuestionId(id);
               // post.Tags = tags;
                var model = CreateQuestionModel(post);
                return Ok(model);
            }
            return NotFound();
        }
        //QUESTION ROUTES END

        //QUESTION COMMENTS ROUTES START
        [HttpGet("{id}/comments", Name = nameof(GetCommentsByQuestionId))]
        public IActionResult GetCommentsByQuestionId(int id)
        {
            var comments = _dataService.GetCommentsByQuestionId(id).Select(x => CreateCommentListModel(x, true));
            var result = new
            {
                Items = comments
            };
            return Ok(result);
        }
        [HttpGet("{id}/comments/{commentId}", Name = nameof(GetCommentForQuestion))]
        public IActionResult GetCommentForQuestion(int commentId)
        {
            var comment = _dataService.GetCommentForQuestion(commentId);
            if (comment == null) return NotFound();
            var model = CreateCommentModel(comment);
            return Ok(model);
        }
        //QUESTION COMMENTS ROUTES END

        //ANSWERS ROUTES START
        [HttpGet("{id}/answers",Name = nameof(GetAnswersByQuestionid))]
        public IActionResult GetAnswersByQuestionid(int page, int pageSize,int id)
        {
            var answers = _dataService.GetAnswersByQuestionId(id).Select(CreateAnswerListModel);

            var result = new
            {
                Items = answers
            };
            return Ok(result);
        }

        [HttpGet("{id}/answers/{answerId}", Name = nameof(GetAnswerById))]
        public IActionResult GetAnswerById(int answerId)
        {
            var answer = _dataService.GetAnswer(answerId);
            if (answer == null) return NotFound();
            if (answer.Type == 2)
            {
                var model = CreateAnswerModel(answer);
                return Ok(model);
            }
            return NotFound();
        }
        //ANSWERS ROUTES END

        //ANSWERS COMMENTS ROUTES START
        [HttpGet("{id}/answers/{answerId}/comments", Name = nameof(GetCommentsByAnswerId))]
        public IActionResult GetCommentsByAnswerId(int answerId)
        {
            var comments = _dataService.GetCommentsByAnswerId(answerId).Select(x => CreateCommentListModel(x, false));
            var result = new
            {
                Items = comments
            };
            return Ok(result);
        }
        [HttpGet("{id}/answers/{answerId}/comments/{anserCommentId}", Name = nameof(GetCommentForAnswer))]
        public IActionResult GetCommentForAnswer(int anserCommentId)
        {
            var comment = _dataService.GetCommentForAnswer(anserCommentId);
            if (comment == null) return NotFound();
            var model = CreateCommentModel(comment);
            return Ok(model);
        }
        //ANSWERS COMMENTS ROUTES END

        //TAGS ROUTES START
        [HttpGet("{id}/tags", Name = nameof(GetTagsByQuestionId))]
        public IActionResult GetTagsByQuestionId(int postId)
        {
            var tags = _dataService.GetTagsByQuestionId(postId).Select(x => CreateTagModel(x, true));
            var result = new
            {
                Items = tags
            };
            return Ok(result);
        }

        [HttpGet("{id}/tags/{tagId}", Name = nameof(GetTagById))]
        public IActionResult GetTagById(int tagId)
        {
            var tag = _dataService.GetTag(tagId);
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
        //TAGS ROUTES END

        CommentModel CreateCommentModel(Comment comment)
        {
            var model = new CommentModel
            {
                Score = comment.Score,
                Text = comment.Text,
                CreationDate = comment.CreationDate
            };
            return model;
        }

        public CommentListModel CreateCommentListModel(Comment comment, bool isQuestionModel)
        {
            Console.WriteLine("url for comment list model :" + nameof(GetCommentForQuestion));
            var model = new CommentListModel
            {
                Score = comment.Score
            };
            model.Url = isQuestionModel
                ? Url.Link(nameof(GetCommentForQuestion), new { commentId = comment.Id })
                : Url.Link(nameof(GetCommentForAnswer), new { answerCommentId = comment.Id });
            return model;
        }

        QuestionModel CreateQuestionModel(Question question)
        {
            var model = new QuestionModel
            {
                Title = question.Title,
                Score = question.Score,
                Body = question.Body,
                CreationDate = question.CreationDate,
                Answers = question.Answers?.Select(CreateAnswerListModel).ToList(),
                Comments = question.Comments?.Select(x => CreateCommentListModel(x, true)).ToList(),
              // Tags = question.PostTags?.Select(x => CreateTagModel(x, true)).ToList()
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
                Url = Url.Link(nameof(GetQuestionById), new { id = question.Id })
            };
            return model;
        }

        public AnswerListModel CreateAnswerListModel(Answer answer)
        {
            var model = new AnswerListModel
            {
                Score = answer.Score,
                CreationDate = answer.CreationDate,
                Url = Url.Link(nameof(GetAnswerById), new { answerId = answer.Id })
            };
            return model;
        }

        AnswerModel CreateAnswerModel(Answer answer)
        {
            var model = new AnswerModel
            {
                Score = answer.Score,
                Body = answer.Body,
                CreationDate = answer.CreationDate
            };
            return model;
        }

        //HELPERS
        static int ComputeNumberOfPages(int pageSize, int numberOfItems)
        {
            return (int)Math.Ceiling((double)numberOfItems / pageSize);
        }
        string CreateLink(int page, int pageSize)
        {
            return Url.Link(nameof(GetQuestions), new { page, pageSize });
        }

        string CreateLinkToNextPage(int page, int pageSize, int numberOfPages)
        {
            return page >= numberOfPages - 1
                ? null
                : CreateLink(page = page + 1, pageSize);
        }

        string CreateLinkToPrevPage(int page, int pageSize)
        {
            return page == 0
                ? null
                : CreateLink(page - 1, pageSize);
        }
    }
}
