using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace ProjectPortfolio2.DatabaseModel
{
    public interface IDataService
    {
        List<Owner> GetOwners();
        Owner GetOwner(int id);
        List<Question> GetQuestions(int page, int pageSize);
        Question GetQuestionById(int id);
        List<User> GetUsers();
        User GetUser(int id);
        List<SearchHistory> GetUserSearchHistory(int userId);
        Comment GetCommentForQuestion(int id);
        Comment GetCommentForAnswer(int id);
        List<Comment> GetCommentsByQuestionId(int questionId);
        List<Comment> GetCommentsByAnswerId(int answerid);
        List<Answer> GetAnswersByQuestionId(int questionId);
        Answer GetAnswer(int id);
        List<Tag> GetTagsByQuestionId(int questionId);
        Tag GetTag(int id);
        User CreateUser(string email, string password, string name, string location);
        User UpdateUser(int UserId, string Email, string Pwd, string Name, string Location);
        bool DeleteUser(int UserId);
        int GetNumberOfQuestions();
        int GetNumberOfAnswersForQuestion(int questionId);
        List<PostMarked> GetMarkedQuestions(int userId);
        PostMarked UserMarkPost(int PostId, int UserId, string Annotation);
        PostMarked UserUpdateMarkedPost(int PostId, int UserId, string Annotation);
        bool UserUnmarkPost(int PostId, int UserId);
        CommentMarked UserMarkComment(int CommentId, int UserId, string Annotation);
        List<CommentMarked> GetMarkedComments(int userId);
        CommentMarked UserUpdateCommentAnnotation(int CommentId, int UserId, string AnnotationText);
        bool UserUnmarkComment(int CommentId, int UserId);
        List<SearchPostsResult> SearchPosts(string searchText, int userId);
        List<Question> GetQuestionForIds(List<int> ids);
    }
    public class DataService : IDataService
    {

        public List<Owner> GetOwners()
        {
            using (var db = new DatabaseContext())
            {
                //TODO this is just a testing query
                return db.Owners.Take(5).ToList();
            }
        }

        public Owner GetOwner(int id)
        {
            using (var db = new DatabaseContext())
            {
                return db.Owners.Find(id);
            }
        }

        public List<Answer> GetAnswersByQuestionId(int questionId)
        {
            using (var db = new DatabaseContext())
            {
                return db.Answers
                         .Where(x => x.QuestionId.Equals(questionId))
                         .ToList();
            }
        }

        public Answer GetAnswer(int id)
        {
            using (var db = new DatabaseContext())
            {
                return db.Answers.Find(id);
            }
        }

        public int GetNumberOfQuestions()
        {
            using (var db = new DatabaseContext())
            {
                return db.Questions.Count();
            }
        }

        public int GetNumberOfAnswersForQuestion(int questionId)
        {
            using (var db = new DatabaseContext())
            {
                return db.Answers.Where(x => x.QuestionId.Equals(questionId)).Count();

            }
        }

            public List<Question> GetQuestions(int page, int pageSize)
        {
            using (var db = new DatabaseContext())
            {
                return db.Questions
                         .OrderByDescending(x => x.Score)
                         .Skip(page * pageSize)
                         .Take(pageSize)
                         .ToList();
            }
        }

        public Question GetQuestionById(int id)
        {
            using (var db = new DatabaseContext())
            {
                var question = db.Questions.Find(id);
                if (question != null)
                {
                    question.Answers = db.Answers.Where(x => x.QuestionId.Equals(id)).ToList();
                    question.Comments = db.Comments.Where(x => x.PostId.Equals(id)).ToList();
                }
                return question;
            }
        }

        public List<User> GetUsers()
        {
            using (var db = new DatabaseContext())
            {
                return db.Users.ToList();
            }
        }

        public User GetUser(int id)
        {
            using (var db = new DatabaseContext())
            {
                var user = db.Users.Find(id);
                return user;
            }
        }

        public List<SearchHistory> GetUserSearchHistory(int userId)
        {
            using (var db = new DatabaseContext())
            {
                return db.SearchHistories.Where(s => s.UserId.Equals(userId)).ToList();
            }
        }

        public List<Comment> GetCommentsByAnswerId(int answerId)
        {
            return GetCommentsByPostId(answerId);
        }

        public List<Comment> GetCommentsByQuestionId(int questionId)
        {
            return GetCommentsByPostId(questionId);
        }

        public List<Comment> GetCommentsByPostId(int postId)
        {
            using (var db = new DatabaseContext())
            {
                return db.Comments.Where(x => x.PostId.Equals(postId)).ToList();
            }
        }

        /*
         * TODO if there is enough time investigate this issue!!
         * need separate function due to weird server error when using same function 
         * for gettting comment in answers and questions controller
         * Attribute routes with the same name 'GetCommentsByPostId' must have the same template:
         * Action: 'WebService.Controllers.AnsweCommentsController.GetCommentsByPostId (WebService)' - Template: 'api/posts/{postId}/answers/{answerId}/comments'
         * Action: 'WebService.Controllers.CommentsController.GetCommentsByPostId (WebService)' - Template: 'api/posts/{postId}/comments'
         */
        public Comment GetCommentForQuestion(int id)
        {
            return GetComment(id);
        }

        public Comment GetCommentForAnswer(int id)
        {
            return GetComment(id);
        }

        public Comment GetComment(int id)
        {
            using (var db = new DatabaseContext())
            {
                return db.Comments.Find(id);
            }

        }

        public List<Tag> GetTagsByQuestionId(int questionId)
        {
            using (var db = new DatabaseContext())
            {
                var postTagTagIds = db.PostTags.Where(x => x.PostId.Equals(questionId)).Select(x => x.TagId).ToList();
                return db.Tags.Where(x => postTagTagIds.Contains(x.Id)).ToList();
            }
        }

        public Tag GetTag(int tagId)
        {
            using (var db = new DatabaseContext())
            {
                return db.Tags.Find(tagId);
            }
        }


        public User CreateUser(string Mail, string Pwd, string Name, string Location)
        {
            using (var db = new DatabaseContext())
            {
                foreach (var result in db.Users.FromSql("select * from create_user({0}, {1}, {2}, {3})", Mail, Pwd, Name, Location))
                {
                    Console.WriteLine($"User created, with id: {result.Id}, Name: {result.Name}, Mail: {result.Email}");
                    return new User
                    {
                        Id = result.Id,
                        Name = result.Name,
                        Email = result.Email,
                        Location = result.Location,
                        Password = result.Password
                    };
                }
            }return null;
        }

        public bool DeleteUser(int UserId) 
        {
            using (var db = new DatabaseContext())
            {
                var usr = db.Users.Find(UserId);
                if (usr != null)
                {
                    foreach (var result in db.Users.FromSql("select * from delete_user({0})", UserId))
                    {
                        Console.WriteLine($"Delted: {result.Name}, id: {result.Id} has been deleted");
                        return true;
                    }
                }return false;


            }
        }

        public User UpdateUser(int UserId, string Email, string Pwd, string Name, string Location)
        {
            using (var db = new DatabaseContext())
            {
                var usr = db.Users.Find(UserId);
                if (usr != null && Email != null && Pwd != null && Name != null)
                {
                    foreach (var result in db.Users.FromSql("select * from update_user({0},{1},{2},{3},{4})", UserId, Email, Pwd, Name, Location))
                    {
                        Console.WriteLine($"user with id: {result.Id}, updated. New info: {result.Name},{result.Email},{result.Location}");
                        return new User
                        {
                            Id = result.Id,
                            Name = result.Name,
                            Email = result.Email,
                            Location = result.Location,
                            Password = result.Password
                        };
                    }
                }

            }return null;
        }

        //public List<SearchHistory> GetUserSearchHistory(int UserId)
        //{
        //    using (var db = new DatabaseContext())
        //    {
        //        var usr = db.Users.Find(UserId);
        //        if (usr != null)
        //        {
        //            List<SearchHistory> UserSearchList = new List<SearchHistory>();
        //            foreach (var result in db.SearchHistories.FromSql("select * from get_user_search_history({0})", UserId))
        //            {
        //                Console.WriteLine($"Searched: {result.Id}, {result.Searchtext}, {result.CreationDate}");
        //                UserSearchList.Add(
        //                    new SearchHistory
        //                    {
        //                        Id = result.Id,
        //                        Searchtext = result.Searchtext,
        //                        CreationDate = result.CreationDate
        //                    }
        //                );
        //            }
        //            return UserSearchList;
        //        }

        //    }return null;
        //}

        public List<SearchPostsResult> SearchPosts(string SearchString, int UserId)
        {
            using (var db = new DatabaseContext())
            {
                List<SearchPostsResult> results = new List<SearchPostsResult>();
                foreach (var result in db.SearchPostsResults.FromSql("select * from search_posts({0}, {1})", SearchString, UserId))
                {
                    results.Add(new SearchPostsResult
                    {
                        Id = result.Id,
                        Title = result.Title,
                        CreationDate = result.CreationDate,
                        ParentId = result.ParentId,
                        Score = result.Score
                    });
                }
                return results;
            }
        }
    
        public CommentMarked UserMarkComment(int CommentId, int UserId, string Annotation) 
        {
            using (var db = new DatabaseContext())
            {
                var usr = db.Users.Find(UserId);
                var ann = db.Comments.Find(CommentId);
                if (usr != null && ann != null)
                {
                    foreach (var result in db.CommentsMarked.FromSql("select * from user_mark_comment({0}, {1}, {2})",
                                    CommentId, UserId, Annotation))
                    {
                        Console.WriteLine($"Comment id({result.CommentId}) by user({result.UserId}) marked with annotation: {result.AnnotationText}");
                        return new CommentMarked
                        {
                            CommentId = result.CommentId,
                            UserId = result.UserId,
                            AnnotationText = result.AnnotationText
                        };
                    }
                }
            }return null;
        }

        public CommentMarked UserUpdateCommentAnnotation(int CommentId, int UserId, string Annotation)
        {
            using (var db = new DatabaseContext())
            {
                var usr = db.CommentsMarked.Find(CommentId, UserId);
                if (usr != null)
                {
                    foreach (var result in db.CommentsMarked.FromSql("select * from update_annotation_to_marked_comment({0}, {1}, {2})",
                                               CommentId, UserId, Annotation))
                    {
                        Console.WriteLine($"Comment id({result.CommentId}) by user({result.UserId}) updated annontaiton: {result.AnnotationText}");
                        return new CommentMarked
                        {
                            CommentId = result.CommentId,
                            UserId = result.UserId,
                            AnnotationText = result.AnnotationText
                        };
                    }
                }
            }return null;
        }

        public bool UserUnmarkComment(int CommentId, int UserId)
        {
            using (var db = new DatabaseContext())
            {
                var usr = db.CommentsMarked.Find(CommentId, UserId);
                if (usr != null)
                {
                    foreach (var result in db.CommentsMarked.FromSql("select * from user_unmark_comment({0}, {1})", CommentId, UserId))
                    {
                        Console.WriteLine($"Comment {result.CommentId}, unmarked by user: {result.UserId}");
                        return true;
                    }

                }
                return false;
            }
        }


        public PostMarked UserMarkPost(int PostId, int UserId, string Annotation) //missing something for when a comment already is marked and you try to do it again
        {
            using (var db = new DatabaseContext())
            {
                foreach (var result in db.PostsMarked.FromSql("select * from user_mark_post({0}, {1}, {2})",
                                                              PostId, UserId, Annotation))
                {
                    Console.WriteLine($"Post id({result.PostId}) by user({result.UserId}) marked with annotation: {result.AnnotationText}");
                    return new PostMarked
                    {
                        PostId = result.PostId,
                        UserId = result.UserId,
                        AnnotationText = result.AnnotationText
                    };
                }
            }
            return null;
        }

        public PostMarked UserUpdateMarkedPost(int PostId, int UserId, string Annotation)
        {
            using (var db = new DatabaseContext())
            {
                //check for marked post
                var markedPost = db.PostsMarked.Find(PostId, UserId);
                if (markedPost != null)
                {
                    //check if post and user exists
                    var post = db.Questions.Find(PostId);
                    var user = db.Users.Find(UserId);
                    if (user == null || post == null) return null;
                    foreach (var result in db.PostsMarked.FromSql("select * from update_annotation_to_marked_post({0}, {1}, {2})",
                                                              PostId, UserId, Annotation))
                    {
                        return new PostMarked
                        {
                            PostId = result.PostId,
                            UserId = result.UserId,
                            AnnotationText = result.AnnotationText
                        };
                    }
                }
                return markedPost;
            }
        }

        //linq + if statement return boolean
        public bool UserUnmarkPost(int PostId, int UserId)
        {
            using (var db = new DatabaseContext())
            {
                var markedPost = db.PostsMarked.Find(PostId, UserId);
                if (markedPost == null) return false;
                foreach (var result in db.PostsMarked.FromSql("select * from user_unmark_post({0}, {1})",
                                                              PostId, UserId))
                {
                    Console.WriteLine($"Post id: {result.PostId} unmarked by user: {result.UserId}");
                    return true;
                }
            }
            return false;
        }

        public List<PostMarked> GetMarkedQuestions(int userId)
        {
            using (var db = new DatabaseContext())
            {
                return db.PostsMarked.Where(x => x.UserId.Equals(userId)).ToList();
            }
        }

        public List<CommentMarked> GetMarkedComments(int userId)
        {
            using (var db = new DatabaseContext())
            {
                return db.CommentsMarked.Where(x => x.UserId.Equals(userId)).ToList();
            }
        }

        public List<Question> GetQuestionForIds(List<int> ids)
        {
            using (var db = new DatabaseContext())
            {
                return db.Questions.Where(x => ids.Contains(x.Id)).ToList();
            }
        }

    }
}
