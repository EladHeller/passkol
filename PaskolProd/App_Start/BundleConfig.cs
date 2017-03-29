using System.Web;
using System.Web.Optimization;

namespace PaskolProd
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/globalize.js",
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/PaskolManager").Include(
                        "~/Scripts/PaskolManager.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                      "~/Scripts/jquery-{version}.js",
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/select2.js",
                      "~/Scripts/jquery-ui.js",
                      "~/Scripts/polyfills.js",
                      "~/TemplatesContent/bootstrap/js/custom.js",
                      "~/TemplatesContent/bootstrap/js/jscrollpane/script/jquery.jscrollpane.js",
                       "~/TemplatesContent/bootstrap/js/jscrollpane/script/jquery.mousewheel.js",
                      "~/Scripts/bootstrap-datepicker.js",
                      "~/Scripts/Angular/angular.js",
                      "~/Scripts/Angular/angular-route.js",
                      "~/Scripts/Angular/angular-sanitize.js",
                      "~/Scripts/Angular/ui-bootstrap1.2.1.min.js",
                      "~/Scripts/Angular/Passkol/passkolApp.js",
                      "~/Scripts/Angular/Passkol/passkolDirectives.js",
                      "~/Scripts/Angular/Passkol/Layout/srvLayout.js",
                      "~/Scripts/Angular/Passkol/Layout/ctrlLayout.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap/css/bootstrap.css",
                      "~/Content/bootstrap/css/Mgrcustom.css",
                      "~/Content/CSS/font.css",
                      "~/Content/CSS/style.css",
                      "~/Content/CSS/select2.css",
                      "~/TemplatesContent/font-awesome/css/font-awesome.css",
                      "~/Content/CSS/jquery-ui.css",
                      "~/Content/PagedList.css"));

            bundles.Add(new StyleBundle("~/Content/Passkol/Css").Include(
                     "~/TemplatesContent/bootstrap/css/bootstrap.css",
                     "~/Content/CSS/font.css",
                     "~/TemplatesContent/bootstrap/css/custom.css",
                     "~/TemplatesContent/bootstrap/js/jscrollpane/style/jquery.jscrollpane.css",
                     "~/TemplatesContent/bootstrap/js/select2/css/select2.css",
                     "~/TemplatesContent/font-awesome/css/font-awesome.css",
                     "~/Content/CSS/jquery-ui.css"));

            bundles.Add(new ScriptBundle("~/bundles/Scripts/PasskolMain").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui.js",
                        "~/Scripts/bootstrap.js",
                        "~/TemplatesContent/bootstrap/js/custom.js",
                        "~/TemplatesContent/bootstrap/js/jscrollpane/script/jquery.jscrollpane.js",
                        "~/TemplatesContent/bootstrap/js/jscrollpane/script/jquery.mousewheel.js",
                        "~/TemplatesContent/bootstrap/js/select2/js/select2.js",
                        "~/Scripts/Angular/angular.js",
                        "~/Scripts/Angular/angular-route.js",
                        "~/Scripts/Angular/angular-sanitize.js",
                        "~/Scripts/Angular/ui-bootstrap1.2.1.min.js",
                        "~/Scripts/polyfills.js"
                        ).IncludeDirectory("~/Scripts/Angular/Passkol", "*.js",true));

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = false;
        }
    }
}
