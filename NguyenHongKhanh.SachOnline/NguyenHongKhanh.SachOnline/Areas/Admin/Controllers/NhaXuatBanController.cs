using NguyenHongKhanh.SachOnline.Filters;
using NguyenHongKhanh.SachOnline.Models;
using PagedList;
using System;
using System.Linq;
using System.Web.Mvc;

namespace NguyenHongKhanh.SachOnline.Areas.Admin.Controllers
{
    [AdminAuthorize]
    public class NhaXuatBanController : Controller
    {
        SachOnlineDataEntities data = new SachOnlineDataEntities();

        // GET: Admin/NhaXuatBan
        public ActionResult Index(int? page)
        {
            // Sử dụng view AJAX mới
            return View("IndexAjax");
        }

        // GET: Admin/NhaXuatBan (Old version with pagination)
        public ActionResult IndexOld(int? page)
        {
            int iPageNum = (page ?? 1);
            int iPageSize = 10;
            return View(data.NHAXUATBANs.ToList().OrderBy(n => n.MaNXB).ToPagedList(iPageNum, iPageSize));
        }

        // GET: Admin/NhaXuatBan/Details/5
        public ActionResult Details(int id)
        {
            var nxb = data.NHAXUATBANs.SingleOrDefault(n => n.MaNXB == id);
            if (nxb == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(nxb);
        }

        // GET: Admin/NhaXuatBan/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/NhaXuatBan/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NHAXUATBAN nxb)
        {
            if (ModelState.IsValid)
            {
                data.NHAXUATBANs.Add(nxb);
                data.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nxb);
        }

        // GET: Admin/NhaXuatBan/Edit/5
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var nxb = data.NHAXUATBANs.SingleOrDefault(n => n.MaNXB == id);
            if (nxb == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(nxb);
        }

        // POST: Admin/NhaXuatBan/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(NHAXUATBAN nxb)
        {
            if (ModelState.IsValid)
            {
                var existingNxb = data.NHAXUATBANs.Find(nxb.MaNXB);
                if (existingNxb != null)
                {
                    existingNxb.TenNXB = nxb.TenNXB;
                    existingNxb.DiaChi = nxb.DiaChi;
                    existingNxb.DienThoai = nxb.DienThoai;
                    data.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(nxb);
        }

        // GET: Admin/NhaXuatBan/Delete/5
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var nxb = data.NHAXUATBANs.SingleOrDefault(n => n.MaNXB == id);
            if (nxb == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(nxb);
        }

        // POST: Admin/NhaXuatBan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirm(int id)
        {
            var nxb = data.NHAXUATBANs.SingleOrDefault(n => n.MaNXB == id);
            if (nxb == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            // Kiểm tra xem có sách nào thuộc nhà xuất bản này không
            var sachThuocNXB = data.SACHes.Where(s => s.MaNXB == id);
            if (sachThuocNXB.Count() > 0)
            {
                ViewBag.ThongBao = "Không thể xóa nhà xuất bản này vì đang có " + sachThuocNXB.Count() + " sách thuộc nhà xuất bản này.<br>" +
                    "Vui lòng xóa hoặc chuyển các sách sang nhà xuất bản khác trước khi xóa.";
                return View(nxb);
            }

            data.NHAXUATBANs.Remove(nxb);
            data.SaveChanges();
            return RedirectToAction("Index");
        }

        // =============== AJAX METHODS (Lab 08) ===============

        /// <summary>
        /// Lấy danh sách Nhà xuất bản dạng JSON
        /// </summary>
        [HttpGet]
        public JsonResult DsNhaXuatBan()
        {
            try
            {
                var dsNXB = (from nxb in data.NHAXUATBANs
                            select new
                            {
                                MaNXB = nxb.MaNXB,
                                TenNXB = nxb.TenNXB,
                                DiaChi = nxb.DiaChi,
                                DienThoai = nxb.DienThoai
                            }).ToList();

                return Json(new
                {
                    code = 200,
                    dsNXB = dsNXB,
                    msg = "Lấy danh sách nhà xuất bản thành công"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    msg = "Lấy danh sách nhà xuất bản thất bại: " + ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Lấy chi tiết một Nhà xuất bản theo MaNXB
        /// </summary>
        [HttpGet]
        public JsonResult DetailAjax(int maNXB)
        {
            try
            {
                var nxb = (from n in data.NHAXUATBANs
                          where n.MaNXB == maNXB
                          select new
                          {
                              MaNXB = n.MaNXB,
                              TenNXB = n.TenNXB,
                              DiaChi = n.DiaChi,
                              DienThoai = n.DienThoai
                          }).SingleOrDefault();

                if (nxb == null)
                {
                    return Json(new
                    {
                        code = 404,
                        msg = "Không tìm thấy nhà xuất bản"
                    }, JsonRequestBehavior.AllowGet);
                }

                return Json(new
                {
                    code = 200,
                    nxb = nxb,
                    msg = "Lấy thông tin nhà xuất bản thành công."
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    msg = "Lấy thông tin nhà xuất bản thất bại: " + ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Thêm mới Nhà xuất bản
        /// </summary>
        [HttpPost]
        public JsonResult AddNhaXuatBan(string tenNXB, string diaChi, string dienThoai)
        {
            try
            {
                // Validation
                if (string.IsNullOrWhiteSpace(tenNXB))
                {
                    return Json(new
                    {
                        code = 400,
                        msg = "Tên nhà xuất bản không được để trống"
                    }, JsonRequestBehavior.AllowGet);
                }

                var nxb = new NHAXUATBAN();
                nxb.TenNXB = tenNXB.Trim();
                nxb.DiaChi = string.IsNullOrWhiteSpace(diaChi) ? "" : diaChi.Trim();
                nxb.DienThoai = string.IsNullOrWhiteSpace(dienThoai) ? "" : dienThoai.Trim();

                data.NHAXUATBANs.Add(nxb);
                data.SaveChanges();

                return Json(new
                {
                    code = 200,
                    msg = "Thêm nhà xuất bản thành công."
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    msg = "Thêm nhà xuất bản thất bại. Lỗi: " + ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Cập nhật Nhà xuất bản
        /// </summary>
        [HttpPost]
        public JsonResult UpdateAjax(int maNXB, string tenNXB, string diaChi, string dienThoai)
        {
            try
            {
                // Validation
                if (string.IsNullOrWhiteSpace(tenNXB))
                {
                    return Json(new
                    {
                        code = 400,
                        msg = "Tên nhà xuất bản không được để trống"
                    }, JsonRequestBehavior.AllowGet);
                }

                var nxb = data.NHAXUATBANs.SingleOrDefault(n => n.MaNXB == maNXB);

                if (nxb == null)
                {
                    return Json(new
                    {
                        code = 404,
                        msg = "Không tìm thấy nhà xuất bản cần sửa"
                    }, JsonRequestBehavior.AllowGet);
                }

                nxb.TenNXB = tenNXB.Trim();
                nxb.DiaChi = string.IsNullOrWhiteSpace(diaChi) ? "" : diaChi.Trim();
                nxb.DienThoai = string.IsNullOrWhiteSpace(dienThoai) ? "" : dienThoai.Trim();
                data.SaveChanges();

                return Json(new
                {
                    code = 200,
                    msg = "Sửa nhà xuất bản thành công."
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    msg = "Sửa nhà xuất bản thất bại. Lỗi: " + ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Xóa Nhà xuất bản
        /// </summary>
        [HttpPost]
        public JsonResult DeleteAjax(int maNXB)
        {
            try
            {
                var nxb = data.NHAXUATBANs.SingleOrDefault(n => n.MaNXB == maNXB);

                if (nxb == null)
                {
                    return Json(new
                    {
                        code = 404,
                        msg = "Không tìm thấy nhà xuất bản cần xóa"
                    }, JsonRequestBehavior.AllowGet);
                }

                // Kiểm tra ràng buộc khóa ngoại
                if (nxb.SACHes.Any())
                {
                    return Json(new
                    {
                        code = 400,
                        msg = "Không thể xóa nhà xuất bản này vì đã có sách liên quan"
                    }, JsonRequestBehavior.AllowGet);
                }

                data.NHAXUATBANs.Remove(nxb);
                data.SaveChanges();

                return Json(new
                {
                    code = 200,
                    msg = "Xóa nhà xuất bản thành công."
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    msg = "Xóa nhà xuất bản thất bại. Lỗi: " + ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
