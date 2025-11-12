using NguyenHongKhanh.SachOnline.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace NguyenHongKhanh.SachOnline.Controllers
{
    public class UserController : Controller
    {
        SachOnlineDataEntities data = new SachOnlineDataEntities();

        // GET: User/DangNhap
        [HttpGet]
        public ActionResult DangNhap(int? id, string url)
        {
            // Lưu URL để redirect sau khi đăng nhập
            ViewBag.UrlReturn = url;
            ViewBag.Id = id;
            return View();
        }

        // POST: User/DangNhap
        [HttpPost]
        public ActionResult DangNhap(FormCollection f, string url)
        {
            var sTenDN = f["TenDN"];
            var sMatKhau = f["MatKhau"];

            if (String.IsNullOrEmpty(sTenDN))
            {
                ViewData["Err1"] = "Bạn chưa nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(sMatKhau))
            {
                ViewData["Err2"] = "Bạn chưa nhập mật khẩu";
            }
            else
            {
                KHACHHANG kh = data.KHACHHANGs.SingleOrDefault(
                    n => n.TaiKhoan == sTenDN && n.MatKhau == sMatKhau);

                if (kh != null)
                {
                    Session["TaiKhoan"] = kh;

                    // Xử lý redirect
                    if (!String.IsNullOrEmpty(url))
                    {
                        return Redirect(url);
                    }
                    else
                    {
                        return RedirectToAction("Index", "SachOnline");
                    }
                }
                else
                {
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            }
            return View();
        }

        // Đăng xuất
        public ActionResult DangXuat()
        {
            Session["TaiKhoan"] = null;
            return RedirectToAction("Index", "SachOnline");
        }
    }
}