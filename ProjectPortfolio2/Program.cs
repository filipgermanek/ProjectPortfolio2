using ProjectPortfolio2.DatabaseModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectPortfolio2
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            var owners = GetOwners();
            foreach(var o in owners)
            {
                Console.WriteLine("owner name: " + o.DisplayName);
            }
            */
            var posts = GetPosts();
            foreach (var p in posts)
            {
                Console.WriteLine("post id: " + p.PostId);
            }

           

        }

        /*
        private static List<Owner> GetOwners()
        {
            using (var db = new DatabaseContext())
            {
                return (from o in db.Owners select o).ToList();
            }
        }
        */
        private static List<Post> GetPosts()
        {
            using (var db = new DatabaseContext())
            {
                return (from p in db.Posts select p).ToList();
            }
           

       }
  
    }
}
