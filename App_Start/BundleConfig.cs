using System.Web;
using System.Web.Optimization;

namespace ZMLisSys
{
    public class BundleConfig
    {
        // 如需統合的詳細資訊，請瀏覽 https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.validate*"));

            // 使用開發版本的 Modernizr 進行開發並學習。然後，當您
            // 準備好可進行生產時，請使用 https://modernizr.com 的建置工具，只挑選您需要的測試。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/contents/css").Include(
                //"~/Content/bootstrap.min.css",
                //"~/Content/fontawesome.min.css",
                // "~/Content/ionicons.min.css",
                //"~/admin-lte/css/skins/all.min.css",
                //"~/admin-lte/css/AdminLTE.min.css",                
                //"~/Adminlte3/plugins/fontawesome-free/css/fontawesome.min.css",
                "~/Adminlte3/plugins/fontawesome-free/css/all.min.css",
                //"~/Content/all.min.css",
                "~/AdminLte3/dist/css/adminlte.min.css",
                "~/Content/kendo/kendo.custom.css",                
                "~/Content/kendo/2018.2.516/kendo.common.min.css",
                "~/Content/kendo/2018.2.516/kendo.rtl.min.css",
                //"~/Content/kendo/2018.2.516/kendo.default.min.css",
                "~/Content/kendo/2018.2.516/kendo.dataviz.min.css",
                "~/Content/kendo/2018.2.516/kendo.dataviz.default.min.css",
                "~/Content/kendo/2018.2.516/kendo.mobile.all.min.css",
                //"~/Content/font-awesome.min.css",
                "~/Content/Site.css",
                "~/Content/Grid.css"
                ));

            bundles.Add(new StyleBundle("~/contents/main").Include(
                //"~/Adminlte3/plugins/fontawesome-free/css/fontawesome.min.css",
                //"~/Content/font-awesome.min.css",
                //"~/Content/font-awesome.min.css",
                "~/Adminlte3/plugins/fontawesome-free/css/all.min.css",
                //"~/Content/all.min.css",
                "~/Adminlte3/dist/css/adminlte.min.css",
                "~/Content/kendo/kendo.custom.css",                
                "~/Content/kendo/2018.2.516/kendo.common.min.css",
                "~/Content/kendo/2018.2.516/kendo.rtl.min.css",
                //"~/Content/kendo/2018.2.516/kendo.default.min.css",
                "~/Content/kendo/2018.2.516/kendo.dataviz.min.css",
                "~/Content/kendo/2018.2.516/kendo.dataviz.default.min.css",
                "~/Content/kendo/2018.2.516/kendo.mobile.all.min.css",
                "~/Content/Site.css",
                "~/Content/Public/ZMCMSv2.css"
                ));

            bundles.Add(new StyleBundle("~/contents/editorcss").Include(
                "~/Content/Editor.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/admin-lte").Include(
                "~/AdminLte3/dist/js/adminlte.min.js",
                "~/AdminLte3/dist/js/pages/dashboard.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/kendo").Include(
                "~/Scripts/kendo/2018.2.516/jszip.min.js",
                "~/Scripts/kendo/2018.2.516/kendo.all.min.js",
                "~/Scripts/kendo/2018.2.516/kendo.web.min.js",
                "~/Scripts/kendo/2018.2.516/kendo.aspnetmvc.min.js",
                "~/Scripts/kendo/2018.2.516/cultures/kendo.culture.zh-TW.min.js",
                "~/Scripts/kendo/2018.2.516/messages/kendo.messages.zh-TW.min.js",
                "~/Scripts/kendo/2018.2.516/pako_deflate.min.js"
                ));

            //bundles.Add(new StyleBundle("~/contents/kendo").Include(
            //    "~/Content/kendo/2018.2.516/kendo.common.min.css",
            //    "~/Content/kendo/2018.2.516/kendo.mobile.all.min.css",
            //    "~/Content/kendo/kendo.custom.css"
            //    ));

            bundles.Add(new ScriptBundle("~/bundles/main").Include(
                //"~/AdminLte3/plugins/bootstrap/js/bootstrap.bundle.min.js",
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/bootstrap.min.js"
                ));
        }
    }
}
