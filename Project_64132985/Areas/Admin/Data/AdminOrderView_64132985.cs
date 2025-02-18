using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_64132985.Areas.Admin.Data
{
    public class AdminOrderView_64132985
    {
        public string OrderID { get; set; }
        public string EmployeeName { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public string Status { get; set; }
        public int Quantity { get; set; }
        public int Total { get; set; }
    }
}