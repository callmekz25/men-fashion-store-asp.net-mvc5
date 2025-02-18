using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Project_64132985.Models;


namespace Project_64132985.Models
{
    public class CartProducts_64132985
    {
        public string CartDetailID { get; set; }
        public int Quantity { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public Product Product { get; set; }
    }
}