using AccountContext;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class UserPostDb
    {
        private SMediaContext db;
        public UserPostDb()
        {
            db = new SMediaContext();
        }
        public IEnumerable<UserPost> GetAll()
        {
            return db.UserPosts.ToList();
        }
        public IEnumerable<UserPost> GetAllWithUserData()
        {
            return db.UserPosts
                .Include(x => x.User)
                .Include(k => k.UserPicture)
                .ToList();
        }

        public IEnumerable<UserPost> GetAllByUserID(long Id)
        {
            return db.GetUserPostsByUserID(Id);
        }

        public IEnumerable<UserPost> GetAllByUserIDWithUserData(long Id)
        {
            var coms = db.UserPosts.AsNoTracking()
                .Where(x => x.User.ID == Id)
                .Include(com => com.User)
                .Include(x => x.UserPicture)
                .ToList();
            return coms;
        }

        public UserPost GetByID(long Id)
        {
            return db.UserPosts.Find(Id);
        }

        public long Insert(UserPost userPost)
        {
            object picID = userPost.UserPicture == null ? null : (object)userPost.UserPicture.PictureID;


            //db.Entry(userPost.User).State = System.Data.Entity.EntityState.Unchanged;            
            //db.UserPosts.Add(userPost);
            //db.Configuration.ValidateOnSaveEnabled = false;
            long i = db.CreatePost(userPost.Subject, userPost.Post_Body, userPost.Img_Extern, picID, userPost.User.ID, userPost.PostDate.ToString());
            //db.Configuration.ValidateOnSaveEnabled = true;
            return i;
        }

        public int Delete(long Id)
        {
            UserPost userPost = GetByID(Id);
            db.UserPosts.Remove(userPost);
            return Save();
        }

        public int Update(UserPost userPost)
        {
            db.Entry(userPost).State = System.Data.Entity.EntityState.Modified;
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
