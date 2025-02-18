using Project_64132985.Models;
using Project_64132985.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_64132985.Controllers
{
    public class Account_64132985Controller : Controller
    {
        // GET: Account_64132985
        private QL_THOITRANG_FINALEntities db = new QL_THOITRANG_FINALEntities();
        private Email_64132985 emailService = new Email_64132985();
        public ActionResult Index()
        {
            // Chưa làm phần tính voucher giảm giá
            var user = Session["User"] as Customer;
            var list = new List<OrderProductView_64132985>();
            if (user != null)
            {
                var order = db.OrderCustomers.Where(o => o.CustomerID == user.CustomerID).ToList();
                if (order != null)
                {
                    foreach (var item in order)
                    {
                        // Lấy tất cả sản phẩm trong đơn hàng
                        var orderDetails = db.OrderCustomerDetails.Where(od => od.OrderID == item.OrderID).ToList();
                        var voucher = db.Vouchers.Where((v) => v.VoucherID == item.VoucherID).FirstOrDefault();
                        int total = 0;
                        // Tạo 1 list có cấu trúc theo model ProductCheckoutView
                        var productList = new List<ProductCheckoutView_64132985>();

                        // Lặp qua từng sản phẩm trong OrderCustomerDetails
                        foreach (var detail in orderDetails)
                        {
                            var product = db.Products.FirstOrDefault(p => p.ProductID == detail.ProductID);

                            if (product != null)
                            {
                                total += ((product.Price) * detail.Quantity);
                                var temp = new ProductCheckoutView_64132985
                                {
                                    ProductID = product.ProductID,
                                    Img = product.Img,
                                    ProductName = product.ProductName,
                                    Quantity = detail.Quantity,
                                    Color = detail.Color,
                                    Size = detail.Size,
                                    Price = product.Price,
                                    Category = product.Category,
                                };
                                productList.Add(temp);
                            }
                        }

                        // Thêm thông tin đơn hàng và danh sách sản phẩm vào list
                        var listOrder = new OrderProductView_64132985();
                        if (voucher != null)
                        {
                            listOrder = new OrderProductView_64132985
                            {
                                OrderID = item.OrderID,
                                CreatedAt = item.CreatedAt,
                                Status = item.Status,
                                Total = Math.Round(total - (total * ((decimal)voucher.Discount / 100)), 2),
                                Products = productList, // Danh sách sản phẩm đầy đủ
                            };
                        }
                        else
                        {
                            listOrder = new OrderProductView_64132985
                            {
                                OrderID = item.OrderID,
                                CreatedAt = item.CreatedAt,
                                Status = item.Status,
                                Total = total,
                                Products = productList, // Danh sách sản phẩm đầy đủ
                            };
                        }

                        list.Add(listOrder);
                    }

                }
                var sortedList = list.OrderByDescending(l => l.CreatedAt).ToList();
                return View(sortedList);
            }
            else
            {
                return RedirectToAction("Login", "Home_64132985");
            }

        }

        public ActionResult Address()
        {
            var user = Session["User"] as Customer;
            if (user != null)
            {
                var customer = db.Customers.FirstOrDefault(c => c.CustomerID == user.CustomerID);
                return View(customer);
            }
            else
            {
                return RedirectToAction("Login", "Home_64132985");
            }
        }
        [HttpPost]
        public ActionResult ChangeInfo(string newAddress, string newName, string newPhone)
        {
            var user = Session["User"] as Customer;
            if (user != null)
            {
                var customer = db.Customers.FirstOrDefault(c => c.CustomerID == user.CustomerID);
                if (customer != null)
                {
                    bool isChanged = false;
                    if (newAddress != customer.Addr)
                    {
                        isChanged = true;
                        customer.Addr = newAddress;
                    }
                    if (newName != customer.CustomerName)
                    {
                        isChanged = true;
                        customer.CustomerName = newName;
                    }
                    if(newPhone != customer.PhoneNumber)
                    {
                        isChanged = true;
                        customer.PhoneNumber = newPhone;
                    }
                    if (isChanged)
                    {

                        db.SaveChanges();

                    }
                    return Json(new { success = true, message = "Cập nhật thành công" });
                }
            }
            return Json(new { success = false, message = "Bạn chưa đăng nhập" });

        }
        public ActionResult ChangePassword()
        {
            var user = Session["User"] as Customer;
            if (user != null)
            {
                var customer = db.Customers.FirstOrDefault(c => c.CustomerID == user.CustomerID);
                return View(customer);
            }
            else
            {
                return RedirectToAction("Login", "Home_64132985");
            }
        }
        [HttpPost]
        public ActionResult CheckPassword(string password)
        {
            var user = Session["User"] as Customer;
            if (user != null)
            {
                var customer = db.Customers.FirstOrDefault(c => c.CustomerID == user.CustomerID);

                if (password == customer.Pass)
                {
                    return Json(new { success = true, message = "Mật khẩu hợp lệ" });
                }
                else
                {
                    return Json(new { success = false, message = "Mật khẩu sai hoặc không hợp lệ" });
                }

            }
            else
            {
                return RedirectToAction("Login", "Home_64132985");
            }
        }
        [HttpPost]
        public ActionResult SendCodeChangePassword(string email)
        {
            string authCode = AuthCode_64132985.GenerateCode();
            string subject = "Mã xác thực để xác minh tài khoản";
            string body = $@"
        <html>
          <head>
            <meta http-equiv='Content-Type' content='text/html; charset=UTF-8'/>
          </head>
          <body style='font-family: Arial, sans-serif; margin: 0; padding: 0; background-color: white;'>
            <table role='presentation' width='100%' cellspacing='0' cellpadding='0' style='background-color: #f0f0f0; max-width: 600px; margin: auto; padding: 20px;'>
              <tr>
                <td style='text-align: center; padding: 20px 0'>
                  
                 <h2 style='font-size: 25px; color: black; margin: 0; margin-top: 30px'>Xin chào, mã xác minh tài khoản của bạn là</h2>
                </td>
              </tr>
              <tr>
                <td style='text-align: center; background-color: white; padding: 20px 0; border-radius: 5px;'>
                 
                  <h3 style='font-size: 36px; color: black; margin: 20px 0'>{authCode}</h3>
                </td>
              </tr>
              <tr>
                <td style='text-align: center; padding-top: 30px; font-size: 16px; color: #666;'>
                  <p> Nếu bạn không phải là người gửi yêu cầu này, hãy kiểm tra lại email của bạn ngay lập tức để tránh việc bị lạm dụng truy cập trái phép. </p>
                </td>
              </tr>
              <tr>
                <td style='text-align: center; padding-top: 40px; font-size: 14px; color: #aaa;'>
                  <p>Trân trọng,<br />Cảm ơn bạn đã sử dụng dịch vụ của chúng tôi!</p>
                </td>
              </tr>
            </table>
          </body>
        </html>";
            emailService.SendEmail(email, subject, body);

            return Json(new { success = true, message = "Đã gửi mã xác thực vui lòng kiểm tra email", authCode = authCode });
        }
        [HttpPost]
        public ActionResult UpdatePassword(string password)
        {
            var user = Session["User"] as Customer;
            if (user != null)
            {
                var userDb = db.Customers.FirstOrDefault(c => c.CustomerID == user.CustomerID);
                userDb.Pass = password;
                db.SaveChanges();
                return Json(new { success = true, message = "Thay đổi mật khẩu thành công" });
            }
            else
            {
                return RedirectToAction("Login", "Home_64132985");
            }
        }
        public ActionResult AccountDetail()
        {
            var user = Session["User"] as Customer;
            if (user != null)
            {
                var customer = db.Customers.FirstOrDefault(c => c.CustomerID == user.CustomerID);
                return View(customer);
            }
            else
            {
                return RedirectToAction("Login", "Home_64132985");
            }
        }
        [HttpPost]
        public ActionResult SendCodeChangeEmail(string email)
        {
            string authCode = AuthCode_64132985.GenerateCode();
            string subject = "Mã xác thực để xác minh tài khoản";
            string body = $@"
        <html>
          <head>
            <meta http-equiv='Content-Type' content='text/html; charset=UTF-8'/>
          </head>
          <body style='font-family: Arial, sans-serif; margin: 0; padding: 0; background-color: white;'>
            <table role='presentation' width='100%' cellspacing='0' cellpadding='0' style='background-color: #f0f0f0; max-width: 600px; margin: auto; padding: 20px;'>
              <tr>
                <td style='text-align: center; padding: 20px 0'>
                  
                  <h2 style='font-size: 25px; color: black; margin: 0; margin-top: 30px'>Xin chào, mã xác minh tài khoản của bạn là</h2>
                </td>
              </tr>
              <tr>
                <td style='text-align: center; background-color: white; padding: 20px 0; border-radius: 5px;'>
                  <h3 style='font-size: 36px; color: black; margin: 20px 0'>{authCode}</h3>
                </td>
              </tr>
              <tr>
                <td style='text-align: center; padding-top: 30px; font-size: 16px; color: #666;'>
                  <p> Nếu bạn không phải là người gửi yêu cầu này, hãy kiểm tra lại email của bạn ngay lập tức để tránh việc bị lạm dụng truy cập trái phép. </p>
                </td>
              </tr>
              <tr>
                <td style='text-align: center; padding-top: 40px; font-size: 14px; color: #aaa;'>
                  <p>Trân trọng,<br />Cảm ơn bạn đã sử dụng dịch vụ của chúng tôi!</p>
                </td>
              </tr>
            </table>
          </body>
        </html>";
            emailService.SendEmail(email, subject, body);

            return Json(new { success = true, message = "Đã gửi mã xác thực vui lòng kiểm tra email", authCode = authCode });
        }

        [HttpPost]
        public ActionResult UpdateEmail(string email)
        {
            var user = Session["User"] as Customer;

            var findUser = db.Customers.FirstOrDefault(c => c.CustomerID == user.CustomerID);

            bool isChanged = false;

            if (email != findUser.Email)
            {
                isChanged = true;
                findUser.Email = email;
            }
            if (isChanged)
            {
                db.SaveChanges();
            }
            return Json(new { success = true, message = "Cập nhật thành công" });
        }
        [HttpPost]
        public ActionResult ForgotPassword(string email, string password)
        {
            var customer = db.Customers.FirstOrDefault(c => c.Email == email);
            if (customer != null)
            {
                string authCode = AuthCode_64132985.GenerateCode();
                string subject = "Mã xác thực khôi phục mật khẩu";
                string body = $@"
        <html>
          <head>
            <meta http-equiv='Content-Type' content='text/html; charset=UTF-8'/>
          </head>
          <body style='font-family: Arial, sans-serif; margin: 0; padding: 0; background-color: white;'>
            <table role='presentation' width='100%' cellspacing='0' cellpadding='0' style='background-color: #f0f0f0; max-width: 600px; margin: auto; padding: 20px;'>
              <tr>
                <td style='text-align: center; padding: 20px 0'>
                  
                  <h2 style='font-size: 25px; color: black; margin: 0; margin-top: 30px'>Xin chào, mã xác minh tài khoản của bạn là</h2>
                </td>
              </tr>
              <tr>
                <td style='text-align: center; background-color: white; padding: 20px 0; border-radius: 5px;'>
                  <h3 style='font-size: 36px; color: black; margin: 20px 0'>{authCode}</h3>
                </td>
              </tr>
              <tr>
                <td style='text-align: center; padding-top: 30px; font-size: 16px; color: #666;'>
                  <p> Nếu bạn không phải là người gửi yêu cầu này, hãy kiểm tra lại email của bạn ngay lập tức để tránh việc bị lạm dụng truy cập trái phép. </p>
                </td>
              </tr>
              <tr>
                <td style='text-align: center; padding-top: 40px; font-size: 14px; color: #aaa;'>
                  <p>Trân trọng,<br />Cảm ơn bạn đã sử dụng dịch vụ của chúng tôi!</p>
                </td>
              </tr>
            </table>
          </body>
        </html>";
                emailService.SendEmail(email, subject, body);
                return Json(new { success = true, message = "Đã gửi mã xác thực vui lòng kiểm tra email", authCode = authCode });
            } else
            {
                return Json(new { success = false, message = "Vui lòng kiểm tra lại email" });
            }
        }
        [HttpPost]
        public ActionResult RecoveryPassword(string email, string password)
        {
            var customer = db.Customers.FirstOrDefault(x => x.Email == email);
            customer.Pass = password;
            db.SaveChanges();
            return Json(new { success = true });
        }
    }
}