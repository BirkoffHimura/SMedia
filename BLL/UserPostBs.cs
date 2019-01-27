using AccountContext;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class UserPostBs
    {
        private UserPostDb db;
        public UserPostBs()
        {
            db = new UserPostDb();
        }
        public IEnumerable<UserPost> GetAll()
        {
            return db.GetAll();
        }
        public IEnumerable<UserPost> GetAllWithUserData()
        {
            return db.GetAllWithUserData();
        }
        public IEnumerable<UserPost> GetAllByUserID(long Id)
        {
            return db.GetAllByUserID(Id);
        }
        public IEnumerable<UserPost> GetAllByUserIDWithUserData(long Id)
        {
            return db.GetAllByUserIDWithUserData(Id);
        }
        public UserPost GetByID(long Id)
        {
            return db.GetByID(Id);
        }

        public long Insert(UserPost UserPost)
        {
            return db.Insert(UserPost);
        }

        public int Delete(long Id)
        {
            return db.Delete(Id);
        }

        public int Update(UserPost UserPost)
        {
            return db.Update(UserPost);
        }
    }
}
