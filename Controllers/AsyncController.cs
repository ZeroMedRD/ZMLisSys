using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using ZMLisSys.Infrastructure.Helpers;
using ClosedXML.Excel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;


namespace ZMLisSys.Controllers
{
    public class AsyncController : Controller
    {
        // GET: Async
        public ActionResult Index()
        {

            DataTable dt = new DataTable();
            return View();
        }
        public ActionResult About()
        {
          
            return View();
        }
       
        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Index_Save(IEnumerable<HttpPostedFileBase> files)
        {
            // The Name of the Upload component is "files"
            if (files != null)
            {
                foreach (var file in files)
                {
                    // Some browsers send file names with full path.
                    // We are only interested in the file name.
                    var fileName = Path.GetFileName(file.FileName);
                    var physicalPath = Path.Combine(Server.MapPath("~/FileCloud"), fileName);

                    // The files are not actually saved in this demo
                    // file.SaveAs(physicalPath);
                }
            }

            // Return an empty string to signify success
            return Content("");
        }
        //kendo 上傳檔案
        public ActionResult Index_Remove(string[] fileNames)
        {
            // The parameter of the Remove action must be called "fileNames"

            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var fileName = Path.GetFileName(fullName);
                    var physicalPath = Path.Combine(Server.MapPath("~/FileCloud"), fileName);

                    // TODO: Verify user permissions

                    if (System.IO.File.Exists(physicalPath))
                    {
                        // The files are not actually removed in this demo
                        // System.IO.File.Delete(physicalPath);
                    }
                }
            }

            // Return an empty string to signify success
            return Content("");
        }

        public virtual ActionResult UploadFile()
        {
            //## 如果有任何檔案類型才做
            if (Request.Files.AllKeys.Any())
            {
                //## 讀取指定的上傳檔案ID
                var httpPostedFile = Request.Files["UploadedImage"];

                //## 真實有檔案，進行上傳
                if (httpPostedFile != null && httpPostedFile.ContentLength != 0)
                {
                    httpPostedFile.SaveAs("~/FileUploads");
                }
            }

            //## 模擬上傳的檔案內容
            List<JsonHelper> oStr = new List<JsonHelper>();
            //oStr.Add(new JsonHelper() { ID = 1, Name = "王1" });
            //oStr.Add(new JsonHelper() { ID = 2, Name = "王2" });

            //## 將結果回傳
            return Json(new { isUploaded = true, result = oStr }, "text/html");
        }

    }
}