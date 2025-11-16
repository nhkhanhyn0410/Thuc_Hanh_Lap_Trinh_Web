using NguyenHongKhanh.SachOnline.Filters;
using NguyenHongKhanh.SachOnline.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace NguyenHongKhanh.SachOnline.Areas.Admin.Controllers
{
    [AdminAuthorize]
    public class DonHangController : Controller
    {
        SachOnlineDataEntities data = new SachOnlineDataEntities();

        // GET: Admin/DonHang
        public ActionResult Index()
        {
            return View();
        }

        // =============== AJAX METHODS (Lab 08) ===============

        /// <summary>
        /// Lấy danh sách Đơn hàng với phân trang
        /// </summary>
        [HttpGet]
        public JsonResult GetListDonHang(int page = 1, int pageSize = 10)
        {
            try
            {
                // Lấy dữ liệu từ database trước (không format trong LINQ to Entities)
                var allDonHangRaw = (from dh in data.DONDATHANGs
                                     select new
                                     {
                                         MaDonHang = dh.MaDonHang,
                                         NgayDat = dh.NgayDat,
                                         NgayGiao = dh.NgayGiao,
                                         DaThanhToan = dh.DaThanhToan,
                                         TinhTrangGiaoHang = dh.TinhTrangGiaoHang,
                                         MaKH = dh.MaKH,
                                         TenKhachHang = dh.KHACHHANG != null ? dh.KHACHHANG.HoTen : "Khách vãng lai",
                                         DiaChiKH = dh.KHACHHANG != null ? dh.KHACHHANG.DiaChi : "",
                                         DienThoaiKH = dh.KHACHHANG != null ? dh.KHACHHANG.DienThoai : ""
                                     }).OrderByDescending(d => d.NgayDat).ToList();

                // Format ngày tháng sau khi đã lấy về memory
                var allDonHang = allDonHangRaw.Select(dh => new
                {
                    MaDonHang = dh.MaDonHang,
                    NgayDat = dh.NgayDat,
                    NgayGiao = dh.NgayGiao,
                    NgayDatDisplay = dh.NgayDat.HasValue ? dh.NgayDat.Value.ToString("dd/MM/yyyy HH:mm") : "",
                    NgayGiaoDisplay = dh.NgayGiao.HasValue ? dh.NgayGiao.Value.ToString("dd/MM/yyyy") : "",
                    DaThanhToan = dh.DaThanhToan,
                    TinhTrangGiaoHang = dh.TinhTrangGiaoHang,
                    MaKH = dh.MaKH,
                    TenKhachHang = dh.TenKhachHang,
                    DiaChiKH = dh.DiaChiKH,
                    DienThoaiKH = dh.DienThoaiKH
                }).ToList();

                var totalRecords = allDonHang.Count();
                var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
                var pagedData = allDonHang.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                return Json(new
                {
                    success = true,
                    data = pagedData,
                    currentPage = page,
                    totalPages = totalPages,
                    totalRecords = totalRecords,
                    message = "Lấy danh sách đơn hàng thành công"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Lấy danh sách đơn hàng thất bại: " + ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Lấy chi tiết đơn hàng (cho View Details)
        /// </summary>
        [HttpGet]
        public JsonResult Details(int id)
        {
            try
            {
                var dh = data.DONDATHANGs.SingleOrDefault(d => d.MaDonHang == id);

                if (dh == null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Không tìm thấy đơn hàng"
                    }, JsonRequestBehavior.AllowGet);
                }

                var chiTiet = (from ct in dh.CHITIETDATHANGs
                              select new
                              {
                                  MaSach = ct.MaSach,
                                  TenSach = ct.SACH.TenSach,
                                  SoLuong = ct.SoLuong,
                                  DonGia = ct.DonGia,
                                  ThanhTien = ct.SoLuong * ct.DonGia
                              }).ToList();

                // Format sau khi đã lấy dữ liệu
                var donHang = new
                {
                    MaDonHang = dh.MaDonHang,
                    NgayDat = dh.NgayDat.HasValue ? dh.NgayDat.Value.ToString("yyyy-MM-dd") : "",
                    NgayGiao = dh.NgayGiao,
                    NgayDatDisplay = dh.NgayDat.HasValue ? dh.NgayDat.Value.ToString("dd/MM/yyyy HH:mm") : "",
                    NgayGiaoDisplay = dh.NgayGiao.HasValue ? dh.NgayGiao.Value.ToString("dd/MM/yyyy") : "",
                    DaThanhToan = dh.DaThanhToan.HasValue ? dh.DaThanhToan.Value : false,
                    TinhTrangGiaoHang = dh.TinhTrangGiaoHang.HasValue ? dh.TinhTrangGiaoHang.Value : 0,
                    MaKH = dh.MaKH,
                    TenKhachHang = dh.KHACHHANG != null ? dh.KHACHHANG.HoTen : "Khách vãng lai",
                    DiaChiKH = dh.KHACHHANG != null ? dh.KHACHHANG.DiaChi : "",
                    DienThoaiKH = dh.KHACHHANG != null ? dh.KHACHHANG.DienThoai : "",
                    ChiTiet = chiTiet,
                    TongTien = chiTiet.Sum(ct => ct.ThanhTien)
                };

                return Json(new
                {
                    success = true,
                    data = donHang,
                    message = "Lấy thông tin đơn hàng thành công."
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Lấy thông tin đơn hàng thất bại: " + ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Cập nhật đơn hàng (cho View Update)
        /// </summary>
        [HttpPost]
        public JsonResult Update(int maDonHang, bool daThanhToan, int tinhTrangGiaoHang, string ngayGiao)
        {
            try
            {
                var dh = data.DONDATHANGs.SingleOrDefault(d => d.MaDonHang == maDonHang);

                if (dh == null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Không tìm thấy đơn hàng cần cập nhật"
                    });
                }

                dh.DaThanhToan = daThanhToan;
                dh.TinhTrangGiaoHang = tinhTrangGiaoHang;

                if (!string.IsNullOrWhiteSpace(ngayGiao))
                {
                    DateTime parsedDate;
                    if (DateTime.TryParse(ngayGiao, out parsedDate))
                    {
                        dh.NgayGiao = parsedDate;
                    }
                }

                data.SaveChanges();

                return Json(new
                {
                    success = true,
                    message = "Cập nhật đơn hàng thành công."
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Cập nhật đơn hàng thất bại. Lỗi: " + ex.Message
                });
            }
        }

        /// <summary>
        /// Xóa đơn hàng (cho View Delete)
        /// </summary>
        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                var dh = data.DONDATHANGs.SingleOrDefault(d => d.MaDonHang == id);

                if (dh == null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Không tìm thấy đơn hàng cần xóa"
                    });
                }

                // Xóa chi tiết đơn hàng trước
                var chiTiet = data.CHITIETDATHANGs.Where(ct => ct.MaDonHang == id);
                data.CHITIETDATHANGs.RemoveRange(chiTiet);

                // Xóa đơn hàng
                data.DONDATHANGs.Remove(dh);
                data.SaveChanges();

                return Json(new
                {
                    success = true,
                    message = "Xóa đơn hàng thành công."
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Xóa đơn hàng thất bại. Lỗi: " + ex.Message
                });
            }
        }

        /// <summary>
        /// Lấy danh sách Đơn hàng dạng JSON
        /// </summary>
        [HttpGet]
        public JsonResult DsDonHang()
        {
            try
            {
                var dsDH = (from dh in data.DONDATHANGs
                           select new
                           {
                               MaDonHang = dh.MaDonHang,
                               NgayDat = dh.NgayDat,
                               NgayGiao = dh.NgayGiao,
                               DaThanhToan = dh.DaThanhToan,
                               TinhTrangGiaoHang = dh.TinhTrangGiaoHang,
                               MaKH = dh.MaKH,
                               TenKH = dh.KHACHHANG != null ? dh.KHACHHANG.HoTen : "Khách vãng lai",
                               SoLuongSach = dh.CHITIETDATHANGs.Count(),
                               TongTien = dh.CHITIETDATHANGs.Sum(ct => ct.SoLuong * ct.DonGia) ?? 0
                           }).OrderByDescending(d => d.NgayDat).ToList();

                return Json(new
                {
                    code = 200,
                    dsDH = dsDH,
                    msg = "Lấy danh sách đơn hàng thành công"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    msg = "Lấy danh sách đơn hàng thất bại: " + ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Lấy chi tiết một Đơn hàng theo MaDonHang
        /// </summary>
        [HttpGet]
        public JsonResult Detail(int maDonHang)
        {
            try
            {
                var dh = data.DONDATHANGs.SingleOrDefault(d => d.MaDonHang == maDonHang);

                if (dh == null)
                {
                    return Json(new
                    {
                        code = 404,
                        msg = "Không tìm thấy đơn hàng"
                    }, JsonRequestBehavior.AllowGet);
                }

                var chiTiet = (from ct in dh.CHITIETDATHANGs
                              select new
                              {
                                  MaSach = ct.MaSach,
                                  TenSach = ct.SACH.TenSach,
                                  SoLuong = ct.SoLuong,
                                  DonGia = ct.DonGia,
                                  ThanhTien = ct.SoLuong * ct.DonGia
                              }).ToList();

                var donHang = new
                {
                    MaDonHang = dh.MaDonHang,
                    NgayDat = dh.NgayDat,
                    NgayGiao = dh.NgayGiao,
                    DaThanhToan = dh.DaThanhToan,
                    TinhTrangGiaoHang = dh.TinhTrangGiaoHang,
                    MaKH = dh.MaKH,
                    TenKH = dh.KHACHHANG != null ? dh.KHACHHANG.HoTen : "Khách vãng lai",
                    DiaChiKH = dh.KHACHHANG != null ? dh.KHACHHANG.DiaChi : "",
                    DienThoaiKH = dh.KHACHHANG != null ? dh.KHACHHANG.DienThoai : "",
                    ChiTiet = chiTiet,
                    TongTien = chiTiet.Sum(ct => ct.ThanhTien)
                };

                return Json(new
                {
                    code = 200,
                    dh = donHang,
                    msg = "Lấy thông tin đơn hàng thành công."
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    msg = "Lấy thông tin đơn hàng thất bại: " + ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Cập nhật trạng thái đơn hàng
        /// </summary>
        [HttpPost]
        public JsonResult UpdateTrangThai(int maDonHang, bool? daThanhToan, int? tinhTrangGiaoHang, string ngayGiao)
        {
            try
            {
                var dh = data.DONDATHANGs.SingleOrDefault(d => d.MaDonHang == maDonHang);

                if (dh == null)
                {
                    return Json(new
                    {
                        code = 404,
                        msg = "Không tìm thấy đơn hàng cần cập nhật"
                    }, JsonRequestBehavior.AllowGet);
                }

                if (daThanhToan.HasValue)
                    dh.DaThanhToan = daThanhToan.Value;

                if (tinhTrangGiaoHang.HasValue)
                    dh.TinhTrangGiaoHang = tinhTrangGiaoHang.Value;

                if (!string.IsNullOrWhiteSpace(ngayGiao))
                {
                    DateTime parsedDate;
                    if (DateTime.TryParse(ngayGiao, out parsedDate))
                    {
                        dh.NgayGiao = parsedDate;
                    }
                }

                data.SaveChanges();

                return Json(new
                {
                    code = 200,
                    msg = "Cập nhật trạng thái đơn hàng thành công."
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    msg = "Cập nhật đơn hàng thất bại. Lỗi: " + ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
