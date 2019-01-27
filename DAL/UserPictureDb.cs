using AccountContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class UserPictureDb
    {
        private SMediaContext db;
        public UserPictureDb()
        {
            db = new SMediaContext();
        }
        public IEnumerable<UserPicture> GetAll()
        {
            return db.UserPictures.ToList();
        }

        public IEnumerable<UserPicture> GetAllByUserID(long Id)
        {
            return db.GetUserPicturesByUserID(Id);
        }

        public UserPicture GetByID(long Id)
        {
            return db.UserPictures.Find(Id);
        }

        public long Insert(UserPicture userPicture)
        {            
            long i = db.CreateUserPicture(userPicture.FileName, userPicture.OriginalFileName, userPicture.ProfilePicture, userPicture.User.ID, DateTime.Now);
            
            return i;
        }

        public int Delete(long Id)
        {
            UserPicture UserPicture = GetByID(Id);
            db.UserPictures.Remove(UserPicture);
            return Save();
        }

        public int Update(UserPicture userPicture)
        {
            db.Entry(userPicture.User).State = System.Data.Entity.EntityState.Unchanged;
            db.Entry(userPicture).State = System.Data.Entity.EntityState.Modified;
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
