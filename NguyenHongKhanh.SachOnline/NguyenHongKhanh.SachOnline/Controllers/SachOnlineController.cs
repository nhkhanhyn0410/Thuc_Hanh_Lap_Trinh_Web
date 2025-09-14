using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NguyenHongKhanh.SachOnline.Models;


namespace NguyenHongKhanh.SachOnline.Controllers
{
    public class SachOnlineController : Controller
    {
        // GET: SachOnline
        public ActionResult Index()
        {
            var listSachMoi = LaySachMoi(6);
            return View(listSachMoi);
        }

        public ActionResult ChuDePartial()
        {
            var listChuDe = from cd in data.CHUDEs select cd;
            return PartialView(listChuDe);
        }

        public ActionResult SachBanNhieuPartial()
        {
            var listSachBanChay = LaySachBanNhieu(6);
            return PartialView(listSachBanChay);
        }

        public ActionResult NhaXuatBanPartial()
        {
            
            var listNXB = from nxb in data.NHAXUATBANs select nxb;
            return PartialView(listNXB);
        }

        public ActionResult NavbarHeaderPartial()
        {
            return PartialView();
        }

        public ActionResult AdsPartial()
        {
            return PartialView();
        }

        SachOnlineDataEntities data = new SachOnlineDataEntities();

        
        private List<SACH> LaySachMoi(int count)
        {
            return data.SACHes.OrderByDescending(a => a.NgayCapNhat).Take(count).ToList();
        }

        private List<SACH> LaySachBanNhieu(int count)
        {
            return data.SACHes.OrderByDescending(a => a.SoLuongBan).Take(count).ToList();
        }


       
        
    }
}
