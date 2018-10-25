using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectPortfolio2
{
    class Program
    {
        static void Main(string[] args)
        {
            var owners = GetOwners();
            foreach(var o in owners)
            {
                Console.WriteLine("owner name: " + o.DisplayName);
            }
        }

        private static List<Owner> GetOwners()
        {
            using (var db = new DatabaseContext())
            {
                return (from o in db.Owners select o).ToList();
            }
        }
    }
}
