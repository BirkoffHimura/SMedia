using AccountContext;
using BLL;
using JsonLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMedia.Areas.User.Controllers
{
    public class UserPictureController : Controller
    {
        UserPictureBs upb;
        public UserPictureController()
        {
            upb = new UserPictureBs();
        }
        // GET: User/UserPicture
        public ActionResult UserPictureList()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetUserPictureListing(long Id)
        {
            JsontResultSet jrs = new JsontResultSet();
            if(Id<1)
            {
                UserBs uBs = new UserBs();
                AccountContext.User currentUser = uBs.GetByUserName(User.Identity.Name);
                var ret = upb.GetAllByUserIDBase64(currentUser.ID, Path.Combine(Server.MapPath(@"\App_Data\Images\")));
                jrs.Data = ret;
            }
            else
            {
                UserBs uBs = new UserBs();
                var ret = upb.GetAllByUserIDBase64(Id, Path.Combine(Server.MapPath(@"\App_Data\Images\")));
                jrs.Data = ret;
            }
            return jrs;
        }
        [HttpPost]
        public ActionResult UploadUserPicture()
        {
            UserBs uBs = new UserBs();
            AccountContext.User currentUser = uBs.GetByUserName(User.Identity.Name);
            foreach (string file in Request.Files)
            {
                var fileContent = Request.Files[file];
                if (fileContent != null && fileContent.ContentLength > 0)
                {
                    Guid guid = Guid.NewGuid();
                    var inputStream = fileContent.InputStream;
                    var fileName = guid.ToString() + Path.GetExtension(fileContent.FileName);
                    var path = Path.Combine(Server.MapPath(@"\App_Data\Images\"), fileName);
                    using (var fileStream = System.IO.File.Create(path))
                    {
                        inputStream.CopyTo(fileStream);
                    }
                    UserPicture userPicture = new UserPicture
                    {
                        FileName = fileName,
                        ProfilePicture = false,
                        UploadDate = DateTime.Now,
                        User = currentUser,
                        OriginalFileName = fileContent.FileName
                    };
                    upb.Insert(userPicture);
                }
            }

            JsontResultSet jrs = new JsontResultSet();

            var ret = upb.GetAllByUserIDBase64(currentUser.ID, Path.Combine(Server.MapPath(@"\App_Data\Images\")));
            jrs.Data = ret;

            return jrs;
        }
    }
}