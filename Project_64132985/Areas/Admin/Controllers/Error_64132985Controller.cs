using Project_64132985.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_64132985.Areas.Admin.Controllers
{
    public class Error_64132985Controller : Controller
    {
        // GET: Admin/Error_64132985
        [AdminAuthorize_64132985(Role = "admin,employee")]
        public ActionResult Index()
        {
            return View();
        }
    }
}