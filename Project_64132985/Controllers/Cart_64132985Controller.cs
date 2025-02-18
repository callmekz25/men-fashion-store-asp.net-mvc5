using Project_64132985.GenerateId;
using Project_64132985.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_64132985.Controllers
{
    public class Cart_64132985Controller : Controller
    {
        // GET: Cart_64132985
        private QL_THOITRANG_FINALEntities db = new QL_THOITRANG_FINALEntities();
        public ActionResult Index()
        {

            var user = Session["User"] as Customer;
            if (user != null)
            {

                var cart = db.Carts.Where(c => c.CustomerID == user.CustomerID).FirstOrDefault();
                if (cart != null)
                {
                    // Truy vấn ra các thông tin của cart của user từ model tự tạo CartProducts
                    var cartDetails = db.CartDetails
                            .Where(cd => cd.CartID == cart.CartID)
                            .Select(cd => new CartProducts_64132985
                            {
                                CartDetailID = cd.CartDetailID,
                                Quantity = cd.Quantity,
                                Size = cd.Size,
                                Color = cd.Color,
                                Product = cd.Product
                            }).ToList();
                    return View(cartDetails);
                }
                else
                {
                    return View(new List<CartProducts_64132985>());
                }


            }
            else
            {
                return RedirectToAction("Login", "Home_64132985");
            }
        }

        [HttpPost]
        public ActionResult Index(string productID, int quantity, string color, string size)
        {
            try
            {
                var user = Session["User"] as Customer;
                if (user == null)
                {
                    return Json(new { success = false, message = "Bạn cần đăng nhập. Nhấn vào đây để đăng nhập." });
                }

                var cart = db.Carts.FirstOrDefault(c => c.CustomerID == user.CustomerID);
                if (cart == null)
                {
                    cart = new Cart
                    {
                        CartID = Guid.NewGuid().ToString(),
                        CustomerID = user.CustomerID,
                    };
                    db.Carts.Add(cart);
                }

                var cartDetail = db.CartDetails.FirstOrDefault(cd => cd.CartID == cart.CartID && cd.ProductID.Trim() == productID.Trim() && cd.Color.Trim() == color.Trim() && cd.Size.Trim() == size.Trim());
                if (cartDetail == null)
                {
                    cartDetail = new CartDetail
                    {
                        CartDetailID = Guid.NewGuid().ToString(),
                        CreatedAt = DateTime.Now,
                        Quantity = quantity,
                        Size = size,
                        Color = color,
                        CartID = cart.CartID,
                        ProductID = productID,
                    };
                    db.CartDetails.Add(cartDetail);
                }
                else
                {
                    cartDetail.Quantity += quantity;
                }

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                // Log lỗi vào file hoặc database
                return Json(new { success = false, message = ex.Message });

            }
            return Json(new { success = true, message = "Thêm vào giỏ hàng thành công" });
        }
        [HttpPost]
        public ActionResult Remove(string productID, string color, string size)
        {
            try
            {
                var user = Session["User"] as Customer;
                if (user == null)
                {
                    return RedirectToAction("Login", "Home_64132985");
                }

                var cart = db.Carts.FirstOrDefault(c => c.CustomerID == user.CustomerID);

                var cartDetail = db.CartDetails.FirstOrDefault(cd => cd.CartID == cart.CartID && cd.ProductID == productID && cd.Color.Trim() == color && cd.Size.Trim() == size);

                db.CartDetails.Remove(cartDetail);


                db.SaveChanges();
            }
            catch (Exception ex)
            {
                // Log lỗi vào file hoặc database
                Console.WriteLine(ex.ToString());
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult UpdateQuantity(string productID, int quantity, string color, string size)
        {
            try
            {
                var user = Session["User"] as Customer;
                if (user == null)
                {
                    return RedirectToAction("Login", "Home_64132985");
                }

                var cart = db.Carts.FirstOrDefault(c => c.CustomerID == user.CustomerID);

                var cartDetail = db.CartDetails.FirstOrDefault(cd => cd.CartID == cart.CartID && cd.ProductID == productID && cd.Color.Trim() == color && cd.Size.Trim() == size);

                cartDetail.Quantity = quantity;

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                // Log lỗi vào file hoặc database
                Console.WriteLine(ex.ToString());
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult GoToCheckOut(List<SelectedProduct_64132985> products)
        {
            Session["SelectedProducts"] = products;
            var user = Session["User"] as Customer;
            IdGenerate_64132985 id = new IdGenerate_64132985();
            var orderId = id.GenerateOrderId();
            if (user == null)
            {
                return Json(new { success = false, message = "Bạn cần đăng nhập. Nhấn vào đây để đăng nhập." });
            }

            return Json(new { success = true, message = "", orderID = orderId });
        }

    }
}