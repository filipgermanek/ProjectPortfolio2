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
            var tags = GetTags();
            foreach(var o in tags)
            {
                Console.WriteLine("tag name: " + o.Name);
            }
        }

        static List<Tag> GetTags()
        {
            using (var db = new DatabaseContext())
            {
                return (from t in db.Tags select t).ToList();
            }
        }
    }
}
