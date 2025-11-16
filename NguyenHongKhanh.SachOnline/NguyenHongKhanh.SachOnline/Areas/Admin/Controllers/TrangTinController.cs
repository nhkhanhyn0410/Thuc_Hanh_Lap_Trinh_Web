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
            // Sử dụng view AJAX mới
            return View("IndexAjax");
        }

        // GET: Admin/TrangTin (Old version with pagination)
        public ActionResult IndexOld(int? page)
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

        // =============== AJAX METHODS (Lab 08) ===============

        /// <summary>
        /// Lấy danh sách Trang tin dạng JSON
        /// </summary>
        [HttpGet]
        public JsonResult DsTrangTin()
        {
            try
            {
                var dsTT = (from tt in data.TRANGTINs
                           select new
                           {
                               MaTT = tt.MaTT,
                               TenTrang = tt.TenTrang,
                               NoiDung = tt.NoiDung,
                               NgayTao = tt.NgayTao,
                               MetaTitle = tt.MetaTitle
                           }).ToList();

                return Json(new
                {
                    code = 200,
                    dsTT = dsTT,
                    msg = "Lấy danh sách trang tin thành công"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    msg = "Lấy danh sách trang tin thất bại: " + ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Lấy chi tiết một Trang tin theo MaTT
        /// </summary>
        [HttpGet]
        public JsonResult DetailAjax(int maTT)
        {
            try
            {
                var tt = (from t in data.TRANGTINs
                         where t.MaTT == maTT
                         select new
                         {
                             MaTT = t.MaTT,
                             TenTrang = t.TenTrang,
                             NoiDung = t.NoiDung,
                             NgayTao = t.NgayTao,
                             MetaTitle = t.MetaTitle
                         }).SingleOrDefault();

                if (tt == null)
                {
                    return Json(new
                    {
                        code = 404,
                        msg = "Không tìm thấy trang tin"
                    }, JsonRequestBehavior.AllowGet);
                }

                return Json(new
                {
                    code = 200,
                    tt = tt,
                    msg = "Lấy thông tin trang tin thành công."
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    msg = "Lấy thông tin trang tin thất bại: " + ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Thêm mới Trang tin
        /// </summary>
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult AddTrangTin(string tenTrang, string noiDung)
        {
            try
            {
                // Validation
                if (string.IsNullOrWhiteSpace(tenTrang))
                {
                    return Json(new
                    {
                        code = 400,
                        msg = "Tên trang không được để trống"
                    }, JsonRequestBehavior.AllowGet);
                }

                var tt = new TRANGTIN();
                tt.TenTrang = tenTrang.Trim();
                tt.NoiDung = noiDung;
                tt.MetaTitle = tenTrang.GenerateSlug();
                tt.NgayTao = DateTime.Now;

                data.TRANGTINs.Add(tt);
                data.SaveChanges();

                return Json(new
                {
                    code = 200,
                    msg = "Thêm trang tin thành công."
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    msg = "Thêm trang tin thất bại. Lỗi: " + ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Cập nhật Trang tin
        /// </summary>
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult UpdateAjax(int maTT, string tenTrang, string noiDung)
        {
            try
            {
                // Validation
                if (string.IsNullOrWhiteSpace(tenTrang))
                {
                    return Json(new
                    {
                        code = 400,
                        msg = "Tên trang không được để trống"
                    }, JsonRequestBehavior.AllowGet);
                }

                var tt = data.TRANGTINs.SingleOrDefault(t => t.MaTT == maTT);

                if (tt == null)
                {
                    return Json(new
                    {
                        code = 404,
                        msg = "Không tìm thấy trang tin cần sửa"
                    }, JsonRequestBehavior.AllowGet);
                }

                tt.TenTrang = tenTrang.Trim();
                tt.NoiDung = noiDung;
                tt.MetaTitle = tenTrang.GenerateSlug();
                data.SaveChanges();

                return Json(new
                {
                    code = 200,
                    msg = "Sửa trang tin thành công."
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    msg = "Sửa trang tin thất bại. Lỗi: " + ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Xóa Trang tin
        /// </summary>
        [HttpPost]
        public JsonResult DeleteAjax(int maTT)
        {
            try
            {
                var tt = data.TRANGTINs.SingleOrDefault(t => t.MaTT == maTT);

                if (tt == null)
                {
                    return Json(new
                    {
                        code = 404,
                        msg = "Không tìm thấy trang tin cần xóa"
                    }, JsonRequestBehavior.AllowGet);
                }

                data.TRANGTINs.Remove(tt);
                data.SaveChanges();

                return Json(new
                {
                    code = 200,
                    msg = "Xóa trang tin thành công."
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    msg = "Xóa trang tin thất bại. Lỗi: " + ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
