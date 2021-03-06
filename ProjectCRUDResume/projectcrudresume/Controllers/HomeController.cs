using projectcrudresume.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace projectcrudresume.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var TitleString = "Project WT";
            ViewBag.Title = TitleString;

            return View();
        }
    }
}
