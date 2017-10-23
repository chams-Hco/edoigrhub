using System.Web;
using System.Web.Optimization;

namespace CICSWebPortal
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
          "~/Scripts/jquery-1.10.2.min.js",
          "~/Scripts/bootstrap.min.js",
          "~/Scripts/jquery.dataTables.min.js",
          "~/Scripts/jquery.flot.min.js",
          "~/Scripts/rapheal.min.js",
          "~/Scripts/morris.min.js",
          "~/Scripts/jquery.colorbox.min.js",
          "~/Scripts/jquery.sparkline.min.js",
          "~/Scripts/pace.min.js",
           "~/Scripts/jquery-ui.min.js",
          "~/Scripts/jquery.popupoverlay.min.js",
          "~/Scripts/jquery.slimscroll.min.js",
           "~/Scripts/jquery.cookie.min.js",
            "~/Scripts/dataTables.scroller.min.js",
          "~/Scripts/dataTables.tableTools.min.js",
          "~/Scripts/modernizr.min.js",
          "~/Scripts/endless.js"
          ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/font-awesome.min.css",
                      "~/Content/jquery.dataTables.min.css",
                      "~/Content/jquery-ui.min.css",
                      "~/Content/morris.css",
                      "~/Content/colorbox.css",
                      "~/Content/pace.css",
                      "~/Content/endless.css",
                      "~/Content/endless-skin.css",
                      "~/Content/dataTables.scroller.min",
                      "~/Content/dataTables.tableTools.min.css",
                      "~/Content/custom.css"));
        }
    }
}
