using Project_64132985.App_Start;
using Project_64132985.GenerateId;
using Project_64132985.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Project_64132985.Areas.Admin.Controllers
{
    public class AdminEmployee_64132985Controller : Controller
    {
        private QL_THOITRANG_FINALEntities db = new QL_THOITRANG_FINALEntities();
        // GET: Admin/AdminEmployee_64132985
        [AdminAuthorize_64132985(Role = "admin")]
        public ActionResult Index(string id = "", DateTime? createdAt = null, string name = "", string email = "", string phone = "", string addr = "", string active = "")
        {
            var user = Session["User"] as Employee;

            ViewBag.User = user;
            ViewBag.Id = id;
            var ids = db.Employees.Select(e => e.EmployeeID).Take(6).ToList();
            ViewBag.Ids = ids;
            int numberFilter = 0;
            var employees = db.Employees.AsQueryable();
            if (string.IsNullOrEmpty(id))
            {
                if (!string.IsNullOrEmpty(name))
                {

                    employees = employees.Where(e => e.EmployeeName.Contains(name));
                    numberFilter += 1;
                } 
                if(!string.IsNullOrEmpty(email))
                {
                    employees = employees.Where(e => e.Email.Contains(email));
                    numberFilter += 1;
                }
                if (!string.IsNullOrEmpty(addr))
                {
                    employees = employees.Where(e => e.Addr.Contains(addr));
                    numberFilter += 1;
                }
                if (!string.IsNullOrEmpty(phone))
                {
                    employees = employees.Where(e => e.PhoneNumber.Contains(phone));
                    numberFilter += 1;
                }
                if (createdAt.HasValue)
                {
                    var to = createdAt.Value.Date;
                    employees = employees.Where(e => DbFunctions.TruncateTime(e.CreatedAt) == to);
                    numberFilter += 1;
                }
                if (!string.IsNullOrEmpty(active))
                {
                    bool isActive = active == "true";
                    employees = employees.Where(e => e.Active == isActive);
                    numberFilter += 1;
                }
                ViewBag.Name = name;
                ViewBag.Email = email;
                ViewBag.Phone = phone;
                ViewBag.Addr = addr;
                ViewBag.Active = active;
                ViewBag.Date = createdAt;
                ViewBag.NumberFilter = numberFilter;
                var filtereds = employees;
                if(filtereds.Count() == 0)
                {
                    ViewBag.Result = "Không tìm thấy kết quả phù hợp";
                }
                return View(employees);
            }
            else
            {
                var result = new List<Employee>();
                var employee = db.Employees.Find(id);
                if (employee != null)
                {
                    result.Add(employee);
                }
                else
                {
                    ViewBag.Result = "Không tìm thấy kết quả phù hợp";
                }
                return View(result);
            }
        }
        [AdminAuthorize_64132985(Role = "admin")]
        public ActionResult Add()
        {
            return View();
        }
        [AdminAuthorize_64132985(Role = "admin")]
        [HttpPost]
        public ActionResult Add(string name, string email, string phone, string address, string pass, string role)
        {
            IdGenerate_64132985 id = new IdGenerate_64132985();
            var imageEmployee = Request.Files["img"];
            string postedFileName = System.IO.Path.GetFileName(imageEmployee.FileName);
            var path = Server.MapPath("/Images/Employees/" + postedFileName);
            imageEmployee.SaveAs(path);
            var employee = new Employee()
            {
                EmployeeID = id.GenerateEmployeeId(),
                EmployeeName = name,
                Email = email,
                PhoneNumber = phone,
                Addr = address,
                Pass = pass,
                Roles = role,
                Active = true,
                Img = postedFileName,
                CreatedAt = DateTime.Now,
            };
            db.Employees.Add(employee);
            db.SaveChanges();
            return Json(new { success = true, message = "Thêm nhân viên thành công" });
        }

        [AdminAuthorize_64132985(Role = "admin")]
        [HttpPost]
        public ActionResult Delete(string id, string status)
        {
            var employee = db.Employees.Find(id);
            if (employee != null)
            {
                if (status == "block")
                {
                    employee.Active = false;
                }
                else
                {
                    employee.Active = true;
                }
                db.SaveChanges();
                return Json(new { success = true, message = "Hủy tài khoản thành công" });
            }
            else
            {
                return Json(new { success = false, message = "Không tìm thấy thông tin" });
            }
        }
    }
}