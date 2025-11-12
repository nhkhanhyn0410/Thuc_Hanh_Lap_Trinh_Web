using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace NguyenHongKhanh.SachOnline.Filters
{
    /// <summary>
    /// Custom Authorization Attribute cho Admin Area
    /// Kiểm tra xem Admin đã đăng nhập chưa trước khi cho phép truy cập
    /// </summary>
    public class AdminAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            // Kiểm tra Session["Admin"] có tồn tại không
            var adminSession = httpContext.Session["Admin"];

            if (adminSession != null)
            {
                return true;
            }

            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            // Nếu chưa đăng nhập, redirect về trang Login của Admin
            filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary(new
                {
                    area = "Admin",
                    controller = "Home",
                    action = "Login"
                })
            );
        }
    }
}
