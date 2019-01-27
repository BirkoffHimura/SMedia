using AccountContext;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class UserPostCommentBs
    {
        private UserPostCommentDb db;
        public UserPostCommentBs()
        {
            db = new UserPostCommentDb();
        }
        public IEnumerable<UserPostComment> GetAll()
        {
            return db.GetAll();
        }
        public IEnumerable<UserPostComment> GetAllByPostID(long Id)
        {
            return db.GetAllByPostID(Id);
        }
        public IEnumerable<UserPostComment> GetAllByPostIDWithUserData(long Id)
        {
            return db.GetAllByPostIDWithUserData(Id);
        }
        public UserPostComment GetByID(long Id)
        {
            return db.GetByID(Id);
        }

        public long Insert(UserPostComment userPostComment)
        {
            return db.Insert(userPostComment);
        }

        public int Delete(long Id)
        {
            return db.Delete(Id);
        }

        public int Update(UserPostComment userPostComment)
        {
            return db.Update(userPostComment);
        }
    }
}
