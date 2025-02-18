using Project_64132985.App_Start;
using Project_64132985.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_64132985.Areas.Admin.Controllers
{
    public class Dashboard_64132985Controller : Controller
    {
        private QL_THOITRANG_FINALEntities db = new QL_THOITRANG_FINALEntities();
        // GET: Admin/Dashboard_64132985
        [AdminAuthorize_64132985(Role = "admin,employee")]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetData()
        {
            var orders = db.OrderCustomers.Where(o => o.Status == "Đã xác nhận").ToList();
            var customers = db.Customers.ToList();
            var ordersJson = new List<Object>();
            var orderDetailJson = new List<Object>();
            var revenueData = new List<Object>();
            var listCustomer = new List<Object>();
            foreach (var item in customers)
            {
                var temp = new
                {
                    CreatedAt = item.CreatedAt.ToString("dd-MM-yyyy"),

                };
                listCustomer.Add(temp);
            }
            foreach (var item in orders)
            {
                var temp = new
                {
                    OrderID = item.OrderID,
                    CreatedAt = item.CreatedAt.ToString("dd-MM-yyyy"),
                };
                var orderDetail = db.OrderCustomerDetails.Where(od => od.OrderID == item.OrderID).ToList();
                foreach (var detail in orderDetail)
                {
                    // Lấy thông tin sản phẩm tương ứng với chi tiết đơn hàng
                    var product = db.Products.Where(p => p.ProductID == detail.ProductID).FirstOrDefault();

                    if (product != null)
                    {
                        // Tính toán giá tiền cho chi tiết đơn hàng
                        var tempOrderDetailJson = new
                        {
                            Category = product.Category,
                            Price = detail.Quantity * product.Price,
                        };

                        // Tạo đối tượng doanh thu cho từng chi tiết đơn hàng
                        var tempRevenueData = new
                        {
                            CreatedAt = item.CreatedAt.ToString("dd-MM-yyyy"),
                            Price = detail.Quantity * product.Price,
                        };

                        // Thêm vào các danh sách kết quả
                        orderDetailJson.Add(tempOrderDetailJson);
                        revenueData.Add(tempRevenueData);
                    }

                }
                ordersJson.Add(temp);
            }
            return Json(new { success = true, orders = ordersJson, ordersDetail = orderDetailJson, revenueData = revenueData, customer = listCustomer }, JsonRequestBehavior.AllowGet);

        }
    }
}