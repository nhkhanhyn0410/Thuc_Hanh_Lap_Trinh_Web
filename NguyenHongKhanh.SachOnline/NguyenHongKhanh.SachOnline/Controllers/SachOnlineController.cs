using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NguyenHongKhanh.SachOnline.Controllers
{
    public class SachOnlineController : Controller
    {
        // GET: SachOnline
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ChuDePartial()
        {
            return PartialView();
        }

        public ActionResult SachBanNhieuPartial()
        {
            return PartialView();
        }

        public ActionResult NhaXuatBanPartial()
        {
            return PartialView();
        }

        public ActionResult NavbarHeaderPartial()
        {
            return PartialView();
        }

        public ActionResult AdsPartial()
        {
            return PartialView();
        }
    }
}
