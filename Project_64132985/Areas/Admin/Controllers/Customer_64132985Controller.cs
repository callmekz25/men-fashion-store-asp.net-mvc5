using Project_64132985.App_Start;
using Project_64132985.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using static System.Data.Entity.Infrastructure.Design.Executor;

namespace Project_64132985.Areas.Admin.Controllers
{
    public class Customer_64132985Controller : Controller
    {
        private QL_THOITRANG_FINALEntities db = new QL_THOITRANG_FINALEntities();
        // GET: Admin/Customer_64132985
        [AdminAuthorize_64132985(Role = "admin,employee")]
        public ActionResult Index(string id = "", string name = "", DateTime? createdAt = null, string email = "", string active = "", string phone = "", string addr = "")
        {
            var user = Session["User"] as Employee;


            ViewBag.User = user;
            ViewBag.Id = id;
            var ids = db.Customers.Select(c => c.CustomerID).Take(6).ToList();
            ViewBag.Ids = ids;
            var customers = db.Customers.AsQueryable();
            int numberFilter = 0;
            if (string.IsNullOrEmpty(id))
            {
                if (!string.IsNullOrEmpty(name))
                {
                    customers = customers.Where(c => c.CustomerName.Contains(name));
                    numberFilter += 1;
                }
                if (createdAt.HasValue)
                {
                    var to = createdAt.Value.Date;
                    customers = customers.Where(c => DbFunctions.TruncateTime(c.CreatedAt) == to);
                    numberFilter += 1;
                }
                if (!string.IsNullOrEmpty(email))
                {
                    customers = customers.Where(c => c.Email.Contains(email));
                    numberFilter += 1;
                }
                if (!string.IsNullOrEmpty(phone))
                {
                    customers = customers.Where(c => c.PhoneNumber.Contains(phone));
                    numberFilter += 1;
                }
                if (!string.IsNullOrEmpty(addr))
                {
                    customers = customers.Where(c => c.Addr.Contains(addr));
                    numberFilter += 1;
                }
                if (!string.IsNullOrEmpty(active))
                {
                    bool isActive = active == "true";
                    customers = customers.Where(c => c.Active == isActive);
                    numberFilter += 1;
                }
                ViewBag.Name = name;
                ViewBag.Email = email;
                ViewBag.Phone = phone;
                ViewBag.Addr = addr;
                ViewBag.Active = active;
                ViewBag.Date = createdAt;
                ViewBag.NumberFilter = numberFilter;
                var filteredCustomer = customers.ToList();
                if(filteredCustomer.Count() == 0)
                {
                    ViewBag.Result = "Không tìm thấy kết quả phù hợp";
                }
                return View(filteredCustomer);
            }
            else
            {
                var listCustomer = new List<Customer>();
                var customer = db.Customers.Find(id);
                if (customer != null)
                {
                    listCustomer.Add(customer);
                }
                else
                {
                    ViewBag.Result = "Không tìm thấy kết quả phù hợp";
                }
                return View(listCustomer);
            }

        }
        [AdminAuthorize_64132985(Role = "admin")]
        [HttpPost]
        public ActionResult BlockCustomer(string id, string status)
        {
            var findCustomer = db.Customers.FirstOrDefault(c => c.CustomerID == id);
            if (findCustomer != null)
            {
                if (status == "block")
                {
                    findCustomer.Active = false;
                }
                else
                {
                    findCustomer.Active = true;
                }
                db.SaveChanges();
                return Json(new { success = true, message = "Hủy thành công" });
            }
            else
            {
                return Json(new { success = false, message = "Không tìm thấy khách hàng này trong dữ liệu" });
            }
        }
    }
}