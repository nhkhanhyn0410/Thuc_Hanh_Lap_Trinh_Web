using NguyenHongKhanh.SachOnline.Filters;
using NguyenHongKhanh.SachOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NguyenHongKhanh.SachOnline.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        SachOnlineDataEntities data = new SachOnlineDataEntities();

        // GET: Admin/Home
        [AdminAuthorize]
        public ActionResult Index()
        {
            return View();
        }

        // GET: Admin/Home/Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        // POST: Admin/Home/Login
        [HttpPost]
        public ActionResult Login(FormCollection f)
        {
            var sTenDN = f["UserName"];
            var sMatKhau = f["Password"];

            ADMIN ad = data.ADMINs.SingleOrDefault(n =>
                n.TenDN == sTenDN && n.MatKhau == sMatKhau);

            if (ad != null)
            {
                Session["Admin"] = ad;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }
    }
}