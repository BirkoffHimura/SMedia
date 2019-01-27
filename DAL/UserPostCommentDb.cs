using AccountContext;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class UserPostCommentDb
    {
        private SMediaContext db;
        public UserPostCommentDb()
        {
            db = new SMediaContext();
        }
        public IEnumerable<UserPostComment> GetAll()
        {
            return db.UserPostComments.ToList();
        }
        public IEnumerable<UserPostComment> GetAllByPostID(long Id)
        {
            return db.GetUserPostCommentByPostID(Id);
        }

        public IEnumerable<UserPostComment> GetAllByPostIDWithUserData(long Id)
        {
            var coms = db.UserPostComments.AsNoTracking()
                .Where(x => x.UserPost.ID == Id)
                .Include(com => com.FromUser)
                .ToList();
            return coms;
        }

        public UserPostComment GetByID(long Id)
        {
            return db.UserPostComments.Find(Id);
        }

        public long Insert(UserPostComment userPostComment)
        {
            long id = db.CreateUserPostComment(userPostComment.Comment_Body, userPostComment.CommentDate, userPostComment.FromUser.ID, userPostComment.UserPost.ID);
            return id;
        }

        
        public int Delete(long Id)
        {
            UserPostComment UserPostComment = GetByID(Id);
            db.UserPostComments.Remove(UserPostComment);
            return Save();
        }

        public int Update(UserPostComment UserPostComment)
        {
            db.Entry(UserPostComment).State = System.Data.Entity.EntityState.Modified;
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
