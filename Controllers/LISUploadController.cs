using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.Configuration;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ZMLISSys.Models;
using ZMLISSys.ViewModels;
using ZMLib;

namespace ZMLISSys.Controllers
{
    public class LISUploadController : Controller
    {
        private LIS_AntifatEntities db_antifat = new LIS_AntifatEntities();
        private ZMLISEntities db_zmlis = new ZMLISEntities();
        private LIS_ZMCMSv2Entities db_zmcms = new LIS_ZMCMSv2Entities();

        ZMClass myClass = new ZMClass();
        public string dbs, ic, userid, password;
        //public string hislink = "res://*/Models.LIS_HISModel.csdl|res://*/Models.LIS_HISModel.ssdl|res://*/Models.LIS_HISModel.msl";

        #region 取得資料庫的連結
        private void GetLink()
        {
            // 依據使用者所屬的醫事機構代碼切換所屬資料庫            
            // dbs = [SQL Server IP,Port number]
            // InitialCatalog = Database Name
            // UserID = Login SQL Server User id
            // Password = Login SQL Server Password
            // 1. 從 Web.config 取得上面四個參數並且以 Json 方式傳遞
            string connectionParameters = myClass.GetParamFromWebConfig();

            // 2. 將取得的 Json 資料反序列化, 做反序列化需先定義 EFLink 類,它放在 DAO_SQLink.cs 裡
            EFLink eflink = JsonConvert.DeserializeObject<EFLink>(connectionParameters);

            // 3. 將反序列化後的資料解密並依屬性的值放在變數裡
            //dbs = eflink.Server;
            dbs = eflink.Server2;
            ic = eflink.InitialCatalog;
            userid = eflink.UserID;
            password = eflink.Password;
        }
        #endregion

        public ActionResult LUIndex()
        {
            if (Session["HospID"] != null)
            {
                ViewBag.HospID = Session["HospID"].ToString();
            }
            else
            {
                ViewBag.HospID = String.Empty;
            }

            return View();
        }

        public ActionResult lisPatientLaboratoryMaster_Read([DataSourceRequest] DataSourceRequest request, string sHospID, string startDate, string endDate)
        {
            DataSourceResult result = ("").ToDataSourceResult(request);

            DateTime sdt = DateTime.Parse(startDate);
            DateTime edt = DateTime.Parse(endDate);

            if (!String.IsNullOrEmpty(sHospID))
            {
                GetLink();

                // 判斷資料庫是否存在
                if (myClass.DatabaseOrTableExist(dbs, ic, userid, password, "his" + sHospID, null, 0) == true)
                {
                    LIS_HISEntities db_zmcmsdata = new LIS_HISEntities(myClass.GetSQLConnectionString(dbs, "his" + sHospID, userid, password, @"res://*/Models.LIS_HISModel.csdl|res://*/Models.LIS_HISModel.ssdl|res://*/Models.LIS_HISModel.msl"));

                    result = (from lplm in db_zmcmsdata.lisPatientLaboratoryMaster 
                              where lplm.PLMApplyDate>=sdt && lplm.PLMApplyDate<=edt                              
                              select lplm
                              ).ToDataSourceResult(request);
                }
            }

            return Json(result);
        }

        public ActionResult lisPatientLaboratoryDetail_Read([DataSourceRequest] DataSourceRequest request, string sHospID, string sPLMRowid)
        {
            DataSourceResult result = ("").ToDataSourceResult(request);

            if (!String.IsNullOrEmpty(sHospID))
            {
                GetLink();

                // 判斷資料庫是否存在
                if (myClass.DatabaseOrTableExist(dbs, ic, userid, password, "his" + sHospID, null, 0) == true)
                {
                    LIS_HISEntities db_zmcmsdata = new LIS_HISEntities(myClass.GetSQLConnectionString(dbs, "his" + sHospID, userid, password, @"res://*/Models.LIS_HISModel.csdl|res://*/Models.LIS_HISModel.ssdl|res://*/Models.LIS_HISModel.msl"));

                    result = (from lpld in db_zmcmsdata.lisPatientLaboratoryDetail 
                              where lpld.PLMRowid == sPLMRowid                              
                              select lpld).ToDataSourceResult(request);
                }
            }

            return Json(result);
        }

        public ActionResult Async_Save(IEnumerable<HttpPostedFileBase> annex, string HospID)
        {
            foreach (var file in annex)
            {
                if (file != null && file.ContentLength > 0)
                {
                    if (!String.IsNullOrEmpty(HospID))
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        string sDir = Server.MapPath("~/FileCloud") + @"/UploadFile/LISTemp/" + HospID;
                        //string sDir = @"\FileCloud\UploadFile\" + Session["HospID"].ToString().Trim();
                        if (Directory.Exists(sDir) == false)
                        {
                            DirectoryInfo di = Directory.CreateDirectory(sDir);
                        }

                        Session["targetNewFileName"] = HospID + "_" + Guid.NewGuid().ToString() + "_" + fileName;

                        var destinationPath = Path.Combine(sDir + "/", Session["targetNewFileName"].ToString());
                        file.SaveAs(destinationPath);

                        // 把上傳的紀錄寫至 UploadServer
                        var us = new sysUploadServer()
                        {
                            USRowid = Guid.NewGuid().ToString(),
                            //USHospRowid = Session["HospRowid"].ToString(),
                            USHospID = HospID,
                            USLoadFilename = Session["targetNewFileName"].ToString(),
                            USLoadDateTime = DateTime.Now,
                            USServerStatus = "S",
                            USRecordCount = 0,
                            USType = "L"
                        };

                        db_zmcms.sysUploadServer.Add(us);
                        db_zmcms.SaveChanges();
                    }
                }
            }

            //Return an empty string to signify success.
            return Content("");
        }

        public ActionResult Async_Remove(string[] fileNames)
        {
            // The parameter of the Remove action must be called "fileNames"
            if (Session["targetNewFileName"] != null)
            {
                string sDir = Server.MapPath("~/FileCloud") + @"/UploadFile/LISTemp/" + Session["HospID"].ToString().Trim();

                var deleteFile = Path.Combine(sDir + "/", Session["targetNewFileName"].ToString());
                if (System.IO.File.Exists(deleteFile))
                {
                    System.IO.File.Delete(deleteFile);
                }

                var us = (from u in db_zmcms.sysUploadServer where u.USLoadFilename == Session["targetNewFileName"].ToString() select u).FirstOrDefault();
                db_zmcms.sysUploadServer.Remove(us);
                db_zmcms.SaveChanges();
            }

            // Return an empty string to signify success
            return Content("");
        }

        #region 取得主機轉檔排程詳細狀態
        public ActionResult SysUploadServer_Read([DataSourceRequest] DataSourceRequest request, string sHospID)
        {
            DataSourceResult
                result = (from u in db_zmcms.sysUploadServer
                          let USServerStatus = (u.USServerStatus == "S" ? "待處理" :
                                                u.USServerStatus == "P" ? "處理中" :
                                                u.USServerStatus == "E" ? "已完成" : u.USServerStatus)
                          where u.USHospID == sHospID               // && u.USHospRowid.StartsWith(sDataType)
                          orderby u.USLoadDateTime
                          select new
                          {
                              u.USRowid,
                              u.USHospRowid,
                              u.USLoadDateTime,
                              u.USLoadFilename,
                              USServerStatus,
                              u.USRecordCount
                          }).ToDataSourceResult(request);

            return Json(result);
        }
        #endregion
    }
}