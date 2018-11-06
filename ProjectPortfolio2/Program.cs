using System;
using System.Collections.Generic;
using System.Linq;
using ProjectPortfolio2.DatabaseModel;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace ProjectPortfolio2
{
    class Program
    {
        static void Main(string[] args)
        {
            Program program = new Program();

            //program.CreateUser("nicolaisen@ja.dk", "mitpsdw", "MitNavn", "Danmark");
            //program.DeleteUser(4);
            //program.UpdateUser(5, "email@mail.com", "pwd2", "nytNavn", "Dk" );

            //program.GetUserSearchHistory(5);
            //program.SearchPosts("always", 4);

            //program.UserMarkComment(9158912, 5, "this is alsoooooddsfdg marked");
            //program.UserUpdateCommentAnnotation(9158912, 5, "updated agaiinnananan");
            //program.UserUnmarkComment(9158912, 5);


            program.UserMarkPost(22106846, 1, "hej post du marked", true);
            //UserUpdatePost(22525799, 4, "nnyyyyyy");
            //UserUnmarkPost(22525799, 4); 
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

            }
            return null;
        }

        public SearchPostsResult SearchPosts(string SearchString, int UserId)
        {
            using (var db = new DatabaseContext())
            {
                // you can add parameters to the query, as shown here, by list them after the 
                // statement, and reference them with {0} {1} ... {n}, where 0 is the first argument,
                // 1 is the second etc.
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
            }return null;
        }
        //check if comment and user != null, if null return null if not run 
        public CommentMarked UserMarkComment(int CommentId, int UserId, string Annotation) //missing something for when a comment already is marked and you try to do it again
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

        //check if annotation id and user id exists
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

        //linq + if statement return boolean
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

                }return false;
            }
        }


        public PostMarked UserMarkPost(int PostId, int UserId, string Annotation, bool IsQuestion) //missing something for when a comment already is marked and you try to do it again
        {
            using (var db = new DatabaseContext())
            {
                bool DoPostExist = false;

                if (IsQuestion){
                    var question = db.Questions.Find(PostId);
                    DoPostExist = question != null;
                }else{
                    var answer = db.Answers.Find(PostId);
                    DoPostExist = answer != null;
                }

                var usr = db.Users.Find(UserId);
                if (usr != null && DoPostExist)
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
            }return null;
        }

        public PostMarked UserUpdatePost(int PostId, int UserId, string Annotation) //missing something for when a comment already is marked and you try to do it again
        {
            using (var db = new DatabaseContext())
            {
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
            }return null;
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
