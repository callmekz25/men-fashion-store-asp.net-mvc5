using Project_64132985.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_64132985.Services
{
    public class RandomVoucher_64132985
    {
        private QL_THOITRANG_FINALEntities db = new QL_THOITRANG_FINALEntities();
        public string GetRandomVoucher()
        {
            var vouchers = db.Vouchers.ToList();
            var random = new Random();
            var randomVoucher = vouchers[random.Next(vouchers.Count)];
            return randomVoucher.VoucherID;
        }
    }
}