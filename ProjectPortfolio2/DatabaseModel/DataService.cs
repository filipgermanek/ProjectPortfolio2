using System;
using System.Collections.Generic;
using System.Linq;
using Npgsql;

namespace ProjectPortfolio2.DatabaseModel
{
    public interface IDataService
    {
        List<Owner> GetOwners();
        Owner GetOwner(int id);
        List<Question> GetQuestions();
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
                return db.Answers.Where(x => x.QuestionId.Equals(questionId)).ToList();
            }
        }

        public Answer GetAnswer(int id)
        {
            using (var db = new DatabaseContext())
            {
                return db.Answers.Find(id);
            }
        }

        public List<Question> GetQuestions()
        {
            using (var db = new DatabaseContext())
            {
                return db.Questions.Take(10).ToList();
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
                return db.Users.Find(id);
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

        public static string ConnectionString =
            "host=localhost;db=stackoverflow;uid=filipgermanek;pwd=GRuby123";

        public User CreateUser(string email, string password, string name, string location)
        {
            using (var conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "select * from create_user(@4)";
                    cmd.Parameters.AddWithValue("@1", email);
                    cmd.Parameters.AddWithValue("@2", password);
                    cmd.Parameters.AddWithValue("@3", name);
                    cmd.Parameters.AddWithValue("@4", location);
                    Console.WriteLine("email " + email + " pass " + password + " name " + name + " location " + location);
                    using (var reader = cmd.ExecuteReader())
                    {
                        //while (reader.Read())
                        //{
                        //    Console.WriteLine($"Result(EF): {reader.GetInt32(0)}, {reader.GetString(1)}");
                        //}
                        Console.WriteLine("in reader");
                        //TODO this needs to return actual user returned by function
                        User user = new User
                        {
                            Name = name,
                            Location = location,
                            Password = password,
                            Email = email
                        };
                        return user;
                    }

                }
            }
        }
    }
}
