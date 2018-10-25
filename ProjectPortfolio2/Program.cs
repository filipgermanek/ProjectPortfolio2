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


            var comments = GetCommments();
            foreach(var c in comments)
            {
                Console.WriteLine("comment id: " + c.Id + "comment: " + c.Text);
            }

        }



        private static List<Owner> GetOwners()
        {
            using (var db = new DatabaseContext())
            {
                return (from o in db.Owners select o).ToList();
            }
        }


        private static List<Comment> GetCommments()
        {
            using (var db = new DatabaseContext())
            {
                return (from c in db.Comments select c).ToList();
            }
        }
    }
}
