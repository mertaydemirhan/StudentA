using StudentApp.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace StudentApp.Controllers
{
    public class AdminController : Controller
    {

        StudentAppEntities db = new StudentAppEntities();

        public ActionResult Index()
        {
            return View(db.Admins.ToList());
        }
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
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Where(x => x.ID == id).FirstOrDefault();
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create([Bind(Include = "ID,Mail,Password,IsPowered")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Admins.Add(admin);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(admin);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Where(x => x.ID == id).FirstOrDefault();
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        [HttpPost]
        public ActionResult Edit([Bind(Include = "ID,Mail,Password,IsPowered")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Entry(admin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(admin);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Where(x => x.ID == id).FirstOrDefault();
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Admin admin = db.Admins.Where(x => x.ID == id).FirstOrDefault();
            db.Admins.Remove(admin);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult Logout()
        {
            Session["UserInfo"] = null;
            Session.Abandon();
            return RedirectToAction("Login", "Admin");
        }
    }
}