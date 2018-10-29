using System;
using System.Collections.Generic;
using System.Linq;

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
