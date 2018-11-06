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
        List<Question> GetPosts();
        Question GetPostById(int id);
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

        public List<Question> GetPosts()
        {
            using (var db = new DatabaseContext())
            {
                return db.Questions.Take(10).ToList();
            }
        }

        public Question GetPostById(int id)
        {
            using (var db = new DatabaseContext())
            {
                return db.Questions.Find(id);
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

        /*
        public List<SearchHistory> GetUserSearchHistory(int userId)
        {
            using (var db = new DatabaseContext())
            {
                return db.SearchHistories.Where(s => s.UserId.Equals(userId)).ToList();
            }
        }
        */

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
                        Email = result.Email

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
                if (usr != null)
                {
                    foreach (var result in db.Users.FromSql("select * from update_user({0},{1},{2},{3},{4})", UserId, Email, Pwd, Name, Location))
                    {
                        Console.WriteLine($"user with id: {result.Id}, updated. New info: {result.Name},{result.Email},{result.Location}");
                        return new User
                        {
                            Id = result.Id,
                            Name = result.Name,
                            Email = result.Email,
                            Location = result.Location
                        };
                    }
                }

            }return null;
        }

        public List<SearchHistory> GetUserSearchHistory(int UserId)
        {
            using (var db = new DatabaseContext())
            {
                var usr = db.Users.Find(UserId);
                if (usr != null)
                {
                    List<SearchHistory> UserSearchList = new List<SearchHistory>();
                    foreach (var result in db.SearchHistories.FromSql("select * from get_user_search_history({0})", UserId))
                    {
                        Console.WriteLine($"Searched: {result.Id}, {result.Searchtext}, {result.CreationDate}");
                        UserSearchList.Add(
                            new SearchHistory
                            {
                                Id = result.Id,
                                Searchtext = result.Searchtext,
                                CreationDate = result.CreationDate
                            }
                        );
                    }
                    return UserSearchList;

                }

            }return null;
        }

        public SearchPostsResult SearchPosts(string SearchString, int UserId)
        {
            using (var db = new DatabaseContext())
            {
                foreach (var result in db.SearchPostsResults.FromSql("select * from search_posts({0}, {1})", SearchString, UserId))
                {
                    Console.WriteLine($"Result: {result.Id}, {result.Title}, {result.CreationDate}");
                    return new SearchPostsResult
                    {
                        Id = result.Id,
                        Title = result.Title,
                        CreationDate = result.CreationDate
                    };
                }
            }
            return null;
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

        //this also needs an linq + if statement???? if there is no comment to update
        public CommentMarked UserUpdateCommentAnnotation(int CommentId, int UserId, string Annotation) //missing something for when a comment already is marked and you try to do it again
        {
            using (var db = new DatabaseContext())
            {
                var usr = db.CommentsMarked.Find(CommentId, UserId);
                //var cmt = db.CommentsMarked.Find(CommentId);
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

        public PostMarked UserUpdatePost(int PostId, int UserId, string Annotation) //missing something for when a comment already is marked and you try to do it again
        {
            using (var db = new DatabaseContext())
            {//check for marked post
                foreach (var result in db.PostsMarked.FromSql("select * from update_annotation_to_marked_post({0}, {1}, {2})",
                                                              PostId, UserId, Annotation))
                {
                    Console.WriteLine($"Post id({result.PostId}) by user({result.UserId}) updated annotation: {result.AnnotationText}");
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

        //linq + if statement return boolean
        public void UserUnmarkPost(int PostId, int UserId)
        {
            using (var db = new DatabaseContext())
            {
                foreach (var result in db.PostsMarked.FromSql("select * from user_unmark_post({0}, {1})",
                                                              PostId, UserId))
                {
                    Console.WriteLine($"Post id: {result.PostId} unmarked by user: {result.UserId}");
                }
            }
        }





    }
}
