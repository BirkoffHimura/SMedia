using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace SMedia.Areas.Security.Controllers
{
    [AllowAnonymous]
    public class RegisterController : Controller
    {
        // GET: Security/Register
        public ActionResult Index()
        {
            return View();
        }

        //AccountContext.User user
        [HttpPost]
        public ActionResult Register(AccountContext.User user)
        {
            try
            {
                user.SignupDate = DateTime.Now;
                UserBs uDb = new UserBs();                

                int i = uDb.Insert(user);
                if (i < 0)
                {
                    TempData["msg"] = "<strong id=\"myErrorMessage\">Register Failed!</strong>";
                }
                else
                {
                    TempData["msg"] = "<strong id=\"myErrorMessage\">Account registered successfully!!</strong>";
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = "<strong id=\"myErrorMessage\">Register Failed!</strong> <i>" + ex.Message + "</i>";
            }
            return RedirectToAction("Index");
        }
    }
}