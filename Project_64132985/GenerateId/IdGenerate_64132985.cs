using Project_64132985.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_64132985.GenerateId
{
    public class IdGenerate_64132985
    {
        private QL_THOITRANG_FINALEntities db = new QL_THOITRANG_FINALEntities();
        public string GenerateCustomerId()
        {
            var maxId = db.Customers.OrderByDescending(c => c.CustomerID).Select(c => c.CustomerID).FirstOrDefault();
            if (string.IsNullOrEmpty(maxId))
            {
                return "C001";
            }
            var numberPart = maxId.Substring(1);
            if (int.TryParse(numberPart, out int numericId))
            {
                // Tăng số lên 1 và ghép lại thành ID mới
                numericId++;
                return "C" + numericId.ToString("D3");
            }

            // Xử lý lỗi nếu không parse được
            throw new InvalidOperationException("Invalid ID format in database.");
        }
        public string GenerateOrderId()
        {
            var maxId = db.OrderCustomers.OrderByDescending(o => o.OrderID).Select(o => o.OrderID).FirstOrDefault();
            if (string.IsNullOrEmpty(maxId))
            {
                return "O001";
            }
            var numberPart = maxId.Substring(1);
            if (int.TryParse(numberPart, out int numericId))
            {
                // Tăng số lên 1 và ghép lại thành ID mới
                numericId++;
                return "O" + numericId.ToString("D3");
            }

            // Xử lý lỗi nếu không parse được
            throw new InvalidOperationException("Invalid ID format in database.");
        }
        public string GenerateOrderDetailId()
        {
            var maxId = db.OrderCustomerDetails.OrderByDescending(o => o.OrderDetailID).Select(o => o.OrderDetailID).FirstOrDefault();
            if (string.IsNullOrEmpty(maxId))
            {
                return "OD001";
            }
            var numberPart = maxId.Substring(2);
            if (int.TryParse(numberPart, out int numericId))
            {
                // Tăng số lên 1 và ghép lại thành ID mới
                numericId++;
                return "OD" + numericId.ToString("D3");
            }

            // Xử lý lỗi nếu không parse được
            throw new InvalidOperationException("Invalid ID format in database.");
        }
        public string GenerateProductId()
        {
            var maxId = db.Products.OrderByDescending(o => o.ProductID).Select(o => o.ProductID).FirstOrDefault();
            if (string.IsNullOrEmpty(maxId))
            {
                return "P001";
            }
            var numberPart = maxId.Substring(1);
            if (int.TryParse(numberPart, out int numericId))
            {
                // Tăng số lên 1 và ghép lại thành ID mới
                numericId++;
                return "P" + numericId.ToString("D3");
            }

            // Xử lý lỗi nếu không parse được
            throw new InvalidOperationException("Invalid ID format in database.");
        }
        public string GenerateEmployeeId()
        {
            var maxId = db.Employees.OrderByDescending(o => o.EmployeeID).Select(o => o.EmployeeID).FirstOrDefault();
            if (string.IsNullOrEmpty(maxId))
            {
                return "E001";
            }
            var numberPart = maxId.Substring(1);
            if (int.TryParse(numberPart, out int numericId))
            {
                // Tăng số lên 1 và ghép lại thành ID mới
                numericId++;
                return "E" + numericId.ToString("D3");
            }

            // Xử lý lỗi nếu không parse được
            throw new InvalidOperationException("Invalid ID format in database.");
        }
    }
}