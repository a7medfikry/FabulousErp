using System.Web;
using System.Web.Optimization;

namespace FabulousErp
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            BundleTable.EnableOptimizations = false;

            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"
            //            ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            if(HttpContext.Current.IsDebuggingEnabled)
            {
                bundles.Add(new StyleBundle("~/NContent/css").Include(
                "~/Content/css/font-awesome.css",
               //"~/Content/bootstrap.css",
               "~/Content/bootstrap4-dataTables.css",
               "~/Content/bootstrap4-responsive.css",
               "~/Content/fixedHeader.bootstrap4.min.css",
               "~/Content/site.css",
               //"~/_Content/Styles/main.css",
               //"~/_Content/Styles/print.css",
               "~/_Content/Styles/util.css",
               "~/_Content/Styles/validations.css",
               "~/_Content/Styles/datatables.min.css"
             ));
                bundles.Add(new StyleBundle("~/NLayoutCss").Include(
                   "~/Content/bootstrap4-dataTables.css",
                   "~/Content/bootstrap4-responsive.css",
                   "~/Content/site.css",
                   //"~/_Content/Styles/main.css",
                   //"~/_Content/Styles/print.css",
                   "~/_Content/Styles/util.css",
                   "~/_Content/Styles/validations.css",
                   "~/_Content/Styles/datatables.min.css"
                 ));
                bundles.Add(new ScriptBundle("~/NCommon").Include(
                   //"~/Scripts/jquery-3.3.1.min.js",
                   "~/_Content/BootstrapSelect/popper.min.js",
                   //"~/Scripts/bootstrap.js",
                   "~/_Content/CommonScript.js",
                   //"~/_Content/JS/jQuery.print.min.js",
                   "~/Areas/FixedAssets/Scripts/jquery.unobtrusive-ajax.min.js",
                   "~/Areas/FixedAssets/Scripts/jquery.validate.min.js",
                   "~/Areas/FixedAssets/Scripts/jquery.validate.unobtrusive.js",
                   "~/_Content/moment.js",
                   "~/_Content/BootstrapSelect/bootstrap-select.min.js",
                   "~/_Content/jquery.tabletoCSV.js",
                   "~/_Content/JS/datatables.min.js"

               ));
            }
            


            bundles.Add(new StyleBundle("~/Content/css").Include(
    "~/Content/css/font-awesome.css",
   "~/Content/bootstrap.css",
   "~/Content/bootstrap4-dataTables.css",
   "~/Content/bootstrap4-responsive.css",
   "~/Content/fixedHeader.bootstrap4.min.css",
   "~/Content/site.css",
   "~/_Content/Styles/main.css",
   "~/_Content/Styles/print.css",
   "~/_Content/Styles/util.css",
   "~/_Content/Styles/validations.css",
   "~/_Content/Styles/datatables.min.css"
 ));
            bundles.Add(new StyleBundle("~/LayoutCss").Include(
               "~/Content/bootstrap4-dataTables.css",
               "~/Content/bootstrap4-responsive.css",
               "~/Content/site.css",
               "~/_Content/Styles/main.css",
               "~/_Content/Styles/print.css",
               "~/_Content/Styles/util.css",
               "~/_Content/Styles/validations.css",
               "~/_Content/Styles/datatables.min.css"
             ));
            bundles.Add(new ScriptBundle("~/Common").Include(
               "~/Scripts/jquery-3.3.1.min.js",
               "~/_Content/BootstrapSelect/popper.min.js",
               "~/Scripts/bootstrap.js",
               "~/_Content/CommonScript.js",
               "~/_Content/JS/jQuery.print.min.js",
               "~/Areas/FixedAssets/Scripts/jquery.unobtrusive-ajax.min.js",
               "~/Areas/FixedAssets/Scripts/jquery.validate.min.js",
               "~/Areas/FixedAssets/Scripts/jquery.validate.unobtrusive.js",
               "~/_Content/moment.js",
               "~/_Content/BootstrapSelect/bootstrap-select.min.js",
               "~/_Content/jquery.tabletoCSV.js",
               "~/_Content/JS/datatables.min.js"

               ));
            bundles.Add(new ScriptBundle("~/VoidCommon").Include(
                "~/_Content/JS/Inquiry/Inquiry_JETransaction.js",
                "~/_Content/JS/ShowTransaction.js"));
           

            bundles.Add(new ScriptBundle("~/MainTransaction").Include(
            "~/_Content/JS/MainTransactions.js",
"~/_Content/JS/Settings/Transaction_Script.js"
              ));



            bundles.Add(new StyleBundle("~/FileUploadCss")
             .Include(
                 "~/_Content/Styles/FileUpload/blueimp-gallery.min.css",
                 "~/_Content/Styles/FileUpload/jquery.fileupload.css",
                 "~/_Content/Styles/FileUpload/jquery.fileupload-ui.css"
             ));

            bundles.Add(new ScriptBundle("~/FileUploadJs")
                          .Include(
                              "~/_Content/JS/FileUpload/jquery.ui.widget.js",
                              "~/_Content/JS/FileUpload/tmpl.min.js",
                              "~/_Content/JS/FileUpload/load-image.all.min.js",
                              "~/_Content/JS/FileUpload/canvas-to-blob.min.js",
                              "~/_Content/JS/FileUpload/jquery.blueimp-gallery.min.js",
                              "~/_Content/JS/FileUpload/jquery.iframe-transport.js",
                              "~/_Content/JS/FileUpload/jquery.fileupload.js",
                              "~/_Content/JS/FileUpload/jquery.fileupload-process.js",
                              "~/_Content/JS/FileUpload/jquery.fileupload-image.js",
                              "~/_Content/JS/FileUpload/jquery.fileupload-validate.js",
                              "~/_Content/JS/FileUpload/jquery.fileupload-ui.js"
                          ));

            bundles.Add(new ScriptBundle("~/Js")
           .Include(
                "~/Scripts/New/jquery.min.js",
                "~/Scripts/New/jquery-ui.min.js",
                "~/Scripts/New/popper.min.js",
                //"~/Scripts/New/bootstrap.min.js",
                "~/Scripts/New/jquery.slimscroll.js",
                "~/Scripts/New/modernizr.js",
                "~/Scripts/New/css-scrollbars.js",
                "~/Scripts/New/i18next.min.js",
                "~/Scripts/New/i18nextXHRBackend.min.js",
                "~/Scripts/New/i18nextBrowserLanguageDetector.min.js",
                "~/Scripts/New/jquery-i18next.min.js",
                "~/Scripts/New/common-pages.js"
           ));


            bundles.Add(new StyleBundle("~/Css")
          .IncludeDirectory(
              "~/Styles/", "*.css", false
          ));
            BundleTable.EnableOptimizations = false;


        }
    }
}

