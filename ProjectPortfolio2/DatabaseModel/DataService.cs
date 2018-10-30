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
        List<Post> GetPosts();
        Post GetPostById(int id);
        List<User> GetUsers();
        User GetUser(int id);
        List<SearchHistory> GetUserSearchHistory(int userId);
        Comment GetComment(int id);
        List<Comment> GetComments();
        List<Answer> GetAnswersByParentId(int parentId);
        Answer GetAnswer(int id);
        User CreateUser(string email, string password, string name, string location);
    }
    public class DataService : IDataService
    {
        public static string ConnectionString =
            "host=localhost;db=stackoverflow;uid=filipgermanek;pwd=GRuby123";

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

        public List<Answer> GetAnswersByParentId(int parentId)
        {
            using (var db = new DatabaseContext())
            {
                return db.Answers.Where(x => x.ParentId.Equals(parentId)).ToList();
            }
        }

        public Answer GetAnswer(int id)
        {
            using (var db = new DatabaseContext())
            {
                return db.Answers.Find(id);
            }
        }

        public List<Post> GetPosts ()
        {
            using (var db = new DatabaseContext())
            {
                return db.Posts.Take(10).ToList();
            }
        }

        public Post GetPostById(int id)
        {
            using (var db = new DatabaseContext())
            {
                return db.Posts.Find(id);
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

        public List<Comment> GetComments()
        {
            using (var db = new DatabaseContext())
            {
                return db.Comments.Take(5).ToList();
            }
        }

        public Comment GetComment(int id)
        {
            using (var db = new DatabaseContext())
            {
                return db.Comments.Find(id);
            }

        }

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

        /*
        public List<CommentMarked> GetCommentsMarked(int id)
        {
            using (var db = new DatabaseContext())
            {
                return db.CommentsMarked.Take(5).ToList();
        
            }
        }
        public CommentMarked GetCommentMarked(int id)
        {
            using (var db = new DatabaseContext())
            {
                return db.CommentsMarked.Find(id);
            }
        }

    */




    }
}
