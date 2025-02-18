using Project_64132985.GenerateId;
using Project_64132985.Models;
using Project_64132985.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Project_64132985.Controllers
{
    public class Home_64132985Controller : Controller
    {
        private QL_THOITRANG_FINALEntities db = new QL_THOITRANG_FINALEntities();
        private Email_64132985 emailService = new Email_64132985();
        private RandomVoucher_64132985 random = new RandomVoucher_64132985();

        public ActionResult Index()
        {
            var listProducts = db.Products.Take(4).ToList();
            ViewBag.Products = db.Products.Where(p => p.Category == "khoác").Take(4).ToList();
            return View(listProducts);
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            var user = db.Customers.Where(c => c.Email == email && c.Pass == password).FirstOrDefault();
            var employee = db.Employees.Where(e => e.Email == email && e.Pass == password).FirstOrDefault();
            if (user != null)
            {
                if (user.Active)
                {
                    Session["User"] = user;
                    return Json(new { success = true, message = "Đăng nhập thành công" });
                }
                else
                {
                    return Json(new { success = false, message = "Tài khoản bạn đã bị khóa" });
                }
            }
            if (employee != null)
            {
                if (employee.Active)
                {

                    Session["User"] = employee;
                    return Json(new { success = true, role = employee.Roles, message = "Đăng nhập thành công" });
                } else
                {
                    return Json(new { success = false, message = "Tài khoản bạn đã bị khóa" });
                }
            }

            return Json(new { success = false, message = "Email hoặc mật khẩu của bạn không đúng" });
        }
        public ActionResult Signup()
        {
            return View();
        }
        [HttpPost]

        public ActionResult Signup(string CustomerName, string Email, string Pass)

        {
            var emailExist = db.Customers.Where((c) => c.Email == Email).FirstOrDefault();
            if (emailExist != null)
            {
                return Json(new { success = false, message = "Email đã tồn tại" });
            }
            else
            {

                string authCode = AuthCode_64132985.GenerateCode();
                string subject = "Mã xác thực email";
                string body = $@"
        <html>
          <head>
            <meta http-equiv='Content-Type' content='text/html; charset=UTF-8'/>
          </head>
          <body style='font-family: Arial, sans-serif; margin: 0; padding: 0; background-color: white;'>
            <table role='presentation' width='100%' cellspacing='0' cellpadding='0' style='background-color: #f0f0f0; max-width: 600px; margin: auto; padding: 20px;'>
              <tr>
                <td style='text-align: center; padding: 20px 0'>
                  
                  <h2 style='font-size: 32px; color: black; margin: 0; margin-top: 30px'>Mã Xác Thực</h2>
                </td>
              </tr>
              <tr>
                <td style='text-align: center; background-color: white; padding: 30px 0; border-radius: 5px;'>
                  <p style='font-size: 18px; color: #666'>Đây là mã xác thực của bạn:</p>
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
                emailService.SendEmail(Email, subject, body);

                return Json(new { success = true, message = "Đã gửi mã xác thực vui lòng kiểm tra email", authCode = authCode });
            }
        }
        [HttpPost]
        public ActionResult VerifyAndSaveUser(string Email, string CustomerName, string Pass)
        {
            IdGenerate_64132985 id = new IdGenerate_64132985();
            var user = new Customer()
            {
                CustomerID = id.GenerateCustomerId(),
                CustomerName = CustomerName,
                Email = Email,
                Pass = Pass,
                CreatedAt = DateTime.Now,
                Active = true
            };
            db.Customers.Add(user);
            db.SaveChanges();
            return Json(new { success = true, message = "Xác thực tài khoản thành công" });


        }
        [HttpPost]
        public ActionResult SendVoucher(string email)
        {
            string voucher = random.GetRandomVoucher();
            
            var voucherDb = db.Vouchers.Find(voucher);
            string subject = "Chúc mừng! Bạn nhận được voucher giảm giá từ chúng tôi!";
            string body = $@"
<html>
  <head>
    <meta http-equiv='Content-Type' content='text/html; charset=UTF-8'/>
  </head>
  <body style='font-family: Arial, sans-serif; margin: 0; padding: 0; background-color: white;'>
    <table role='presentation' width='100%' cellspacing='0' cellpadding='0' style='background-color: #f0f0f0; max-width: 600px; margin: auto; padding: 20px;'>
      <tr>
        <td style='text-align: center; padding: 20px 0'>
        
          <h2 style='font-size: 32px; color: black; margin: 0; margin-top: 30px'>Chúc Mừng! Bạn Đã Nhận {voucherDb.Descr}</h2>
           <p style='font-size: 17px; color: #666'>Cảm ơn bạn đã tin tưởng sử dụng dịch vụ của chúng tôi. Để chúc mừng bạn, chúng tôi gửi tặng bạn một voucher giảm giá đặc biệt:</p>
        </td>
      </tr>
      <tr>
        <td style='text-align: center; background-color: white; padding: 30px 0; border-radius: 5px;'>
          <p style='font-size: 16px; color: #666'>Đây là mã giảm giá của bạn</p>
          <h3 style='font-size: 36px; color: black; margin: 20px 0'>{voucher}</h3>
        </td>
      </tr>
      <tr>
        <td style='text-align: center; padding-top: 30px; font-size: 16px; color: #666;'>
        <p style='font-size: 18px; color: #666'>Hãy sử dụng voucher này khi mua sắm tại cửa hàng của chúng tôi để nhận ưu đãi đặc biệt!</p>
          <p> Nếu bạn không phải là người gửi yêu cầu này, hãy kiểm tra lại email của bạn ngay lập tức để tránh việc bị lạm dụng truy cập trái phép. </p>
        </td>
      </tr>
      <tr>
        <td style='text-align: center; padding-top: 40px; font-size: 14px; color: #aaa;'>
          <p>Trân trọng,<br />Cảm ơn bạn đã lựa chọn chúng tôi!</p>
        </td>
      </tr>
    </table>
  </body>
</html>";

            emailService.SendEmail(email, subject, body);

            return Json(new { success = true, message = "Bạn vừa nhận được voucher giảm giá. Vui lòng kiểm tra email" }, JsonRequestBehavior.AllowGet);
        }

      


        public ActionResult Logout()
        {

            Session.Remove("User");
            return RedirectToAction("Login", "Home_64132985");
        }


        public ActionResult Error()
        {
            return View();
        }
    }
}