using NguyenHongKhanh.SachOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NguyenHongKhanh.SachOnline.Controllers
{
    public class GioHangController : Controller
    {
        SachOnlineDataEntities data = new SachOnlineDataEntities();
        public List<GioHang> LayGioHang()
        {
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang == null)
            {
                lstGioHang = new List<GioHang>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
        }

        public ActionResult ThemGioHang(int ms, string url)
        {
            // Lấy giỏ hàng hiện tại
            List<GioHang> lstCart = LayGioHang();

            // Kiểm tra nếu sản phẩm chưa có trong giỏ thì thêm vào, nếu đã có thì tăng số lượng
            GioHang sp = lstCart.Find(n => n.iMaSach == ms);
            if (sp == null)
            {
                sp = new GioHang(ms);
                lstCart.Add(sp);
            }
            else
            {
                sp.iSoLuong++;
            }
            return Redirect(url);
        }

        // Tính tổng số lượng
        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                iTongSoLuong = lstGioHang.Sum(n => n.iSoLuong);
            }
            return iTongSoLuong;
        }

        // Tính tổng tiền
        private double TongTien()
        {
            double dTongTien = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                dTongTien = lstGioHang.Sum(n => n.dThanhTien);
            }
            return dTongTien;
        }

        // Xóa sản phẩm khỏi giỏ hàng
        public ActionResult XoaSPKhoiGioHang(int iMaSach)
        {
            List<GioHang> lstGioHang = LayGioHang();
            // Kiểm tra Sach đã có trong Session["GioHang"]
            GioHang sp = lstGioHang.SingleOrDefault(n => n.iMaSach == iMaSach);
            // Xóa sp khỏi giỏ hàng
            if (sp != null)
            {
                lstGioHang.RemoveAll(n => n.iMaSach == iMaSach);
                if (lstGioHang.Count == 0)
                {
                    return RedirectToAction("Index", "SachOnline");
                }
            }
            return RedirectToAction("GioHang");
        }

        // Cập nhật giỏ hàng
        public ActionResult CapNhatGioHang(int iMaSach, FormCollection f)
        {
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sp = lstGioHang.SingleOrDefault(n => n.iMaSach == iMaSach);
            // Nếu tồn tại thì cho sửa số lượng
            if (sp != null)
            {
                sp.iSoLuong = int.Parse(f["txtSoLuong"].ToString());
            }
            return RedirectToAction("GioHang");
        }

        public ActionResult XoaGioHang()
        {
            List<GioHang> lstGioHang = LayGioHang();
            lstGioHang.Clear();
            return RedirectToAction("Index", "SachOnline");
        }

        public ActionResult GioHang()
        {
            List<GioHang> lstGioHang = LayGioHang();
            if (lstGioHang.Count == 0)
            {
                return RedirectToAction("Index", "SachOnline");
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(lstGioHang);
        }

        public ActionResult GioHangPartial()
        {
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return PartialView();
        }


        [HttpGet]
        public ActionResult DatHang()
        {
            // Kiểm tra đăng nhập
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "User");
            }

            // Kiểm tra giỏ hàng
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "SachOnline");
            }

            List<GioHang> lstGioHang = LayGioHang();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();

            return View(lstGioHang);
        }

        [HttpPost]
        public ActionResult DatHang(FormCollection f)
        {
            DONDATHANG ddh = new DONDATHANG();
            KHACHHANG kh = (KHACHHANG)Session["TaiKhoan"];
            List<GioHang> lstCart = LayGioHang();

            ddh.MaKH = kh.MaKH;
            ddh.NgayDat = DateTime.Now;
            ddh.NgayGiao = DateTime.Parse(f["NgayGiao"]);
            ddh.DaThanhToan = false;
            ddh.TinhTrangGiaoHang = 0;
            data.DONDATHANGs.Add(ddh);
            data.SaveChanges();

            // Thêm chi tiết đơn hàng
            foreach (var item in lstCart)
            {
                CHITIETDATHANG ctdh = new CHITIETDATHANG();
                ctdh.MaDonHang = ddh.MaDonHang;
                ctdh.MaSach = item.iMaSach;
                ctdh.SoLuong = item.iSoLuong;
                ctdh.DonGia = (decimal)item.dDonGia;
                data.CHITIETDATHANGs.Add(ctdh);
            }
            data.SaveChanges();

            Session["Cart"] = null;
            return RedirectToAction("XacNhanDonHang", "GioHang");
        }

        public ActionResult XacNhanDonHang()
        {
            return View();
        }
    }
}