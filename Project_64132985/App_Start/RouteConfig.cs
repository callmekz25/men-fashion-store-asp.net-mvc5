using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Project_64132985
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "Login",
                url: "Login",
                defaults: new { controller = "Home_64132985", action = "Login", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Sigup",
                url: "Sigup",
                defaults: new { controller = "Home_64132985", action = "Signup", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "ProductDetail",
                url: "ProductDetail/{id}",
                defaults: new { controller = "Products_64132985", action = "ProductDetail", id = UrlParameter.Optional },
                 constraints: new { id = @"^[a-zA-Z0-9\-]+$" }

            );
            routes.MapRoute(
               name: "Account",
               url: "Account",
               defaults: new { controller = "Account_64132985", action = "Index", id = UrlParameter.Optional }
           );
            routes.MapRoute(
               name: "Address",
               url: "Address",
               defaults: new { controller = "Account_64132985", action = "Address", id = UrlParameter.Optional }
           );
           
            routes.MapRoute(
              name: "Password",
              url: "Password",
              defaults: new { controller = "Account_64132985", action = "ChangePassword", id = UrlParameter.Optional }
          );

            routes.MapRoute(
              name: "AccountDetail",
              url: "Detail",
              defaults: new { controller = "Account_64132985", action = "AccountDetail", id = UrlParameter.Optional }
          );
            routes.MapRoute(
             name: "Cart",
             url: "Cart",
             defaults: new { controller = "Cart_64132985", action = "Index", id = UrlParameter.Optional }
         );
            routes.MapRoute(
          name: "AfterCheckout",
          url: "Notify/{id}",
          defaults: new { controller = "Order_64132985", action = "AfterCheckout", id = UrlParameter.Optional },
          constraints: new { id = @"^[a-zA-Z0-9\-]+$" }

        );
            routes.MapRoute(
             name: "Checkout",
             url: "Checkout/{id}",
             defaults: new { controller = "Order_64132985", action = "Index", id = UrlParameter.Optional },
             constraints: new { id = @"^[a-zA-Z0-9\-]+$" }

         );
            routes.MapRoute(
            name: "CartGoToCheckout",
            url: "Cart/GoToCheckout",
            defaults: new { controller = "Cart_64132985", action = "GoToCheckout"}
          );

            routes.MapRoute(
          name: "OurProduct",
          url: "OurProduct",
          defaults: new { controller = "Products_64132985", action = "OurProduct" }
        );

            routes.MapRoute(
         name: "ErrorPage",
         url: "Error",
         defaults: new { controller = "Home_64132985", action = "Error" }
       );
            routes.MapRoute(
          name: "CancelOrder",
          url: "Account/Cancel",
          defaults: new { controller = "Order_64132985", action = "CancelOrder" }
        );



            routes.MapRoute(
                name: "",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home_64132985", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
