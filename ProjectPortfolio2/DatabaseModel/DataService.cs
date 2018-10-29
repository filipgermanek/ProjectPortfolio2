using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectPortfolio2.DatabaseModel
{
    public interface IDataService
    {
        List<Owner> GetOwners();
        Owner GetOwner(int id);
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
