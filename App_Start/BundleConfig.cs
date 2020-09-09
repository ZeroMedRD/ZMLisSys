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
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/jasny-bootstrap.js",
                      "~/Scripts/jquery.form.js",
                      "~/Scripts/respond.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/jasny-bootstrap.css",
                      "~/Content/site.css",
                       "~/Content/LabStyle.css",
                      "~/Content/kendo/2018.2.516/kendo.dataviz.min.css",
                      "~/Content/kendo/2018.2.516/kendo.dataviz.default.min.css",
                      "~/Content/kendo/2018.2.516/kendo.mobile.all.min.css"
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

            // 將 EnableOptimizations 設為 false 以進行偵錯。如需詳細資訊，
            // 請造訪 http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;
        }
    }
}
