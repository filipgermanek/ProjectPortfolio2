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
            //CreateUser("nicolaisen@ja.dk", "mitpsdw", "MitNavn", "Danmark");
            //DeleteUser(2);
            //UpdateUser(4, "email@mail.com", "pwd2", "nytNavn", "Dk" );

            //GetUserSearchHistory(4);
            SearchPosts("always", 4);

            //UserMarkComment(35996338, 2, "this is alsoooooddsfdg marked");
            //UserUnmarkComment(35996338, 2);
            //UserUpdateCommentAnnotation(35996338, 2, "updated agaiinnananan");
            //UserMarkPost(22525799, 4, "hej post du marked");
            //UserUpdatePost(22525799, 4, "nnyyyyyy");
            //UserUnmarkPost(22525799, 4); 
        }

        private static User CreateUser(string Mail, string Pwd, string Name, string Location)
        {
            using (var db = new DatabaseContext())
            {
                foreach(var result in db.Users.FromSql("select * from create_user({0}, {1}, {2}, {3})", Mail, Pwd, Name, Location))
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

        private static void DeleteUser(int UserId) //use linq to determine if user exists return boolean
        {
            using (var db = new DatabaseContext())
            {
                foreach (var result in db.Users.FromSql("select * from delete_user({0})", UserId))
                {
                    Console.WriteLine($"user: {result.Name}, id: {result.Id} has been deleted");

                }
            }
        }

        private static User UpdateUser(int UserId, string Email, string Pwd, string Name, string Location)
        {
            using (var db = new DatabaseContext())
            {
                foreach(var result in db.Users.FromSql("select * from update_user({0},{1},{2},{3},{4})", UserId, Email, Pwd, Name, Location))
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
            }return null;
        }

        private static SearchHistory GetUserSearchHistory(int UserId)
        {
            using (var db = new DatabaseContext())
            {
                foreach (var result in db.SearchHistories.FromSql("select * from get_user_search_history({0})", UserId))
                {
                    Console.WriteLine($"Searched: {result.Id}, {result.Searchtext}, {result.CreationDate}");
                    return new SearchHistory
                    {
                        Id = result.Id,
                        Searchtext = result.Searchtext,
                        CreationDate = result.CreationDate
                    };
                }
            }return null;
        }

        private static SearchPostsResult SearchPosts(string SearchString, int UserId)
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

        private static CommentMarked UserMarkComment(int CommentId, int UserId, string Annotation) //missing something for when a comment already is marked and you try to do it again
        {
            using (var db = new DatabaseContext())
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
            }return null;   
        }

        //this also needs an linq + if statement???? if there is no comment to update
        private static CommentMarked UserUpdateCommentAnnotation(int CommentId, int UserId, string Annotation) //missing something for when a comment already is marked and you try to do it again
        {
            using (var db = new DatabaseContext())
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
            }return null;
        }

        //linq + if statement return boolean
        private static void UserUnmarkComment(int CommentId, int UserId)
        {
            using (var db = new DatabaseContext())
            {
                foreach (var result in db.CommentsMarked.FromSql("select * from user_unmark_comment({0}, {1})", CommentId, UserId))
                {
                    Console.WriteLine($"Comment {result.CommentId}, unmarked by user: {result.UserId}");
                }
            }
        }


        private static PostMarked UserMarkPost(int PostId, int UserId, string Annotation) //missing something for when a comment already is marked and you try to do it again
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
            }return null;
        }

        private static PostMarked UserUpdatePost(int PostId, int UserId, string Annotation) //missing something for when a comment already is marked and you try to do it again
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
        private static void UserUnmarkPost(int PostId, int UserId) 
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
