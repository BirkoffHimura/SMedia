using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SMedia.Areas.Security.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        // GET: Security/Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(AccountContext.User user)
        {
            try
            {
                if (Membership.ValidateUser(user.UserName, user.Password))
                {
                    FormsAuthentication.SetAuthCookie(user.UserName, false);
                    return RedirectToAction("Index", "Home", new { area = "User" });

                }
                TempData["msg"] = "<strong>Login Failed!</strong> Please check your credentials";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["msg"] = "<strong>Login Failed!</strong> " + ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}