using NguyenHongKhanh.SachOnline.Filters;
using NguyenHongKhanh.SachOnline.Models;
using PagedList;
using System;
using System.Linq;
using System.Web.Mvc;

namespace NguyenHongKhanh.SachOnline.Areas.Admin.Controllers
{
    [AdminAuthorize]
    public class KhachHangController : Controller
    {
        SachOnlineDataEntities data = new SachOnlineDataEntities();

        // GET: Admin/KhachHang
        public ActionResult Index(int? page)
        {
            // Sử dụng view AJAX mới
            return View("IndexAjax");
        }

        // GET: Admin/KhachHang (Old version with pagination)
        public ActionResult IndexOld(int? page)
        {
            int iPageNum = (page ?? 1);
            int iPageSize = 10;
            return View(data.KHACHHANGs.ToList().OrderBy(n => n.MaKH).ToPagedList(iPageNum, iPageSize));
        }

        // GET: Admin/KhachHang/Details/5
        public ActionResult Details(int id)
        {
            var kh = data.KHACHHANGs.SingleOrDefault(n => n.MaKH == id);
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(kh);
        }

        // GET: Admin/KhachHang/Edit/5
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var kh = data.KHACHHANGs.SingleOrDefault(n => n.MaKH == id);
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(kh);
        }

        // POST: Admin/KhachHang/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(KHACHHANG kh)
        {
            if (ModelState.IsValid)
            {
                var existingKh = data.KHACHHANGs.Find(kh.MaKH);
                if (existingKh != null)
                {
                    existingKh.HoTen = kh.HoTen;
                    existingKh.Email = kh.Email;
                    existingKh.DiaChi = kh.DiaChi;
                    existingKh.DienThoai = kh.DienThoai;
                    existingKh.NgaySinh = kh.NgaySinh;
                    // Không cho phép sửa TaiKhoan và MatKhau từ admin
                    data.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(kh);
        }

        // GET: Admin/KhachHang/Delete/5
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var kh = data.KHACHHANGs.SingleOrDefault(n => n.MaKH == id);
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(kh);
        }

        // POST: Admin/KhachHang/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirm(int id)
        {
            var kh = data.KHACHHANGs.SingleOrDefault(n => n.MaKH == id);
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            // Kiểm tra xem khách hàng có đơn hàng không
            var donHang = data.DONDATHANGs.Where(dh => dh.MaKH == id);
            if (donHang.Count() > 0)
            {
                ViewBag.ThongBao = "Không thể xóa khách hàng này vì đang có " + donHang.Count() + " đơn hàng.<br>" +
                    "Vui lòng xóa các đơn hàng trước khi xóa khách hàng.";
                return View(kh);
            }

            data.KHACHHANGs.Remove(kh);
            data.SaveChanges();
            return RedirectToAction("Index");
        }

        // =============== AJAX METHODS (Lab 08) ===============

        /// <summary>
        /// Lấy danh sách Khách hàng dạng JSON
        /// </summary>
        [HttpGet]
        public JsonResult DsKhachHang()
        {
            try
            {
                var dsKH = (from kh in data.KHACHHANGs
                           select new
                           {
                               MaKH = kh.MaKH,
                               HoTen = kh.HoTen,
                               TaiKhoan = kh.TaiKhoan,
                               Email = kh.Email,
                               DiaChi = kh.DiaChi,
                               DienThoai = kh.DienThoai,
                               NgaySinh = kh.NgaySinh
                           }).ToList();

                return Json(new
                {
                    code = 200,
                    dsKH = dsKH,
                    msg = "Lấy danh sách khách hàng thành công"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    msg = "Lấy danh sách khách hàng thất bại: " + ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Lấy chi tiết một Khách hàng theo MaKH
        /// </summary>
        [HttpGet]
        public JsonResult DetailAjax(int maKH)
        {
            try
            {
                var kh = (from k in data.KHACHHANGs
                         where k.MaKH == maKH
                         select new
                         {
                             MaKH = k.MaKH,
                             HoTen = k.HoTen,
                             TaiKhoan = k.TaiKhoan,
                             Email = k.Email,
                             DiaChi = k.DiaChi,
                             DienThoai = k.DienThoai,
                             NgaySinh = k.NgaySinh
                         }).SingleOrDefault();

                if (kh == null)
                {
                    return Json(new
                    {
                        code = 404,
                        msg = "Không tìm thấy khách hàng"
                    }, JsonRequestBehavior.AllowGet);
                }

                return Json(new
                {
                    code = 200,
                    kh = kh,
                    msg = "Lấy thông tin khách hàng thành công."
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    msg = "Lấy thông tin khách hàng thất bại: " + ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Cập nhật Khách hàng
        /// </summary>
        [HttpPost]
        public JsonResult UpdateAjax(int maKH, string hoTen, string email, string diaChi, string dienThoai, string ngaySinh)
        {
            try
            {
                // Validation
                if (string.IsNullOrWhiteSpace(hoTen))
                {
                    return Json(new
                    {
                        code = 400,
                        msg = "Họ tên không được để trống"
                    }, JsonRequestBehavior.AllowGet);
                }

                var kh = data.KHACHHANGs.SingleOrDefault(k => k.MaKH == maKH);

                if (kh == null)
                {
                    return Json(new
                    {
                        code = 404,
                        msg = "Không tìm thấy khách hàng cần sửa"
                    }, JsonRequestBehavior.AllowGet);
                }

                kh.HoTen = hoTen.Trim();
                kh.Email = string.IsNullOrWhiteSpace(email) ? "" : email.Trim();
                kh.DiaChi = string.IsNullOrWhiteSpace(diaChi) ? "" : diaChi.Trim();
                kh.DienThoai = string.IsNullOrWhiteSpace(dienThoai) ? "" : dienThoai.Trim();

                if (!string.IsNullOrWhiteSpace(ngaySinh))
                {
                    DateTime parsedDate;
                    if (DateTime.TryParse(ngaySinh, out parsedDate))
                    {
                        kh.NgaySinh = parsedDate;
                    }
                }

                data.SaveChanges();

                return Json(new
                {
                    code = 200,
                    msg = "Cập nhật khách hàng thành công."
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    msg = "Cập nhật khách hàng thất bại. Lỗi: " + ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Xóa Khách hàng
        /// </summary>
        [HttpPost]
        public JsonResult DeleteAjax(int maKH)
        {
            try
            {
                var kh = data.KHACHHANGs.SingleOrDefault(k => k.MaKH == maKH);

                if (kh == null)
                {
                    return Json(new
                    {
                        code = 404,
                        msg = "Không tìm thấy khách hàng cần xóa"
                    }, JsonRequestBehavior.AllowGet);
                }

                // Kiểm tra ràng buộc khóa ngoại
                if (kh.DONDATHANGs.Any())
                {
                    return Json(new
                    {
                        code = 400,
                        msg = "Không thể xóa khách hàng này vì đã có đơn hàng liên quan"
                    }, JsonRequestBehavior.AllowGet);
                }

                data.KHACHHANGs.Remove(kh);
                data.SaveChanges();

                return Json(new
                {
                    code = 200,
                    msg = "Xóa khách hàng thành công."
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    msg = "Xóa khách hàng thất bại. Lỗi: " + ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
