using NguyenHongKhanh.SachOnline.Filters;
using NguyenHongKhanh.SachOnline.Models;
using PagedList;
using System;
using System.Linq;
using System.Web.Mvc;

namespace NguyenHongKhanh.SachOnline.Areas.Admin.Controllers
{
    [AdminAuthorize]
    public class ChuDeController : Controller
    {
        SachOnlineDataEntities data = new SachOnlineDataEntities();

        // GET: Admin/ChuDe
        public ActionResult Index(int? page)
        {
            int iPageNum = (page ?? 1);
            int iPageSize = 10;
            return View(data.CHUDEs.ToList().OrderBy(n => n.MaCD).ToPagedList(iPageNum, iPageSize));
        }

        // GET: Admin/ChuDe/Details/5
        public ActionResult Details(int id)
        {
            var chuDe = data.CHUDEs.SingleOrDefault(n => n.MaCD == id);
            if (chuDe == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(chuDe);
        }

        // GET: Admin/ChuDe/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/ChuDe/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CHUDE chuDe)
        {
            if (ModelState.IsValid)
            {
                data.CHUDEs.Add(chuDe);
                data.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(chuDe);
        }

        // GET: Admin/ChuDe/Edit/5
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var chuDe = data.CHUDEs.SingleOrDefault(n => n.MaCD == id);
            if (chuDe == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(chuDe);
        }

        // POST: Admin/ChuDe/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CHUDE chuDe)
        {
            if (ModelState.IsValid)
            {
                var existingChuDe = data.CHUDEs.Find(chuDe.MaCD);
                if (existingChuDe != null)
                {
                    existingChuDe.TenChuDe = chuDe.TenChuDe;
                    data.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(chuDe);
        }

        // GET: Admin/ChuDe/Delete/5
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var chuDe = data.CHUDEs.SingleOrDefault(n => n.MaCD == id);
            if (chuDe == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(chuDe);
        }

        // POST: Admin/ChuDe/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirm(int id)
        {
            var chuDe = data.CHUDEs.SingleOrDefault(n => n.MaCD == id);
            if (chuDe == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            // Kiểm tra xem có sách nào thuộc chủ đề này không
            var sachThuocChuDe = data.SACHes.Where(s => s.MaCD == id);
            if (sachThuocChuDe.Count() > 0)
            {
                ViewBag.ThongBao = "Không thể xóa chủ đề này vì đang có " + sachThuocChuDe.Count() + " sách thuộc chủ đề này.<br>" +
                    "Vui lòng xóa hoặc chuyển các sách sang chủ đề khác trước khi xóa.";
                return View(chuDe);
            }

            data.CHUDEs.Remove(chuDe);
            data.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
