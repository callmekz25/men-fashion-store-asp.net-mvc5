using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Project_64132985.Models;

namespace Project_64132985.Controllers
{
    public class Products_64132985Controller : Controller
    {
        private QL_THOITRANG_FINALEntities db = new QL_THOITRANG_FINALEntities();


        // GET: Products/Details/5
        public ActionResult ProductDetail(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Error", "Home_64132985");
            }
            Product product = db.Products.Find(id);
            ViewBag.RelatedProducts = db.Products.Where(p => p.Category == product.Category).Take(4).ToList();
            if (product == null)
            {
                return RedirectToAction("Error", "Home_64132985");
            }
            return View(product);
        }
        [HttpGet]
        public JsonResult GetProductsData()
        {            
            var products = db.Products.ToList();
            var mylist = new List<Object>();
            foreach (var p in products)
            {
                mylist.Add(new
                {
                    ProductID = p.ProductID,
                    ProductName = p.ProductName,
                    Img = p.Img,
                    Price = p.Price,
                    Descr = p.Descr,
                    Category = p.Category,
                    Created = p.CreatedAt,
                    Colors = p.Colors,
                    Sizes = p.Sizes,
                });
            }  
            return Json(mylist, JsonRequestBehavior.AllowGet); 
        }
        public ActionResult OurProduct()
        {
            var uniqueCategories = db.Products.Select(p => p.Category)
            .Distinct()
            .ToList();
            var colors = db.Products.Select(p => new {p.Colors}).ToList();
            var uniqueColors = colors
            .SelectMany(p => p.Colors.Split(',')) // Tách các màu sắc thành mảng
            .Select(color => color.Trim())      // Loại bỏ khoảng trắng thừa
            .Distinct()                         // Loại bỏ màu trùng lặp
            .ToList();
            ViewBag.Categories = uniqueCategories;
            ViewBag.Colors = uniqueColors;
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
