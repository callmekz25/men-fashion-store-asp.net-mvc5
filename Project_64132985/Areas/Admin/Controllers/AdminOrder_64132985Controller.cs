
using Project_64132985.App_Start;
using Project_64132985.Areas.Admin.Data;
using Project_64132985.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_64132985.Areas.Admin.Controllers
{
    public class AdminOrder_64132985Controller : Controller
    {
        private QL_THOITRANG_FINALEntities db = new QL_THOITRANG_FINALEntities();
        // GET: Admin/Order_64132985
        [AdminAuthorize_64132985(Role = "admin,employee")]
        public ActionResult Index(string id = "", string cname = "", string ename = "", string cemail = "", string cphone = "", DateTime? fromDate = null, DateTime? toDate = null, int price = 0, string status = "")
        {
            var user = Session["User"] as Employee;
            if (user == null || user.Roles == null)
            {
                return RedirectToAction("Error", "Home_64132985", new { area = "" });

            }
            ViewBag.User = user;
            var ids = db.OrderCustomers.Select(o => o.OrderID).Take(6).ToList();
            ViewBag.Ids = ids;
            int numberFilter = 0;
            var orders = db.OrderCustomers.AsQueryable();
            var result = new List<AdminOrderView_64132985>();
            //if (string.IsNullOrEmpty(id))
            //{

            //    var orders = db.OrderCustomers.ToList();
            //    var result = new List<AdminOrderView_64132985>();
            //    var total = 0;
            //    foreach (var item in orders)
            //    {
            //        // Reset lại total sau mỗi đơn đặt hàng
            //        total = 0;
            //        var orderDetail = db.OrderCustomerDetails.Where(od => od.OrderID == item.OrderID).ToList();
            //        var employee = db.Employees.FirstOrDefault(e => e.EmployeeID == item.EmployeeID);
            //        var discount = db.Vouchers.FirstOrDefault(v => v.VoucherID == item.VoucherID);
            //        var empName = "";
            //        if (employee == null)
            //        {
            //            empName = "Trống";
            //        }
            //        else
            //        {
            //            empName = employee.EmployeeName;
            //        }
            //        foreach (var item1 in orderDetail)
            //        {
            //            var product = db.Products.FirstOrDefault(p => p.ProductID == item1.ProductID);
            //            total += product.Price * item1.Quantity;
            //        }
            //        // Tính Total sau khi áp dụng discount (nếu có)
            //        double finalTotal = total;
            //        if (discount != null)
            //        {
            //            finalTotal = total * (1 - (double)discount.Discount / 100); // Tính giảm giá
            //        }
            //        var data = new AdminOrderView_64132985()
            //        {
            //            OrderID = item.OrderID,
            //            EmployeeName = empName,
            //            Status = item.Status,
            //            CreatedAt = item.CreatedAt,
            //            Total = (int)finalTotal, // Ép kiểu về int nếu cần
            //            Quantity = orderDetail.Count()
            //        };
            //        result.Add(data);
            //    }
            //    result = result.OrderByDescending(x => x.CreatedAt).ToList();
            //    return View(result);
            //}
            //else
            //{
            //    var order = db.OrderCustomers.Find(id);
            //    var result = new List<AdminOrderView_64132985>();
            //    ViewBag.Id = id;
            //    if (order != null)
            //    {

            //        var total = 0;
            //        var orderDetail = db.OrderCustomerDetails.Where(od => od.OrderID == order.OrderID).ToList();
            //        var employee = db.Employees.FirstOrDefault(e => e.EmployeeID == order.EmployeeID);
            //        var discount = db.Vouchers.FirstOrDefault(v => v.VoucherID == order.VoucherID);
            //        var empName = "";
            //        if (employee == null)
            //        {
            //            empName = "Trống";
            //        }
            //        else
            //        {
            //            empName = employee.EmployeeName;
            //        }
            //        foreach (var item1 in orderDetail)
            //        {
            //            var product = db.Products.FirstOrDefault(p => p.ProductID == item1.ProductID);
            //            total += product.Price * item1.Quantity;
            //        }
            //        // Tính Total sau khi áp dụng discount (nếu có)
            //        double finalTotal = total;
            //        if (discount != null)
            //        {
            //            finalTotal = total * (1 - (double)discount.Discount / 100); // Tính giảm giá
            //        }
            //        var data = new AdminOrderView_64132985()
            //        {
            //            OrderID = order.OrderID,
            //            EmployeeName = empName,
            //            Status = order.Status,
            //            CreatedAt = order.CreatedAt,
            //            Total = (int)finalTotal, // Ép kiểu về int nếu cần
            //            Quantity = orderDetail.Count()
            //        };
            //        result.Add(data);


            //    }
            //    else
            //    {
            //        ViewBag.Result = "Không tìm thấy kết quả phù hợp";
            //    }
            //    return View(result);
            //}
            // Lọc theo OrderID
            if (!string.IsNullOrEmpty(id))
            {
                orders = orders.Where(o => o.OrderID.Contains(id));
            }

            // Lọc theo EmployeeName
            if (!string.IsNullOrEmpty(ename))
            {
                var employeeIds = db.Employees
            .Where(e => e.EmployeeName.Contains(ename)) // Tìm nhân viên theo tên
            .Select(e => e.EmployeeID)                  // Lấy ra danh sách EmployeeID
            .ToList();

                orders = orders.Where(o => employeeIds.Contains(o.EmployeeID));
                numberFilter += 1;
            }
            // Lọc theo tên khách hàng
            if (!string.IsNullOrEmpty(cname))
            {
                var customerIds = db.Customers
            .Where(c => c.CustomerName.Contains(cname))
            .Select(c => c.CustomerID)
            .ToList();
                orders = orders.Where(o => customerIds.Contains(o.CustomerID));
                numberFilter += 1;
            }
            // Lọc theo email khách hàng
            if (!string.IsNullOrEmpty(cemail))
            {
                var customerIds = db.Customers
            .Where(c => c.Email.Contains(cemail))
            .Select(c => c.CustomerID)
            .ToList();
                orders = orders.Where(o => customerIds.Contains(o.CustomerID));
                numberFilter += 1;
            }
            // Lọc theo sdt khách hàng
            if (!string.IsNullOrEmpty(cphone))
            {
                var customerIds = db.Customers
            .Where(c => c.PhoneNumber.Contains(cphone))
            .Select(c => c.CustomerID)
            .ToList();
                orders = orders.Where(o => customerIds.Contains(o.CustomerID));
                numberFilter += 1;
            }
            
            // Lọc theo trạng thái đơn hàng
            if (!string.IsNullOrEmpty(status))
            {
                orders = orders.Where(o => o.Status == status);
                numberFilter += 1;
            }

            // Lọc theo ngày (từ ngày đến ngày)
            if (fromDate.HasValue && toDate.HasValue)
            {
                var from = fromDate.Value.Date;
                var to = toDate.Value.Date;
                orders = orders.Where(o => DbFunctions.TruncateTime(o.CreatedAt) >= from && DbFunctions.TruncateTime(o.CreatedAt) <= to);
                numberFilter += 1;
            }
           
            foreach (var item in orders)
            {
                var total = 0;
                var orderDetail = db.OrderCustomerDetails.Where(od => od.OrderID == item.OrderID).ToList();
                var employee = db.Employees.FirstOrDefault(e => e.EmployeeID == item.EmployeeID);
                var discount = db.Vouchers.FirstOrDefault(v => v.VoucherID == item.VoucherID);
                var empName = employee?.EmployeeName ?? "Trống";

                foreach (var item1 in orderDetail)
                {
                    var product = db.Products.FirstOrDefault(p => p.ProductID == item1.ProductID);
                    total += product?.Price * item1.Quantity ?? 0;
                }

                double finalTotal = total;
                if (discount != null)
                {
                    finalTotal = total * (1 - (double)discount.Discount / 100); // Tính giảm giá
                }

                var data = new AdminOrderView_64132985()
                {
                    OrderID = item.OrderID,
                    EmployeeName = empName,
                    Status = item.Status,
                    CreatedAt = item.CreatedAt,
                    Total = (int)finalTotal,
                    Quantity = orderDetail.Count()
                };
                result.Add(data);
            }
            ViewBag.CName = cname;
            ViewBag.CEmail = cemail;
            ViewBag.CPhone = cphone;
            ViewBag.EName = ename;
            ViewBag.Status = status;
            ViewBag.To = toDate;
            ViewBag.From = fromDate;
            ViewBag.NumberFilter = numberFilter;
            ViewBag.Id = id;
            result = result.OrderByDescending(x => x.CreatedAt).ToList();
            if (result.Count() == 0)
            {
                ViewBag.Result = "Không tìm thấy kết quả phù hợp";
            } 
            return View(result);

        }
        [AdminAuthorize_64132985(Role = "admin,employee")]
        [HttpPost]
        public ActionResult Detail(string id)
        {

            var order = db.OrderCustomers.FirstOrDefault(od => od.OrderID == id);
            var employee = db.Employees.FirstOrDefault(e => e.EmployeeID == order.EmployeeID);
            var employeeName = "";
            var discount = 0;
            if (employee != null)
            {
                employeeName = employee.EmployeeName;
            }
            else
            {
                employeeName = "";
            }
            var customer = db.Customers.FirstOrDefault(c => c.CustomerID == order.CustomerID);
            var voucher = db.Vouchers.FirstOrDefault(v => v.VoucherID == order.VoucherID);
            if (voucher != null)
            {
                discount = voucher.Discount;
            }
            var orderDetail = db.OrderCustomerDetails.Where(odd => odd.OrderID == order.OrderID).ToList();
            var products = new List<ProductCheckoutView_64132985>();
            foreach (var item in orderDetail)
            {
                var productDetail = db.Products.FirstOrDefault(p => p.ProductID == item.ProductID);
                var res = new ProductCheckoutView_64132985()
                {
                    ProductID = item.ProductID,
                    ProductName = productDetail.ProductName,
                    Img = productDetail.Img,
                    Price = productDetail.Price,
                    Quantity = item.Quantity,
                    Color = item.Color,
                    Size = item.Size,
                    Category = productDetail.Category,

                };
                products.Add(res);
            }
            var result = new
            {
                OrderID = order.OrderID,
                ProductCheckoutView = products,
                EmployeeName = employeeName,
                CreatedAt = order.CreatedAt.ToString("dd-MM-yyyy"),
                Status = order.Status,
                CustomerName = customer.CustomerName,
                Address = order.Addr,
                Phone = order.PhoneNumber,
                Email = customer.Email,
                Discount = discount,
            };
            return Json(new { success = true, message = "Chi tiết đơn đặt hàng", orderDetail = result });
        }
        [AdminAuthorize_64132985(Role = "admin,employee")]
        [HttpPost]
        public ActionResult ConfirmOrder(string id, string action)
        {

            var emp = Session["User"] as Employee;

            var order = db.OrderCustomers.FirstOrDefault(o => o.OrderID == id);
            if (order == null)
            {
                return Content($"Order with ID {id} not found."); // Debug nếu không tìm thấy Order
            }
            if (action == "cancel")
            {
                order.Status = "Đã hủy";
                order.EmployeeID = emp.EmployeeID;
            }
            else if (action == "confirm")
            {

                order.Status = "Đã xác nhận";
                order.EmployeeID = emp.EmployeeID;
            }

            try
            {
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Content($"Error saving changes: {ex.Message}"); // Debug nếu có lỗi khi lưu
            }
        }
        [AdminAuthorize_64132985(Role = "admin")]
        [HttpPost]
        public ActionResult DeleteOrder(string id)
        {
            var order = db.OrderCustomers.FirstOrDefault(o => o.OrderID == id);
            if (order != null)
            {
                var orderDetail = db.OrderCustomerDetails.Where(od => od.OrderID == order.OrderID).ToList();
                db.OrderCustomerDetails.RemoveRange(orderDetail);
                db.OrderCustomers.Remove(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
    }

}