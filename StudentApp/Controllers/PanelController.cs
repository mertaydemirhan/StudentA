using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentApp.Controllers
{
    [Authorize]
    public class PanelController : Controller
    {
        // GET: Panel
        public ActionResult Dashboard()
        {
            return View();
        }
    }
}