using StudentApp.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentApp.Controllers
{
    public class ContactUsController : Controller
    {

        readonly StudentAppEntities db = new StudentAppEntities();
        // GET: ContactUs
        public ActionResult ContactUs()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SendMessage(Message Meessage)
        {
            int userID = (int)Session["UserID"];
            db.Messages.Add(new Message{ 
                Name = Meessage.Name,
                Email = Meessage.Email,
                Subject = Meessage.Subject,
                Note = Meessage.Note,
                UserID = userID
            });
            db.SaveChanges();
            TempData["Message"] = "Your Message registered to our Database";
            return Json(TempData["Message"], JsonRequestBehavior.AllowGet);
        }



    }
}