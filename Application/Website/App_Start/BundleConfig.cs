using System.Web;
using System.Web.Optimization;

namespace Website
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.unobtrusive-ajax.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include("~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                     "~/Scripts/bootstrap.js",
                     "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/admin-library").Include(
                        "~/Content/js/ace-extra.min.js",
                        "~/Content/js/jquery-ui.custom.min.js",
                        "~/Content/js/ace-elements.min.js",
                        "~/Content/js/ace.min.js",
                        "~/Scripts/sb-common.js"));

            bundles.Add(new ScriptBundle("~/bundles/site-library").Include(
                       "~/Scripts/jquery.placeholder.js",
                       "~/Scripts/sb-common.js",
                       "~/Scripts/moment.js",
                       "~/Scripts/jquery.daterangepicker.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include("~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/admin-css").Include(
                    "~/Content/css/bootstrap.min.css",
                    "~/Content/css/site.css",
                    "~/Content/font-awesome/4.2.0/css/font-awesome.min.css",
                    "~/Content/fonts/fonts.googleapis.com.css",
                    "~/Content/css/ace.min.css"));

            bundles.Add(new StyleBundle("~/Content/site-css").Include(
                "~/Content/site.css",
                "~/Content/bootstrap.css",
                "~/Content/daterangepicker.css"));
        }
    }
}
