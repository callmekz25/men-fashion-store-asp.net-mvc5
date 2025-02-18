using Project_64132985.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_64132985.Areas.Admin.Controllers
{
    public class Login_64132985Controller : Controller
    {
        private QL_THOITRANG_FINALEntities db = new QL_THOITRANG_FINALEntities();
        // GET: Admin/Login_64132985
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            var admin = db.Employees.FirstOrDefault(ad => ad.Email == email && ad.Pass == password);
            if (admin == null)
            {
                return Json(new { success = false, message = "Email hoặc mật khẩu sai" }, JsonRequestBehavior.AllowGet);
            } else
            {
                return Json(new { success = true, message = "Đăng nhập thành công" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}