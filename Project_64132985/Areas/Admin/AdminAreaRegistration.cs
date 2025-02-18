using System.Web.Mvc;

namespace Project_64132985.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Admin",
                "Admin",
                new { controller = "Login_64132985" ,action = "Index", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "Dashboard",
                "Admin/Dashboard",
                new { controller = "Dashboard_64132985", action = "Index", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "Product",
                "Admin/Product",
                new { controller = "AdminProduct_64132985", action = "Index", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "Customer",
                "Admin/Customer",
                new { controller = "Customer_64132985", action = "Index", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "DeleteCustomer",
                "Admin/Customer/Delete",
                new { controller = "Customer_64132985", action = "BlockCustomer", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "Employee",
                "Admin/Employee",
                new { controller = "AdminEmployee_64132985", action = "Index", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "AddEmployee",
                "Admin/Employee/Add",
                new { controller = "AdminEmployee_64132985", action = "Add", id = UrlParameter.Optional }
            );
            context.MapRoute(
               "UpdateAccount",
               "Admin/Account/Update",
               new { controller = "AdminAccount_64132985", action = "UpdateAccount", id = UrlParameter.Optional }
           );
            context.MapRoute(
               "DeleteEmployee",
               "Admin/Employee/Delete",
               new { controller = "AdminEmployee_64132985", action = "Delete", id = UrlParameter.Optional }
           );

            context.MapRoute(
               "EditProduct",
               "Product/Edit/{id}",
               new { controller = "AdminProduct_64132985", action = "Edit", id = UrlParameter.Optional }
           );
            context.MapRoute(
               "AddProduct",
               "Product/Add",
               new { controller = "AdminProduct_64132985", action = "Add", id = UrlParameter.Optional }
           );
            context.MapRoute(
               "DeleteProduct",
               "Product/Delete",
               new { controller = "AdminProduct_64132985", action = "Delete", id = UrlParameter.Optional }
           );
            context.MapRoute(
               "Orders",
               "Admin/Order",
               new { controller = "AdminOrder_64132985", action = "Index", id = UrlParameter.Optional }
           );
            context.MapRoute(
               "OrderDetail",
               "Admin/Order/Detail",
               new { controller = "AdminOrder_64132985", action = "Detail", id = UrlParameter.Optional }
           );
            context.MapRoute(
               "OrderConfirmed",
               "Admin/Order/Confirm",
               new { controller = "AdminOrder_64132985", action = "ConfirmOrder", id = UrlParameter.Optional }
           );
            context.MapRoute(
               "GetDataToDashboard",
               "Admin/Dashboard",
               new { controller = "Dashboard_64132985", action = "GetData", id = UrlParameter.Optional }
           );
            context.MapRoute(
              "Error",
              "Admin/Error",
              new { controller = "Error_64132985", action = "Index", id = UrlParameter.Optional }
          );
            context.MapRoute(
              "AdminAccountDetail",
              "Admin/Account",
              new { controller = "AdminAccount_64132985", action = "Index", id = UrlParameter.Optional }
          );
            context.MapRoute(
             "AdminVoucher",
             "Admin/Voucher",
             new { controller = "AdminVoucher_64132985", action = "Index", id = UrlParameter.Optional }
         );
            context.MapRoute(
         "AdminAddVoucher",
         "Admin/Voucher/Add",
         new { controller = "AdminVoucher_64132985", action = "AddVoucher", id = UrlParameter.Optional }
     ); context.MapRoute(
         "AdminEditVoucher",
         "Admin/Voucher/Edit/{id}",
         new { controller = "AdminVoucher_64132985", action = "Edit", id = UrlParameter.Optional }
     );
            context.MapRoute(
        "AdminDeleteVoucher",
        "Admin/Voucher/Delete",
        new { controller = "AdminVoucher_64132985", action = "Delete", id = UrlParameter.Optional }
    );
            context.MapRoute(
         "Admin_default",
         "Admin/{controller}/{action}/{id}",
         new { action = "Index", id = UrlParameter.Optional }
     );




        }
    }
}