using StudentApp.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentApp.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        readonly StudentAppEntities db = new StudentAppEntities();
        public ActionResult Index()
        {
            return View();
        }
    }
}