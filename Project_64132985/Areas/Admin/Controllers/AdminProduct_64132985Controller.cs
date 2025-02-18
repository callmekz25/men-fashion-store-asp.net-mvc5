using Project_64132985.App_Start;
using Project_64132985.GenerateId;
using Project_64132985.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.UI.WebControls;
namespace Project_64132985.Areas.Admin.Controllers
{
    public class AdminProduct_64132985Controller : Controller
    {
        private QL_THOITRANG_FINALEntities db = new QL_THOITRANG_FINALEntities();
        // GET: Admin/Product_64132985
        [AdminAuthorize_64132985(Role = "admin,employee")]
        public ActionResult Index(string id = "", string name = "", int price = 0, string category = "", DateTime? fromDate = null, DateTime? toDate = null, string colors = "", string sizes = "")
        {
            var user = Session["User"] as Employee;
            if (user == null || user.Roles == null)
            {
                return RedirectToAction("Error", "Home_64132985", new { area = "" });

            }
            var ids = db.Products.Select(p => p.ProductID).Take(6).ToList();
            ViewBag.Ids = ids;
            var products = db.Products.AsQueryable();
            int numberFilter = 0;
            ViewBag.Id = id;
            ViewBag.User = user;
            if (string.IsNullOrEmpty(id))
            {

                if (!string.IsNullOrEmpty(name))
                {
                    products = products.Where(p => p.ProductName.Contains(name));
                    numberFilter += 1;
                }
                if (fromDate.HasValue && toDate.HasValue)
                {
                    var from = fromDate.Value.Date;
                    var to = toDate.Value.Date;
                    products = products.Where(p => (p.CreatedAt) >= from && (p.CreatedAt) <= to);
                    numberFilter += 1;
                }
                // Lọc theo giá nếu `price` có giá trị
                if (price > 100000)
                {
                    numberFilter += 1;
                    products = products.Where(p => p.Price <= price);
                }

                // Lọc theo danh mục nếu `category` không trống
                if (!string.IsNullOrEmpty(category))
                {
                    numberFilter += 1;
                    products = products.Where(p => p.Category == category);
                }

              
                
                if (!string.IsNullOrEmpty(colors))
                {
                    var colorsList = colors.Split(',').ToList();
                    numberFilter += 1;
                    if (colorsList.Any())
                    {

                        products = products.Where(p => colorsList.Any(c => p.Colors.Contains(c)));
                    }
                }

                if (!string.IsNullOrEmpty(sizes))
                {
                    var sizesList = sizes.Split(',').ToList();
                    numberFilter += 1;
                    if (sizesList.Any())
                    {

                        products = products.Where(p => sizesList.Any(s => p.Sizes.Contains(s)));
                    }
                }
                ViewBag.NumberFilter = numberFilter;
                ViewBag.From = fromDate;
                ViewBag.To = toDate;
                ViewBag.ProductName = name;
                ViewBag.Price = price;
                ViewBag.SelectedCategory = category;
           
                ViewBag.Colors = colors;
                ViewBag.Sizes = sizes;
                ViewBag.Categories = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Áo thun", Value = "thun" },
                    new SelectListItem { Text = "Áo hoodie", Value = "hoodie" },
                    new SelectListItem { Text = "Áo khoác", Value = "khoác" },
                    new SelectListItem { Text = "Áo sơ mi", Value = "sơ mi" },
                    new SelectListItem { Text = "Áo polo", Value = "polo" },
                    new SelectListItem { Text = "Áo sweater", Value = "sweater" }
                };
                var filteredProducts = products.ToList();
                if (filteredProducts.Count() <= 0)
                {
                    ViewBag.Result = "Không tìm thấy kết quả phù hợp";
                }
                
                return View(filteredProducts);
            }
            else
            {
                var listProducts = new List<Product>();
                var product = db.Products.Find(id);
              
                ViewBag.Categories = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Áo thun", Value = "thun" },
                    new SelectListItem { Text = "Áo hoodie", Value = "hoodie" },
                    new SelectListItem { Text = "Áo khoác", Value = "khoác" },
                    new SelectListItem { Text = "Áo sơ mi", Value = "sơ mi" },
                    new SelectListItem { Text = "Áo polo", Value = "polo" },
                    new SelectListItem { Text = "Áo sweater", Value = "sweater" }
                };
                if (product != null)
                {
                    listProducts.Add(product);
                }
                else
                {
                    ViewBag.Result = "Không tìm thấy kết quả tìm kiếm";

                }
                return View(listProducts);
            }
        }

        [AdminAuthorize_64132985(Role = "admin")]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }
        [AdminAuthorize_64132985(Role = "admin")]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(string name, string descr, int price, string category, string color, string size)
        {
            IdGenerate_64132985 id = new IdGenerate_64132985();
            var imageProduct = Request.Files["img"];
            string postedFileName = System.IO.Path.GetFileName(imageProduct.FileName);
            var path = Server.MapPath("/Images/Products/" + postedFileName);
            imageProduct.SaveAs(path);
            var product = new Product()
            {
                ProductID = id.GenerateProductId(),
                ProductName = name,
                Descr = descr,
                Price = price,
                Category = category,
                Img = postedFileName,
                Colors = color,
                Sizes = size,
                CreatedAt = DateTime.Now,
            };
            db.Products.Add(product);
            db.SaveChanges();
            return Json(new { success = true, message = "Thêm sản phẩm thành công" });
        }

        [HttpPost]
        public ActionResult Edit(string id, string name, string descr, int price, string category, string color, string size)
        {
            if (id == null)
            {
                return Json(new { success = false, message = "Id không được trống" });
            }

            Product product = db.Products.Find(id);
            if (product == null)
            {
                return Json(new { success = false, message = "Không tìm thấy sản phẩm" });
            }

            var imageProduct = Request.Files["img"];
            string postedFileName = "";
            if (imageProduct != null)
            {

                postedFileName = System.IO.Path.GetFileName(imageProduct.FileName);
            }

            bool isChanged = false; // Biến đánh dấu có thay đổi không

            // Kiểm tra và chỉ cập nhật khi có sự thay đổi
            if (product.ProductName != name)
            {
                product.ProductName = name;
                isChanged = true;
            }
            if (product.Descr != descr)
            {
                product.Descr = descr;
                isChanged = true;
            }
          
            if (product.Price != price)
            {
                product.Price = price;
                isChanged = true;
            }
            if (product.Category != category)
            {
                product.Category = category;
                isChanged = true;
            }
            if (product.Img != postedFileName && postedFileName != "")
            {
                product.Img = postedFileName;
                var path = Server.MapPath("/Images/Products/" + postedFileName);
                imageProduct.SaveAs(path);
                isChanged = true;
            }
            if (product.Colors != color)
            {
                product.Colors = color;
                isChanged = true;
            }
            if (product.Sizes != size)
            {
                product.Sizes = size;
                isChanged = true;
            }

            // Nếu có thay đổi, lưu lại
            if (isChanged)
            {
                product.UpdatedAt = DateTime.Now;
                db.SaveChanges();
            }

            return Json(new { success = true, message = "Cập nhật thành công" }); // Hoặc trả về view khác
        }
        [HttpPost]
        public ActionResult Delete(string id)
        {
            var product = db.Products.FirstOrDefault(p => p.ProductID == id);
            db.Products.Remove(product);
            db.SaveChanges();
            return Json(new { success = true, message = "Xóa sản phẩm thành công" });
        }

    }
}