using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using ZMLisSys.Models;
using BlogSample.Infrastructure.CustomResults;
using BlogSample.Infrastructure.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PagedList;
using System.Data.Entity;

namespace ZMLisSys.Controllers
{
    public class ListestsController : Controller
    {
        private ZMListestEntities db = new ZMListestEntities();

        //// GET: Listests
        //public ActionResult Index()
        //{
        //    return View(db.Listests.ToList());
        //}

        //檔案上傳程式開始
        public ActionResult Index(int page = 1)
        {
            int currentPage = page < 1 ? 1 : page;

            var query = db.Listests
                           .OrderBy(x => x.ID)
                           .ThenBy(x => x.TestName)
                           .ThenBy(x => x.Idno_id);

            var result = query.ToPagedList(currentPage, 10);

            return View(result);
        }

        private string fileSavedPath = WebConfigurationManager.AppSettings["UploadPath"];

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            JObject jo = new JObject();
            string result = string.Empty;

            if (file == null)
            {
                jo.Add("Result", false);
                jo.Add("Msg", "請上傳檔案!");
                result = JsonConvert.SerializeObject(jo);
                return Content(result, "application/json");
            }
            if (file.ContentLength <= 0)
            {
                jo.Add("Result", false);
                jo.Add("Msg", "請上傳正確的檔案.");
                result = JsonConvert.SerializeObject(jo);
                return Content(result, "application/json");
            }

            string fileExtName = Path.GetExtension(file.FileName).ToLower();

            if (!fileExtName.Equals(".xls", StringComparison.OrdinalIgnoreCase)
                &&
                !fileExtName.Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                jo.Add("Result", false);
                jo.Add("Msg", "請上傳 .xls 或 .xlsx 格式的檔案");
                result = JsonConvert.SerializeObject(jo);
                return Content(result, "application/json");
            }

            try
            {
                var uploadResult = this.FileUploadHandler(file);

                jo.Add("Result", !string.IsNullOrWhiteSpace(uploadResult));
                jo.Add("Msg", !string.IsNullOrWhiteSpace(uploadResult) ? uploadResult : "");

                result = JsonConvert.SerializeObject(jo);
            }
            catch (Exception ex)
            {
                jo.Add("Result", false);
                jo.Add("Msg", ex.Message);
                result = JsonConvert.SerializeObject(jo);
            }
            return Content(result, "application/json");
        }

        /// <summary>
        /// Files the upload handler.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">file;上傳失敗：沒有檔案！</exception>
        /// <exception cref="System.InvalidOperationException">上傳失敗：檔案沒有內容！</exception>
        private string FileUploadHandler(HttpPostedFileBase file)
        {
            string result;

            if (file == null)
            {
                throw new ArgumentNullException("file", "上傳失敗：沒有檔案！");
            }
            if (file.ContentLength <= 0)
            {
                throw new InvalidOperationException("上傳失敗：檔案沒有內容！");
            }

            try
            {
                string virtualBaseFilePath = Url.Content(fileSavedPath);
                string filePath = HttpContext.Server.MapPath(virtualBaseFilePath);

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                string newFileName = string.Concat(
                    DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                    Path.GetExtension(file.FileName).ToLower());

                string fullFilePath = Path.Combine(Server.MapPath(fileSavedPath), newFileName);
                file.SaveAs(fullFilePath);

                result = newFileName;
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        [HttpPost]
        public ActionResult Import(string savedFileName)
        {
            var jo = new JObject();
            string result;

            try
            {
                var fileName = string.Concat(Server.MapPath(fileSavedPath), "/", savedFileName);

                var importZipCodes = new List<Listest>();

                var helper = new ImportDataHelper();
                var checkResult = helper.CheckImportData(fileName, importZipCodes);

                jo.Add("Result", checkResult.Success);
                jo.Add("Msg", checkResult.Success ? string.Empty : checkResult.ErrorMessage);

                if (checkResult.Success)
                {
                    //儲存匯入的資料
                    helper.SaveImportData(importZipCodes);
                }
                result = JsonConvert.SerializeObject(jo);
            }
            catch (Exception ex)
            {
                throw;
            }
            return Content(result, "application/json");
        }


        [HttpPost]
        public ActionResult HasData()
        {
            JObject jo = new JObject();
            bool result = !db.Listests.Count().Equals(0);
            jo.Add("Msg", result.ToString());
            return Content(JsonConvert.SerializeObject(jo), "application/json");
        }

        public ActionResult Export()
        {
            var exportSpource = this.GetExportData();
            var dt = JsonConvert.DeserializeObject<DataTable>(exportSpource.ToString());

            var exportFileName = string.Concat(
                "TaiwanZipCode_",
                DateTime.Now.ToString("yyyyMMddHHmmss"),
                ".xlsx");

            return new ExportExcelResult
            {
                SheetName = "UCL_Report",
                FileName = exportFileName,
                ExportData = dt
            };
        }

        private JArray GetExportData()
        {
            var query = db.Listests
                           .OrderBy(x => x.ID)
                           .ThenBy(x => x.TestName)
                           .ThenBy(x => x.Idno_id);

            JArray jObjects = new JArray();

            foreach (var item in query)
            {
                var jo = new JObject();
                jo.Add("ID", item.ID);
                jo.Add("TestName", item.TestName);
                jo.Add("P_name", item.P_name);
                jo.Add("Sex_id", item.Sex_id);
                jo.Add("Idno_id", item.Idno_id);
                jObjects.Add(jo);
            }
            return jObjects;
        }
        //檔案上傳程式結束

        // GET: Listests/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Listest listest = db.Listests.Find(id);
            if (listest == null)
            {
                return HttpNotFound();
            }
            return View(listest);
        }

        // GET: Listests/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Listests/Create
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,TestName,P_name,Sex_id,Idno_id,CreateDate")] Listest listest)
        {
            if (ModelState.IsValid)
            {
                db.Listests.Add(listest);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(listest);
        }

        // GET: Listests/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Listest listest = db.Listests.Find(id);
            if (listest == null)
            {
                return HttpNotFound();
            }
            return View(listest);
        }

        // POST: Listests/Edit/5
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,TestName,P_name,Sex_id,Idno_id,CreateDate")] Listest listest)
        {
            if (ModelState.IsValid)
            {
                db.Entry(listest).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(listest);
        }

        // GET: Listests/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Listest listest = db.Listests.Find(id);
            if (listest == null)
            {
                return HttpNotFound();
            }
            return View(listest);
        }

        // POST: Listests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Listest listest = db.Listests.Find(id);
            db.Listests.Remove(listest);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
