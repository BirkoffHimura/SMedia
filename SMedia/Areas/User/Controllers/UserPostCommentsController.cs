using AccountContext;
using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;

namespace SMedia.Areas.User.Controllers
{
    public class UserPostCommentsController : Controller
    {
        private UserPostCommentBs upcb;
        private UserPostBs upb;
        private UserBs ub;
        public UserPostCommentsController()
        {
            upb = new UserPostBs();
            ub = new UserBs();
            upcb = new UserPostCommentBs();
        }
         
        [HttpPost]
        public ActionResult PostComment(string CommentData, long postID)
        {
            AccountContext.User currentUser = ub.GetByUserName(User.Identity.Name);
            UserPostComment temp = new UserPostComment();
            temp.CommentDate = DateTime.Now;
            temp.Comment_Body = CommentData;
            temp.FromUser = currentUser;
            temp.UserPost = upb.GetByID(postID);
            upcb.Insert(temp);

            var commentListing = upcb.GetAllByPostID(postID);

            return Json(commentListing, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetPostCommentListing(long thePostID)
        {            
            return Json(upcb.GetAllByPostIDWithUserData(thePostID), JsonRequestBehavior.AllowGet);                        
        }
    }
}