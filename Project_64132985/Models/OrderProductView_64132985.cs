using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_64132985.Models
{
    public class OrderProductView_64132985
    {
        public string OrderID { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public string Status { get; set; } 
        public decimal Total {  get; set; }
        public List<ProductCheckoutView_64132985> Products { get; set; }
    }
}