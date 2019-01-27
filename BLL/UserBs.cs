using AccountContext;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class UserBs
    {
        private UserDb db;
        public UserBs()
        {
            db = new UserDb();
        }
        public IEnumerable<User> GetAll()
        {
            return db.GetAll();
        }
        public User GetByID(long Id)
        {
            return db.GetByID(Id);
        }

        public User GetByUserName(string UserName)
        {
            return db.GetByUserName(UserName);
        }

        public int Insert(User user)
        {            
            return db.Insert(user);
        }

        public int Delete(long Id)
        {
            return db.Delete(Id);
        }

        public int Update(User user)
        {
            return db.Update(user);
        }
    }
}
