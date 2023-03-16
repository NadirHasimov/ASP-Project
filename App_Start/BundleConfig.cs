using System.Web;
using System.Web.Optimization;

namespace Marmara
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {

            //============================ Image Upload ===============================//
            bundles.Add(new StyleBundle("~/imageUpload/css").Include(
                   "~/Public/imageUpload/css/image.css",
                   "~/Public/imageUpload/css/bootstrap-slider.css"
               ));

            bundles.Add(new ScriptBundle("~/imageUpload/js").Include(
                "~/Public/imageUpload/js/jquery.min.js",
                "~/Public/imageUpload/js/bootstrap.min.js",
                "~/Public/imageUpload/js/bootstrap-slider.js",
                "~/Public/imageUpload/js/ImageUpload.js"

                 ));

            //============================ ADMIN ===============================//

            bundles.Add(new StyleBundle("~/admin/css").Include(
                    "~/Public/admin/css/main.css",
                    "~/Public/site/css/signalR.css",
                    "~/Public/admin/css/new.css"
                ));

            bundles.Add(new ScriptBundle("~/admin/js").Include(
                "~/Public/admin/js/popper.min.js",
                "~/Public/admin/js/main.js",
                "~/Public/admin/js/pace.min.js",
                "~/Public/admin/js/plugins/jquery.dataTables.min.js",
                "~/Public/admin/js/plugins/dataTables.bootstrap.min.js"
                 ));

            //bundles.Add(new ScriptBundle("~/admin/js").Include(
            //          "~/Public/admin/js/jquery-3.3.1.min.js",
            //          "~/Public/admin/js/popper.min.js",
            //          "~/Public/admin/js/bootstrap.min.js",
            //          "~/Public/admin/js/main.js",
            //          "~/Public/admin/js/pace.min.js",
            //          "~/Public/admin/js/plugins/jquery.dataTables.min.js",
            //          "~/Public/admin/js/plugins/dataTables.bootstrap.min.js"
            //          ));



            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //          "~/Public/admin/js/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/admin").Include(
            //          "~/Public/admin/js/jquery.min.js",
            //          "~/Public/admin/js/bootstrap.min.js",
            //          "~/Public/admin/js/bootstrap-slider.js",
            //          "~/Public/admin/js/ImageUpload.js"
            //          ));

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Public/admin/css/bootstrap.css",
            //          "~/Public/admin/css/site.css",
            //          "~/Public/admin/css/bootstrap-slider.css"
            //          ));


            //============================ SITE ===============================//


            bundles.Add(new ScriptBundle("~/bundles/LayoutScript").Include(
                "~/Public/site/js/jquery-1.12.4.min.js",
                "~/Public/site/js/jquery-ui.js",
                "~/Public/site/js/popper.min.js",
                "~/Public/site/js/bootstrap.min.js",
                "~/Public/site/owlcarousel/js/owl.carousel.min.js",
                "~/Public/site/js/magnific-popup.min.js",
                "~/Public/site/js/waypoints.min.js",
                "~/Public/site/js/parallax.js",
                "~/Public/site/js/jquery.countdown.min.js",
                "~/Public/site/js/jquery.counterup.min.js",
                "~/Public/site/js/imagesloaded.pkgd.min.js",
                "~/Public/site/js/isotope.min.js",
                "~/Public/site/js/jquery.parallax-scroll.js",
                "~/Public/site/js/countDown.min.js",
                "~/Public/site/js/scripts.js",
                "~/Public/admin/js/plugins/bootstrap-datepicker.min.js"

                ));

            bundles.Add(new StyleBundle("~/Content/LayoutStyle").Include(

                "~/Public/site/css/animate.css",
                "~/Public/site/css/bootstrap.min.css",
                "~/Public/site/css/ionicons.min.css",
                "~/Public/site/css/themify-icons.css",
                "~/Public/site/css/all.min.css",
                "~/Public/site/owlcarousel/css/owl.carousel.min.css",
                "~/Public/site/owlcarousel/css/owl.theme.css",
                "~/Public/site/owlcarousel/css/owl.theme.default.min.css",
                "~/Public/site/css/magnific-popup.css",
                "~/Public/site/css/style.css",
                "~/Public/site/css/responsive.css",
                "~/Public/site/css/datepicker.css",
                "~/Public/site/css/countdown.css",
                "~/Public/site/css/certificate.css",
                "~/Public/site/css/signalR.css",
                "~/Public/site/css/color/theme.css"
                ));

            //============================ SITE ===============================//


            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.validate*"));

            //// Use the development version of Modernizr to develop with and learn from. Then, when you're
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js"));

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/bootstrap.css",
            //          "~/Content/site.css"));


        }
    }
}
