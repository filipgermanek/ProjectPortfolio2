using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectPortfolio2.DatabaseModel
{
    public interface IDataService
    {
        List<Owner> GetOwners();
        Owner GetOwner(int id);
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
    }
}
