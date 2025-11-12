using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NguyenHongKhanh.SachOnline.Models;
using PagedList;
using PagedList.Mvc;


namespace NguyenHongKhanh.SachOnline.Controllers
{
    public class SachOnlineController : Controller
    {
        // GET: SachOnline
        public ActionResult Index(int? page)
        {
            int pageSize = 6; // Số sản phẩm trên 1 trang
            int pageNumber = (page ?? 1); // Trang hiện tại, mặc định là trang 1

            // Lấy danh sách sách mới
            var listSachMoi = data.SACHes
                                  .OrderByDescending(a => a.NgayCapNhat)
                                  .ToList();

            return View(listSachMoi.ToPagedList(pageNumber, pageSize));
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

        public ActionResult SachTheoChuDe(int id, int? page)
        {
            int pageSize = 6; // Số sản phẩm trên 1 trang
            int pageNumber = (page ?? 1); // Trang hiện tại, mặc định là trang 1

            var kq = (from s in data.SACHes
                      where s.MaCD == id
                      select s).ToList();

            ViewBag.MaCD = id;

            return View(kq.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult SachTheoNXB(int? id, int? page)
        {
            int pageSize = 6;
            int pageNumber = (page ?? 1);

            var kq = (from s in data.SACHes
                      where s.MaNXB == id
                      select s).ToList();

            ViewBag.MaNXB = id;

            return View(kq.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult ChiTietSach(int id)
        {
            var sach = from s in data.SACHes
                       where s.MaSach == id
                       select s;
            return View(sach.Single());
        }

        public ActionResult LoginLogoutPartial()
        {
            return PartialView();
        }

        // Lab 07: Menu động
        [ChildActionOnly]
        public ActionResult NavPartial()
        {
            List<MENU> lst = data.MENUs.Where(m => m.ParentId == null).OrderBy(m => m.OrderNumber).ToList();

            int[] a = new int[lst.Count()];
            for (int i = 0; i < lst.Count; i++)
            {
                int id = lst[i].Id;
                List<MENU> l = data.MENUs.Where(m => m.ParentId == id).ToList();
                int k = l.Count();
                a[i] = k;
            }
            ViewBag.lst = a;

            return PartialView(lst);
        }

        [ChildActionOnly]
        public ActionResult LoadChildMenu(int parentId)
        {
            List<MENU> lst = new List<MENU>();
            lst = data.MENUs.Where(m => m.ParentId == parentId).OrderBy(m => m.OrderNumber).ToList();
            ViewBag.Count = lst.Count();
            int[] a = new int[lst.Count()];
            for (int i = 0; i < lst.Count; i++)
            {
                int id = lst[i].Id;
                List<MENU> l = data.MENUs.Where(m => m.ParentId == id).ToList();
                int k = l.Count();
                a[i] = k;
            }
            ViewBag.lst = a;
            return PartialView("LoadChildMenu", lst);
        }

        // Lab 07: Trang tin
        public ActionResult TrangTin(string metatitle)
        {
            var tt = (from t in data.TRANGTINs where t.MetaTitle == metatitle select t).SingleOrDefault();
            if (tt == null)
            {
                return HttpNotFound();
            }
            return View(tt);
        }
    }
}
