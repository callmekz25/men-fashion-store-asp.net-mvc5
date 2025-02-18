using Project_64132985.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Routing;

namespace Project_64132985.App_Start
{
    public class AdminAuthorize_64132985 : AuthorizeAttribute
    {
        public string Role { get; set; }
        private QL_THOITRANG_FINALEntities db = new QL_THOITRANG_FINALEntities();
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var user = HttpContext.Current.Session["user"];
            if (user == null)
            {

                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(
                        new { controller = "Home_64132985", action = "Login", area = "" }));
            }
            else
            {
                if (user is Employee emp)
                {

                    var rolesArray = Role.Split(',');
                    var count = db.Employees.Count(e => e.EmployeeID == emp.EmployeeID && rolesArray.Contains(e.Roles));

                    if (count == 0)
                    {
                        filterContext.Result = new RedirectToRouteResult(
                            new RouteValueDictionary(
                                new { controller = "Error_64132985", action = "Index", area = "Admin" }));
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    filterContext.Result = new RedirectToRouteResult(
                   new RouteValueDictionary(
                       new { controller = "Home_64132985", action = "Error", area = "" }));
                }
            }

        }
    }
}