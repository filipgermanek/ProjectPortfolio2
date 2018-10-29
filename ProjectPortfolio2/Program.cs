using System;
using System.Collections.Generic;
using System.Linq;
using ProjectPortfolio2.DatabaseModel;

namespace ProjectPortfolio2
{
    class Program
    {
        static void Main(string[] args)
        {

            var comments = GetCommments();
            foreach (var c in comments){
                Console.WriteLine("score: " + c.Score + " post id: " + c.PostId);
                Console.WriteLine();
            }

        }

        private static List<Comment> GetCommments()
        {
            using (var db = new DatabaseContext())
            {
                return (from c in db.Comments 
                        where c.Score > 500
                        select c).ToList();
            }
        }
    }
}
