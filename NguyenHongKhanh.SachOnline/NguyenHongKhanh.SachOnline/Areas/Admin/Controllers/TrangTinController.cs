using NguyenHongKhanh.SachOnline.Filters;
using NguyenHongKhanh.SachOnline.Helpers;
using NguyenHongKhanh.SachOnline.Models;
using PagedList;
using System;
using System.Linq;
using System.Web.Mvc;

namespace NguyenHongKhanh.SachOnline.Areas.Admin.Controllers
{
    [AdminAuthorize]
    public class TrangTinController : Controller
    {
        SachOnlineDataEntities data = new SachOnlineDataEntities();

        // GET: Admin/TrangTin
        public ActionResult Index(int? page)
        {
            int iPageNum = (page ?? 1);
            int iPageSize = 10;
            return View(data.TRANGTINs.ToList().OrderBy(n => n.MaTT).ToPagedList(iPageNum, iPageSize));
        }

        // GET: Admin/TrangTin/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/TrangTin/Create
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TRANGTIN trangTin, FormCollection f)
        {
            if (ModelState.IsValid)
            {
                // Lấy nội dung từ CKEditor
                trangTin.NoiDung = f["sNoiDung"];

                // Tự động tạo MetaTitle từ TenTrang
                trangTin.MetaTitle = trangTin.TenTrang.GenerateSlug();

                // Set ngày tạo
                trangTin.NgayTao = DateTime.Now;

                data.TRANGTINs.Add(trangTin);
                data.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(trangTin);
        }

        // GET: Admin/TrangTin/Edit/5
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var trangTin = data.TRANGTINs.SingleOrDefault(n => n.MaTT == id);
            if (trangTin == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.NoiDung = trangTin.NoiDung;
            return View(trangTin);
        }

        // POST: Admin/TrangTin/Edit/5
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TRANGTIN trangTin, FormCollection f)
        {
            if (ModelState.IsValid)
            {
                var existingTrangTin = data.TRANGTINs.Find(trangTin.MaTT);
                if (existingTrangTin != null)
                {
                    existingTrangTin.TenTrang = trangTin.TenTrang;
                    existingTrangTin.NoiDung = f["sNoiDung"];

                    // Cập nhật MetaTitle nếu tên trang thay đổi
                    existingTrangTin.MetaTitle = trangTin.TenTrang.GenerateSlug();

                    data.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            ViewBag.NoiDung = f["sNoiDung"];
            return View(trangTin);
        }

        // GET: Admin/TrangTin/Delete/5
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var trangTin = data.TRANGTINs.SingleOrDefault(n => n.MaTT == id);
            if (trangTin == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(trangTin);
        }

        // POST: Admin/TrangTin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirm(int id)
        {
            var trangTin = data.TRANGTINs.SingleOrDefault(n => n.MaTT == id);
            if (trangTin == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            data.TRANGTINs.Remove(trangTin);
            data.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
