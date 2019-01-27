using AccountContext;
using DAL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class UserPictureBs
    {
        private UserPictureDb db;
        public UserPictureBs()
        {
            db = new UserPictureDb();
        }
        public IEnumerable<UserPicture> GetAll()
        {
            return db.GetAll();
        }

        public IEnumerable<UserPictureBase64> GetAllBase64(string filePath)
        {
            var ret = db.GetAll();
            List<UserPictureBase64> upb64 = new List<UserPictureBase64>();
            try
            {
                foreach (UserPicture userPicture in ret)
                {
                    string base64String;
                    string fileName = Path.Combine(filePath, userPicture.FileName);
                    Bitmap bitmap = new Bitmap(fileName);
                    ImageFormat fmt = ImageFormat.Png;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        bitmap.Save(ms, fmt);
                        base64String = Convert.ToBase64String(ms.ToArray());
                    }
                    UserPictureBase64 tmp = new UserPictureBase64()
                    {
                        PictureID = userPicture.PictureID,
                        FileName = userPicture.FileName,
                        ProfilePicture = userPicture.ProfilePicture,
                        UploadDate = userPicture.UploadDate,
                        Base64ImageString = base64String,
                        User = userPicture.User,
                        OriginalFileName = userPicture.OriginalFileName
                    };
                    upb64.Add(tmp);
                }
            }
            catch { }
            return upb64;
        }

        public IEnumerable<UserPicture> GetAllByUserID(long Id)
        {
            return db.GetAllByUserID(Id);
        }

        public IEnumerable<UserPictureBase64> GetAllByUserIDBase64(long Id, string filePath)
        {
            var ret = db.GetAllByUserID(Id);
            List<UserPictureBase64> upb64 = new List<UserPictureBase64>();
            try
            {
                foreach (UserPicture userPicture in ret)
                {
                    string base64String;
                    string fileName = Path.Combine(filePath, userPicture.FileName);
                    Bitmap bitmap = new Bitmap(fileName);
                    ImageFormat fmt = ImageFormat.Png;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        bitmap.Save(ms, fmt);
                        base64String = Convert.ToBase64String(ms.ToArray());
                    }
                    UserPictureBase64 tmp = new UserPictureBase64()
                    {
                        PictureID = userPicture.PictureID,
                        FileName = userPicture.FileName,
                        ProfilePicture = userPicture.ProfilePicture,
                        UploadDate = userPicture.UploadDate,
                        Base64ImageString = base64String,
                        User = userPicture.User,
                        OriginalFileName = userPicture.OriginalFileName
                    };
                    upb64.Add(tmp);
                }
            }
            catch { }
            return upb64;
        }

        public UserPicture GetByID(long Id)
        {
            return db.GetByID(Id);
        }
        public UserPictureBase64 GetByIDBase64(long Id, string filePath)
        {
            var ret = db.GetByID(Id);
            
            string base64String = string.Empty;
            try
            {
                string fileName = Path.Combine(filePath, ret.FileName);
                Bitmap bitmap = new Bitmap(fileName);
                ImageFormat fmt = ImageFormat.Png;
                using (MemoryStream ms = new MemoryStream())
                {
                    bitmap.Save(ms, fmt);
                    base64String = Convert.ToBase64String(ms.ToArray());
                }
            }
            catch { }
            UserPictureBase64 tmp = new UserPictureBase64()
            {
                PictureID = ret.PictureID,
                FileName = ret.FileName,
                ProfilePicture = ret.ProfilePicture,
                UploadDate = ret.UploadDate,
                Base64ImageString = base64String,
                User = ret.User,
                OriginalFileName = ret.OriginalFileName
            };
            
            return tmp;
        }

        public long Insert(UserPicture userPicture)
        {
            long i = db.Insert(userPicture);

            return i;
        }

        public int Delete(long Id)
        {            
            return db.Delete(Id);
        }

        public int Update(UserPicture userPicture)
        {           
            return db.Update(userPicture);
        }
    }
}
