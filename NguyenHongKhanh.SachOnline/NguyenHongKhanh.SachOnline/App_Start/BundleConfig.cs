using System.Web.Optimization;

namespace NguyenHongKhanh.SachOnline
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Bundle cho jQuery
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // Bundle cho jQuery Validation
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Bundle cho Modernizr
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            // Bundle cho Bootstrap JS
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            // *** THÊM BUNDLE NÀY CHO ADMIN ***
            bundles.Add(new ScriptBundle("~/Scripts/js").Include(
                      "~/Scripts/jquery-{version}.js",
                      "~/Scripts/bootstrap.js"));

            // Bundle cho CSS
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            // *** THÊM BUNDLE CSS CHO ADMIN (nếu cần) ***
            bundles.Add(new StyleBundle("~/Content/Admin/css").Include(
                      "~/Content/structure.css",
                      "~/Content/reset.css"));
        }
    }
}