using AccountContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class UserDb
    {
        private SMediaContext db;
        public UserDb()
        {
            db = new SMediaContext();
        }
        public IEnumerable<User> GetAll()
        {
            return db.Users.ToList();
        }
        public User GetByID(long Id)
        {
            return db.Users.Find(Id);
        }
        public User GetByUserName(string UserNane)
        {
            return db.GetUserByUserName(UserNane);
        }

        public int Insert(User user)
        {
            db.Users.Add(user);
            return Save();
        }

        public int Delete(long Id)
        {
            User user = GetByID(Id);
            db.Users.Remove(user);
            return Save();
        }

        public int Update(User user)
        {
            db.Entry(user).State = System.Data.Entity.EntityState.Modified;
            db.Configuration.ValidateOnSaveEnabled = false;
            int i = Save();
            db.Configuration.ValidateOnSaveEnabled = true;
            return i;
        }

        public int Save()
        {            
            return db.SaveChanges();
        }
    }
}
