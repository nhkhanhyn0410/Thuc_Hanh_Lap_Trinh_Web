using NguyenHongKhanh.SachOnline.Filters;
using NguyenHongKhanh.SachOnline.Models;
using PagedList;
using PagedList.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NguyenHongKhanh.SachOnline.Areas.Admin.Controllers
{
    [AdminAuthorize]
    public class SachController : Controller
    {
        SachOnlineDataEntities data = new SachOnlineDataEntities();
        // GET: Admin/Sach
        public ActionResult Index(int? page)
        {
            int iPageNum = (page ?? 1);
            int iPageSize = 7;
            return View(data.SACHes.ToList().OrderBy(n => n.MaSach).ToPagedList(iPageNum, iPageSize));
        }

        // GET: Admin/Sach/Details/5 - Xem chi tiết sách
        public ActionResult Details(int id)
        {
            var sach = data.SACHes.SingleOrDefault(n => n.MaSach == id);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }

        // GET: Admin/Sach/Create - Hiển thị form thêm sách
        [HttpGet]
        public ActionResult Create()
        {
            // Đưa dữ liệu vào DropDownList
            ViewBag.MaCD = new SelectList(data.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(data.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            return View();
        }

        // POST: Admin/Sach/Create - Xử lý thêm sách
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(SACH sach, FormCollection f, HttpPostedFileBase fFileUpload)
        {
            // Đưa dữ liệu vào DropDownList
            ViewBag.MaCD = new SelectList(data.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(data.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");

            if (fFileUpload == null)
            {
                // Nội dung thông báo yêu cầu chọn ảnh bìa
                ViewBag.ThongBao = "Hãy chọn ảnh bìa.";
                // Lưu thông tin này lên trang do yêu cầu chọn ảnh bìa sẽ hiện thị
                // các thông tin này lên trang
                ViewBag.TenSach = f["sTenSach"];
                ViewBag.MoTa = f["sMoTa"];
                ViewBag.SoLuong = int.Parse(f["iSoLuong"]);
                ViewBag.GiaBan = decimal.Parse(f["mGiaBan"]);
                ViewBag.MaCD = new SelectList(data.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe", int.Parse(f["MaCD"]));
                ViewBag.MaNXB = new SelectList(data.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB", int.Parse(f["MaNXB"]));
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    // Lấy tên file (Khai báo thư viện: System.IO)
                    var sFileName = Path.GetFileName(fFileUpload.FileName);
                    // Lấy đường dẫn lưu file
                    var path = Path.Combine(Server.MapPath("~/Images"), sFileName);
                    // Kiểm tra ảnh bìa đã tồn tại chưa để lưu lên thư mục
                    if (!System.IO.File.Exists(path))
                    {
                        fFileUpload.SaveAs(path);
                    }
                    // Lưu Sach vào CSDL
                    sach.TenSach = f["sTenSach"];
                    sach.MoTa = f["sMoTa"];
                    sach.AnhBia = sFileName;
                    sach.NgayCapNhat = Convert.ToDateTime(f["dNgayCapNhat"]);
                    sach.SoLuongBan = int.Parse(f["iSoLuong"]);
                    sach.GiaBan = decimal.Parse(f["mGiaBan"]);
                    sach.MaCD = int.Parse(f["MaCD"]);
                    sach.MaNXB = int.Parse(f["MaNXB"]);
                    data.SACHes.Add(sach);
                    data.SaveChanges();

                    return RedirectToAction("Index");
                }
                return View();
            }
        }

        // GET: Admin/Sach/Edit/5 - Hiển thị form sửa sách
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var sach = data.SACHes.SingleOrDefault(n => n.MaSach == id);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            // Hiển thị danh sách chủ đề và nhà xuất bản đồng thời chọn chủ đề và nhà xuất bản
            // của cuốn hiện tại
            ViewBag.MaCD = new SelectList(data.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe", sach.MaCD);
            ViewBag.MaNXB = new SelectList(data.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB", sach.MaNXB);

            return View(sach);
        }

        // POST: Admin/Sach/Edit/5 - Xử lý cập nhật sách
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection f, HttpPostedFileBase fFileUpload)
        {
            int maSach = int.Parse(f["iMaSach"]);
            var sach = data.SACHes.SingleOrDefault(n => n.MaSach == maSach);
            ViewBag.MaCD = new SelectList(data.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe", sach.MaCD);
            ViewBag.MaNXB = new SelectList(data.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB", sach.MaNXB);

            if (ModelState.IsValid)
            {
                if (fFileUpload != null) // Kiểm tra để xác nhận cho thay đổi ảnh bìa
                {
                    // Lấy tên file (Khai báo thư viện: System.IO)
                    var sFileName = Path.GetFileName(fFileUpload.FileName);
                    // Lấy đường dẫn lưu file
                    var path = Path.Combine(Server.MapPath("~/Images"), sFileName);
                    // Kiểm tra file đã tồn tại chưa để lưu lên thư mục
                    if (!System.IO.File.Exists(path))
                    {
                        fFileUpload.SaveAs(path);
                    }
                    sach.AnhBia = sFileName;
                }
                // Lưu Sach vào CSDL
                sach.TenSach = f["sTenSach"];
                sach.MoTa = f["sMoTa"];
                sach.NgayCapNhat = Convert.ToDateTime(f["dNgayCapNhat"]);
                sach.SoLuongBan = int.Parse(f["iSoLuong"]);
                sach.GiaBan = decimal.Parse(f["mGiaBan"]);
                sach.MaCD = int.Parse(f["MaCD"]);
                sach.MaNXB = int.Parse(f["MaNXB"]);
                data.SaveChanges();
                // Về lại trang Quản lý sách
                return RedirectToAction("Index");
            }
            return View(sach);
        }

        // GET: Admin/Sach/Delete/5 - Hiển thị xác nhận xóa
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var sach = data.SACHes.SingleOrDefault(n => n.MaSach == id);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }

        // POST: Admin/Sach/DeleteConfirm/5 - Xử lý xóa sách
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id, FormCollection f)
        {
            var sach = data.SACHes.SingleOrDefault(n => n.MaSach == id);

            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            var ctdh = data.CHITIETDATHANGs.Where(ct => ct.MaSach == id);
            if (ctdh.Count() > 0)
            {
                // Nội dung sẽ hiển thị khi sách này đang có trong bảng chi tiết đặt hàng
                ViewBag.ThongBao = "Sách này đang có trong bảng chi tiết đặt hàng <br>" +
                    " Nếu muốn xóa thì phải xóa hết mã sách này trong bảng Chi tiết đặt hàng";

                return View(sach);
            }
            // Xóa hết thông tin của cuốn sách trong table VietSach trước khi xóa sách này
            var vietsach = data.VIETSACHes.Where(vs => vs.MaSach == id).ToList();
            if (vietsach != null)
            {
                data.VIETSACHes.RemoveRange(vietsach);
                data.SaveChanges();
            }
            // Xóa sách
            data.SACHes.Remove(sach);
            data.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}