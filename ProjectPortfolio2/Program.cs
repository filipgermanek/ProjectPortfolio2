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
