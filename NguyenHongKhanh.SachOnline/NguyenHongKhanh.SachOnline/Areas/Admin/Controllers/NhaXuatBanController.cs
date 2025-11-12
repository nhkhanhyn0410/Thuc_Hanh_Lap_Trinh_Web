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
    }
}
