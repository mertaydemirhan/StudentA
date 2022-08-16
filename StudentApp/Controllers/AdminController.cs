using StudentApp.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace StudentApp.Controllers
{
    public class AdminController : Controller
    {

        StudentAppEntities db = new StudentAppEntities();

        // GET: Admin
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Admin a)
        {
            var data = db.Admins.FirstOrDefault(x => x.Mail == a.Mail && x.Password == a.Password);

            if (data != null)
            {
                FormsAuthentication.SetAuthCookie(data.Mail, false);
                Session["Mail"] = data.Mail.ToString();
                return RedirectToAction("Dashboard", "Panel");
            }
            else
            {
                return View();
            }

        }

        public ActionResult Logout()
        {
            Session["UserInfo"] = null;
            Session.Abandon();
            return RedirectToAction("Login", "Admin");
        }
    }
}