using Project_64132985.App_Start;
using Project_64132985.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_64132985.Areas.Admin.Controllers
{
    public class AdminVoucher_64132985Controller : Controller
    {
        // GET: Admin/AdminVoucher_64132985
        private QL_THOITRANG_FINALEntities db = new QL_THOITRANG_FINALEntities();
        [AdminAuthorize_64132985(Role = "admin")]
        public ActionResult Index(string id = "", DateTime? fromDate = null, DateTime? toDate = null, int discount = 0, string active = "")
        {
            var ids = db.Vouchers.Select(o => o.VoucherID).Take(6).ToList();
            ViewBag.Ids = ids;
            var vouchers = db.Vouchers.AsQueryable();
            int numberFilter = 0;
            if (string.IsNullOrEmpty(id))
            {
                if (fromDate.HasValue && toDate.HasValue)
                {
                    var from = fromDate.Value.Date;
                    var to = toDate.Value.Date;
                    vouchers = vouchers.Where(c => DbFunctions.TruncateTime(c.CreatedAt) >= from && DbFunctions.TruncateTime(c.CreatedAt) <= to);
                    numberFilter += 1;
                }
                if (!string.IsNullOrEmpty(active))
                {
                    var currentDate = DateTime.Now.Date;
                    bool isActive = active == "true";
                    if (isActive)
                    {

                        vouchers = vouchers.Where(c => DbFunctions.TruncateTime(c.Expired) >= currentDate);
                    } else
                    {
                        vouchers = vouchers.Where(c => DbFunctions.TruncateTime(c.Expired) <= currentDate);
                    }
                    numberFilter += 1;
                }
                if(discount > 0)
                {
                    vouchers = vouchers.Where(c => c.Discount == discount);
                    numberFilter += 1;
                }
                ViewBag.NumberFilter = numberFilter;
                ViewBag.To = toDate;
                ViewBag.From = fromDate;
                ViewBag.Discount = discount;
                ViewBag.Active = active;

                var filteredCustomer = vouchers.ToList();
                if (filteredCustomer.Count() == 0)
                {
                    ViewBag.Result = "Không tìm thấy kết quả phù hợp";
                }
                return View(vouchers);
            }
            else
            {
                var listVoucher = new List<Voucher>();
                var voucher = db.Vouchers.Find(id);

                if (voucher == null)
                {
                    ViewBag.Result = "Không tìm thấy kết quả phù hợp";
                    return View(listVoucher);
                }
                else
                {
                    listVoucher.Add(voucher);
                    return View(listVoucher);
                }
            }
        }
        [AdminAuthorize_64132985(Role = "admin")]
        public ActionResult AddVoucher()
        {
            return View();
        }
        [AdminAuthorize_64132985(Role = "admin")]
        [HttpPost]
        public ActionResult AddVoucher(string id, string descr, int discount, DateTime expired)
        {
            var exist = db.Vouchers.Find(id);
            if (exist == null)
            {

                if (string.IsNullOrEmpty(descr))
                {
                    var voucher = new Voucher()
                    {
                        VoucherID = id,
                        Discount = discount,
                        Expired = expired,
                        CreatedAt = DateTime.Now,
                    };
                    db.Vouchers.Add(voucher);
                }
                else
                {
                    var voucher = new Voucher()
                    {
                        VoucherID = id,
                        Descr = descr,
                        Discount = discount,
                        Expired = expired,
                        CreatedAt = DateTime.Now,
                    };
                    db.Vouchers.Add(voucher);
                }
                db.SaveChanges();
                return Json(new { success = true, message = "Thêm mã giảm giá thành công" });
            }
            else
            {
                return Json(new { success = false, message = "Đã tồn tại mã giảm giá" });
            }

        }
        [AdminAuthorize_64132985(Role = "admin")]
        public ActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Error", "Home_64132985", new { area = "" });
            }
            var voucher = db.Vouchers.Find(id);
            return View(voucher);
        }
        [AdminAuthorize_64132985(Role = "admin")]
        [HttpPost]
        public ActionResult Edit(string id, string descr, int discount, DateTime expired)
        {
            var exist = db.Vouchers.Find(id);
            if (exist != null)
            {

                bool isChanged = false;
                if (descr != exist.Descr)
                {
                    isChanged = true;
                    exist.Descr = descr;
                }
                if (discount != exist.Discount)
                {
                    isChanged = true;
                    exist.Discount = discount;
                }
                if (expired != exist.Expired)
                {
                    isChanged = true;
                    exist.Expired = expired;
                }
                if (isChanged)
                {
                    db.SaveChanges();
                }
                return Json(new { success = true, message = "Cập nhật mã giảm giá thành công" });
            }
            else
            {
                return Json(new { success = false, message = "Lỗi" });
            }
        }
        [AdminAuthorize_64132985(Role = "admin")]
        [HttpPost]
        public ActionResult Delete(string id)
        {
            var voucher = db.Vouchers.Find(id);
            if (voucher != null)
            {
                db.Vouchers.Remove(voucher);
                db.SaveChanges();
                return Json(new { success = true, message = "Xóa mã giảm giá thành công" });
            }
            else
            {
                return Json(new { success = false, message = "Lỗi không tìm thấy mã giảm giá" });
            }
        }
    }
}