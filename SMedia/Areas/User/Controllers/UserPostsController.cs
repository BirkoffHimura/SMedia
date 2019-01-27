using AccountContext;
using BLL;
using JsonLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SMedia.Areas.User.Controllers
{   
    public class UserPostsController : Controller
    {
        
        private UserPostCommentBs upcb;
        private UserPostBs upb;
        private UserBs ub;
        public UserPostsController()
        {
            upb = new UserPostBs();
            ub = new UserBs();
            upcb = new UserPostCommentBs();
        }

        [HttpPost]
        public ActionResult CreatePost(string subject, string post_body)
        {
            UserPictureBs upb = new UserPictureBs();
            UserPost up = new UserPost();
            UserPostBs upBs = new UserPostBs();
            UserBs uBs = new UserBs();
            AccountContext.SMediaContext smc = new AccountContext.SMediaContext();
            AccountContext.User currentUser = uBs.GetByUserName(User.Identity.Name);
            foreach (string file in Request.Files)
            {
                var fileContent = Request.Files[file];
                if (fileContent != null && fileContent.ContentLength > 0)
                {
                    Guid guid = Guid.NewGuid();
                    var inputStream = fileContent.InputStream;
                    var fileName = guid.ToString() + Path.GetExtension(fileContent.FileName);
                    var path = Path.Combine(Server.MapPath("~/App_Data/Images"), fileName);
                    using (var fileStream = System.IO.File.Create(path))
                    {
                        inputStream.CopyTo(fileStream);
                    }
                    UserPicture userPicture = new UserPicture
                    {
                        FileName = fileName,
                        ProfilePicture = false,
                        UploadDate = DateTime.Now,
                        User = currentUser
                    };
                    long recentID = upb.Insert(userPicture);
                    up.UserPicture = userPicture;
                    up.UserPicture.PictureID = recentID;
                }
            }
            
            up.User = currentUser;
            up.PostDate = DateTime.Now;
            up.Post_Body = post_body;
            up.Subject = subject;
            string errorMessage = "";
            try
            {
                long i = upBs.Insert(up);
                UserPostBs upbs = new UserPostBs();
                var upwud = upbs.GetAllWithUserData();
                JsontResultSet jr = new JsontResultSet
                {
                    Data = upwud
                };
                return jr;
            }
            catch (Exception ex)
            {

                errorMessage = ex.Message;
                UserPostBs upbs = new UserPostBs();
                var upwud = upbs.GetAllWithUserData();
                JsontResultSet jr = new JsontResultSet
                {
                    Data = upwud
                };
                return jr;
            }

        }

        public ActionResult GetPostListing()
        {
            var up = upb.GetAllWithUserData();
            JsontResultSet jr = new JsontResultSet
            {
                Data = up
            };
            return jr;
        }
        [HttpPost]
        public ActionResult GetPostImage(long thePostImageID)
        {
            UserPictureBs upbs = new UserPictureBs();
            var uPic = upbs.GetByIDBase64(thePostImageID, Server.MapPath("~/App_Data/Images"));
            JsontResultSet jr = new JsontResultSet
            {
                Data = uPic
            };
            return jr;
        }
    }
}