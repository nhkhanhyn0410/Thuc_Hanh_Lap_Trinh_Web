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
    }
}
