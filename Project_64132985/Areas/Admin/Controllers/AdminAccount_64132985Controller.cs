using Project_64132985.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_64132985.Areas.Admin.Controllers
{
    public class AdminAccount_64132985Controller : Controller
    {
        // GET: Admin/AdminAccount_64132985
        private QL_THOITRANG_FINALEntities db = new QL_THOITRANG_FINALEntities();
        public ActionResult Index()
        {
            var user = Session["User"] as Employee;
            var emp = db.Employees.FirstOrDefault(e => e.EmployeeID == user.EmployeeID);
            var res = new
            {
                EmployeeID = emp.EmployeeID,
                EmployeeName = emp.EmployeeName,
                Img = emp.Img,
                Email = emp.Email,
                PhoneNumber = emp.PhoneNumber,
                Addr = emp.Addr,
                Roles = emp.Roles,
            };
            return Json(new { success = true, user = res }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult UpdateAccount( string phone, string address)
        {
            var empSession = Session["User"] as Employee;
            var emp = db.Employees.Find(empSession.EmployeeID);
            var imageEmp = Request.Files["image-emp"];
            string postedFileName = "";
            bool isChanged = false;
            if(imageEmp != null)
            {
                postedFileName = System.IO.Path.GetFileName(imageEmp.FileName);
            }
          
            if(phone != emp.PhoneNumber)
            {
                emp.PhoneNumber = phone;
                isChanged = true;
            }
            if(address != emp.Addr)
            {
                emp.Addr = address;
                isChanged = true;
            }
            if(postedFileName != "" && emp.Img != postedFileName)
            {
                emp.Img = postedFileName;
                var path = Server.MapPath("/Images/Employees/" + postedFileName);
                imageEmp.SaveAs(path);
                isChanged = true;
            }
            if (isChanged)
            {
                db.SaveChanges();
            }
            return Json(new { success = true, message = "Cập nhật thành công" });
        }
    }
}