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
    public class ChuDeController : Controller
    {
        SachOnlineDataEntities db = new SachOnlineDataEntities();

        // GET: Admin/ChuDe
        public ActionResult Index()
        {
            return View();
        }

        // GET: Lấy danh sách Chủ đề dạng JSON
        [HttpGet]
        public JsonResult DsChuDe()
        {
            try
            {
                var dsCD = (from cd in db.CHUDEs
                            select new
                            {
                                MaCD = cd.MaCD,
                                TenCD = cd.TenChuDe
                            }).ToList();

                return Json(new { code = 200, dsCD = dsCD, msg = "Lấy danh sách chủ đề thành công" },
                           JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy danh sách chủ đề thất bại: " + ex.Message },
                           JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Chi tiết Chủ đề
        [HttpGet]
        public JsonResult Detail(int maCD)
        {
            try
            {
                var cd = (from s in db.CHUDEs
                          where s.MaCD == maCD
                          select new
                          {
                              MaCD = s.MaCD,
                              TenChuDe = s.TenChuDe
                          }).SingleOrDefault();

                return Json(new { code = 200, cd = cd, msg = "Lấy thông tin chủ đề thành công." },
                           JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy thông tin chủ đề thất bại: " + ex.Message },
                           JsonRequestBehavior.AllowGet);
            }
        }

        // POST: Thêm mới Chủ đề
        [HttpPost]
        public JsonResult AddChuDe(string strTenCD)
        {
            try
            {
                var cd = new CHUDE();
                cd.TenChuDe = strTenCD;
                db.CHUDEs.Add(cd);
                db.SaveChanges();

                return Json(new { code = 200, msg = "Thêm chủ đề thành công." },
                           JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Thêm chủ đề thất bại. Lỗi: " + ex.Message },
                           JsonRequestBehavior.AllowGet);
            }
        }

        // POST: Cập nhật Chủ đề
        [HttpPost]
        public JsonResult Update(int maCD, string strTenCD)
        {
            try
            {
                var cd = db.CHUDEs.SingleOrDefault(c => c.MaCD == maCD);
                cd.TenChuDe = strTenCD;
                db.SaveChanges();

                return Json(new { code = 200, msg = "Sửa chủ đề thành công." },
                           JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Sửa chủ đề thất bại. Lỗi: " + ex.Message },
                           JsonRequestBehavior.AllowGet);
            }
        }

        // POST: Xóa Chủ đề
        [HttpPost]
        public JsonResult Delete(int maCD)
        {
            try
            {
                var cd = db.CHUDEs.SingleOrDefault(c => c.MaCD == maCD);
                db.CHUDEs.Remove(cd);
                db.SaveChanges();

                return Json(new { code = 200, msg = "Xóa chủ đề thành công." },
                           JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Xóa chủ đề thất bại. Lỗi: " + ex.Message },
                           JsonRequestBehavior.AllowGet);
            }
        }
    }
}