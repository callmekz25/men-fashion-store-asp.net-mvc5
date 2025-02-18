using Project_64132985.GenerateId;
using Project_64132985.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


namespace Project_64132985.Controllers
{
    public class Order_64132985Controller : Controller
    {
        // GET: Order_64132985
        private QL_THOITRANG_FINALEntities db = new QL_THOITRANG_FINALEntities();

        public ActionResult Index(string id)
        {
            if (id != null)
            {

                var products = Session["SelectedProducts"] as List<SelectedProduct_64132985>;
                if (products == null)
                {
                    return RedirectToAction("Error", "Home_64132985");
                }

                Customer customer;
                List<ProductCheckoutView_64132985> productCheckout = new List<ProductCheckoutView_64132985>();

                if (Session["User"] != null)
                {
                    customer = Session["User"] as Customer;
                    ViewBag.User = customer;

                    // Proceed with adding the products to the view model
                    foreach (var item in products)
                    {
                        Product data = db.Products.Find(item.ProductID);
                        var pro = new ProductCheckoutView_64132985
                        {
                            ProductID = item.ProductID,
                            ProductName = data.ProductName,
                            Category = data.Category,
                            Img = data.Img,
                            Quantity = item.Quantity,
                            Color = item.Color,
                            Size = item.Size,
                            Price = data.Price,
                        };
                        productCheckout.Add(pro);
                    }

                    ViewBag.Products = productCheckout;
                    ViewBag.OrderID = id;
                }

                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home_64132985");
            }

        }
        [HttpPost]
        public ActionResult ApplyVoucher(string id)
        {
            var voucher = db.Vouchers.Find(id);

            if (voucher == null)
            {
                return Json(new { success = false, message = "Mã giảm giá không hợp lệ" });
            }
            else
            {
                DateTime? date = DateTime.Now.Date;
                DateTime? expiredDate = voucher.Expired.ToLocalTime();
                if (date > expiredDate)
                {
                    return Json(new { success = false, message = "Mã giảm giá không hợp lệ" });
                }
                
            }
            return Json(new { success = true, message = "Áp dụng mã giảm giá thành công.", discount = voucher.Discount, voucherID = voucher.VoucherID });
        }
        [HttpPost]
        public ActionResult PlaceOrder(string orderId, string name, string email, string phone, string address, string voucher)
        {
            var user = Session["User"] as Customer;
            var products = Session["SelectedProducts"] as List<SelectedProduct_64132985>;
            var order = new OrderCustomer();

            if (user != null)
            {
                if (voucher != "")
                {

                    order = new OrderCustomer
                    {

                        OrderID = orderId,
                        CreatedAt = DateTime.Now,
                        Status = "Đang chờ xác nhận",
                        CustomerID = user.CustomerID,
                        VoucherID = voucher,
                        Addr = address,
                        PhoneNumber = phone,

                    };
                    db.OrderCustomers.Add(order);
                }
                else
                {
                    order = new OrderCustomer
                    {
                        OrderID = orderId,
                        CreatedAt = DateTime.Now,
                        Status = "Đang chờ xác nhận",
                        CustomerID = user.CustomerID,
                        Addr = address,
                        PhoneNumber = phone,

                    };
                    db.OrderCustomers.Add(order);
                }


                IdGenerate_64132985 id = new IdGenerate_64132985();
                foreach (var item in products)
                {
                    var orderDetail = new OrderCustomerDetail
                    {
                        OrderDetailID = id.GenerateOrderDetailId(),
                        ProductID = item.ProductID,
                        Quantity = item.Quantity,
                        Color = item.Color,
                        Size = item.Size,
                        OrderID = order.OrderID,
                    };
                    db.OrderCustomerDetails.Add(orderDetail);
                    db.SaveChanges();
                }
                Session.Remove("SelectedProducts");
                return Json(new { success = true, message = "" });

            }
            else
            {
                return Json(new { success = false, message = "Bạn cần đăng nhập" });
            }
        }

        public ActionResult AfterCheckout(string id)
        {
            var order = db.OrderCustomers.FirstOrDefault(x => x.OrderID == id);
            if (order == null)
            {
                return RedirectToAction("Error", "Home_64132985");
            }
            return View();
        }
        [HttpPost]
        public ActionResult CancelOrder (string id)
        {
            var order = db.OrderCustomers.Find(id);
            if (order != null)
            {
                order.Status = "Đã hủy";
                db.SaveChanges();
                return  Json(new { success = true, message = "" });
            } else
            {
                return Json(new { success = false, message = "Lỗi xảy ra" });
            }
        }

    }
}