using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace NguyenHongKhanh.SachOnline
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // ==========================================
            // ROUTE 1: TRANG CHỦ
            // ==========================================
            routes.MapRoute(
                name: "Trang chu",
                url: "",
                defaults: new { controller = "SachOnline", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "NguyenHongKhanh.SachOnline.Controllers" }
            );

            // ==========================================
            // ROUTE 2: SÁCH THEO CHỦ ĐỀ
            // URL: /sach-theo-chu-de-{id}
            // VD: /sach-theo-chu-de-1
            // ==========================================
            routes.MapRoute(
                name: "Sach theo Chu de",
                url: "sach-theo-chu-de-{id}",
                defaults: new { controller = "SachOnline", action = "SachTheoChuDe", id = UrlParameter.Optional },
                namespaces: new string[] { "NguyenHongKhanh.SachOnline.Controllers" }
            );

            // ==========================================
            // ROUTE 3: SÁCH THEO NHÀ XUẤT BẢN
            // URL: /sach-theo-nxb-{id}
            // VD: /sach-theo-nxb-1
            // ==========================================
            routes.MapRoute(
                name: "Sach theo NXB",
                url: "sach-theo-nxb-{id}",
                defaults: new { controller = "SachOnline", action = "SachTheoNXB", id = UrlParameter.Optional },
                namespaces: new string[] { "NguyenHongKhanh.SachOnline.Controllers" }
            );

            // ==========================================
            // ROUTE 4: CHI TIẾT SÁCH
            // URL: /chi-tiet-sach-{id}
            // VD: /chi-tiet-sach-1
            // ==========================================
            routes.MapRoute(
                name: "Chi tiet sach",
                url: "chi-tiet-sach-{id}",
                defaults: new { controller = "SachOnline", action = "ChiTietSach", id = UrlParameter.Optional },
                namespaces: new string[] { "NguyenHongKhanh.SachOnline.Controllers" }
            );

            // ==========================================
            // ROUTE 5: ĐĂNG KÝ TÀI KHOẢN
            // URL: /dang-ky
            // ==========================================
            routes.MapRoute(
                name: "Dang ky",
                url: "dang-ky",
                defaults: new { controller = "User", action = "Register" },
                namespaces: new string[] { "NguyenHongKhanh.SachOnline.Controllers" }
            );

            // ==========================================
            // ROUTE 6: ĐĂNG NHẬP
            // URL: /dang-nhap
            // ==========================================
            routes.MapRoute(
                name: "Dang nhap",
                url: "dang-nhap",
                defaults: new { controller = "User", action = "Login", url = UrlParameter.Optional },
                namespaces: new string[] { "NguyenHongKhanh.SachOnline.Controllers" }
            );

            // ==========================================
            // ROUTE 7: TRANG TIN (LAB 07)   
            // ==========================================
            routes.MapRoute(
                name: "Trang tin",
                url: "{metatitle}",
                defaults: new { controller = "SachOnline", action = "TrangTin", metatitle = UrlParameter.Optional },
                namespaces: new string[] { "NguyenHongKhanh.SachOnline.Controllers" }
            );

            
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "NguyenHongKhanh.SachOnline.Controllers" }
            );
        }
    }
}