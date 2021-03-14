using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.Configuration;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ZMLISSys.Models;
using ZMLISSys.ViewModels;
using ZMLib;
using DHOLib;

namespace ZMLISSys.Controllers
{
    public class LISUploadController : Controller
    {
        private LIS_AntifatEntities db_antifat = new LIS_AntifatEntities();
        private ZMLISEntities db_zmlis = new ZMLISEntities();
        private LIS_ZMCMSv2Entities db_zmcms = new LIS_ZMCMSv2Entities();

        ZMClass myClass = new ZMClass();
        DHOClass myDHOClass = new DHOClass();

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
                              where lplm.PLMApplyDate >= sdt && lplm.PLMApplyDate <= edt
                              join du in db_zmcmsdata.DHO_Users on lplm.PLMPTIdno equals du.user_id into b
                              from obj in b.DefaultIfEmpty()
                              select new
                              {
                                  PLMRowid = lplm.PLMRowid,
                                  PLMPTRowid = lplm.PTRowid,
                                  PLMPTIdno = lplm.PLMPTIdno,
                                  PLMPTName = lplm.PLMPTName,
                                  PLMPTBirthday = lplm.PLMPTBirthday,
                                  PLMPTGender = lplm.PLMPTGender,
                                  PLMPTCode = lplm.PLMPTCode,
                                  PLMClinicDate = lplm.PLMClinicDate,
                                  PLMApplyDate = lplm.PLMApplyDate,
                                  PLMApplyTime = lplm.PLMApplyTime,
                                  PLMInspDate = lplm.PLMInspDate,
                                  PLMInspTime = lplm.PLMInspTime,
                                  PLMReportDate = lplm.PLMReportDate,
                                  PLMReportTime = lplm.PLMReportTime,
                                  PLMSNo = lplm.PLMSNo,
                                  PLMReqno = lplm.PLMReqno,
                                  APPFlag = (obj.user_id == null) ? false : true
                              }).ToDataSourceResult(request);
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
        /// <summary>
        ///     取得主機轉檔排程詳細狀態
        /// </summary>
        /// <param name="request"></param>
        /// <param name="sHospID">醫事機構代碼</param>
        /// <returns></returns>
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

        #region 推送病人檢驗資料至名醫實時通APP
        /// <summary>
        ///     推送病人檢驗資料至名醫實時通APP
        /// </summary>
        /// <param name="HospID">醫事機構代碼</param>
        /// <param name="UserId">使用者身份證ID</param>
        /// <param name="PLMRowid">檢驗主檔資料序號</param>
        /// <returns>
        ///     sReturnCode:回應代碼 
        ///     00=推送成功
        ///     900=沒授權
        ///     901=取不到 Token
        ///     902=取不到病人的檢驗資料
        ///     998=Get Token發生了 exception 狀況
        ///     999=推送失敗
        /// </returns>        
        public string PushDHO(string HospID, string UserId, string PLMRowid)
        {   
            string sReturnCode = String.Empty;
            string sOrg_code = "TW" + HospID;

            // 確認傳進來的病人是否有在名醫實時通裡面授權
            var db_dho = (from du in db_zmcms.DHO_Users
                          where du.user_id == UserId && du.org_code == sOrg_code
                          select new
                          {
                              du.org_code,
                              du.dho_uid,
                              du.user_id
                          }).First();

            if (db_dho != null)     // 表示名醫實時通裡面有資料可以推播
            {
                // 開始傳送檢驗資料
                // 取得檢驗資料                
                LIS_HISEntities db_his = new LIS_HISEntities();
                LISDATA db_lisdata = new LISDATA();

                List<USER> listUser = new List<USER>();                
                string access_token = String.Empty;
                string sHospID = HospID;            // 病人所屬的醫事機構代碼
                string sUserId = UserId;            // 病人的資料 id
                string sDHOUid = db_dho.dho_uid;    // 病人的 DHO uid

                #region 切換醫療機構所屬資料庫
                GetLink();
                db_his = new LIS_HISEntities(myClass.GetSQLConnectionString(dbs, "his" + sHospID, userid, password, @"res://*/Models.LIS_HISModel.csdl|res://*/Models.LIS_HISModel.ssdl|res://*/Models.LIS_HISModel.msl"));
                #endregion

                // 取得病人的檢驗資料及檢驗數值。
                var lis_result = from lplm in db_his.lisPatientLaboratoryMaster
                                 where lplm.PLMRowid == PLMRowid
                                 join lpld in db_his.lisPatientLaboratoryDetail on lplm.PLMRowid equals lpld.PLMRowid
                                 join lhli in db_his.lisHospitalLaboratoryItem on lpld.PLDCode equals lhli.HLICode
                                 where lhli.LLISubRowid != null && lhli.LLISubRowid != ""
                                 join lli in db_his.lisLaboratoryItem on lhli.LLISubRowid equals lli.LLIRowid
                                 select new
                                 {
                                     lli.LLINhiCode,
                                     lli.LLIUnit,
                                     lhli.LLISubRowid,
                                     lplm.PLMPTIdno,
                                     lplm.PLMPTName,
                                     lplm.PLMApplyDate,
                                     lpld.PLDCode,
                                     lpld.PLDName,
                                     lpld.PLDValue,
                                     lpld.PLDStrValue
                                 };

                // 若有檢驗資料執行下面程序
                if (lis_result != null)
                {
                    GENERAL db_general = new GENERAL();
                    List<EXAMS> listEXAMS = new List<EXAMS>();
                    List<EXAMITEMS> listEXAMITEMS = new List<EXAMITEMS>();
                    DateTime dtPLMApplyDate = DateTime.Now;
                    foreach (var dt_lis in lis_result)
                    {
                        dtPLMApplyDate = (DateTime)dt_lis.PLMApplyDate;

                        listEXAMITEMS.Add(new EXAMITEMS
                        {
                            exam_item_id = dt_lis.LLINhiCode,
                            prefered_unit = dt_lis.LLIUnit,
                            si_value = (String.IsNullOrEmpty(dt_lis.PLDStrValue)) ? dt_lis.PLDValue.ToString() : "0",
                            cu_value = (String.IsNullOrEmpty(dt_lis.PLDStrValue)) ? dt_lis.PLDValue.ToString() : "0",
                            cu_string = (String.IsNullOrEmpty(dt_lis.PLDStrValue)) ? "" : dt_lis.PLDStrValue,
                            si_string = (String.IsNullOrEmpty(dt_lis.PLDStrValue)) ? "" : dt_lis.PLDStrValue
                        });
                    }

                    if (listEXAMITEMS.Count > 0)
                    {
                        listEXAMS.Add(new EXAMS
                        {
                            resource_id = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                            exam_name = sHospID,
                            exam_date = dtPLMApplyDate.ToString("yyyy-MM-dd HH:mm:ss"),
                            exam_items = listEXAMITEMS
                        });

                        listUser.Add(new USER { dho_uid = sDHOUid, general = db_general, exams = listEXAMS });
                    }
                }

                // 如果上面的檢驗有取到資料則執行下面的程序
                if (listUser.Count > 0)
                {
                    // Get Access_Token
                    // var access_token = myDHOClass.GetAccessTokens("TW" + sHospID + "*");
                    // var access_token = myDHOClass.GetAccessTokens("CMSTEST001*");
                    // Get Access_Token
                    try
                    {
                        access_token = myDHOClass.GetAccessTokens("TW" + sHospID + "*");

                        if (!String.IsNullOrEmpty(access_token))
                        {
                            db_lisdata.access_token = access_token;
                            db_lisdata.nrange = "NR_YEADONG";
                            db_lisdata.users = listUser;

                            LISJSONDATA gt = new LISJSONDATA();
                            gt.data = JsonConvert.SerializeObject(db_lisdata);
                            string sJsonString = JsonConvert.SerializeObject(gt);

                            // 推送資料
                            sReturnCode = Post2DHO(sJsonString);
                        }
                        else
                        { sReturnCode = "901"; }
                    }
                    catch (Exception e)
                    {
                        sReturnCode = e.ToString();
                        sReturnCode = "998";
                    }
                }
                else
                { sReturnCode = "902"; }

                // 無論有無推播都要把 listUser 清空.
                listUser.Clear();
            }
            else
            { sReturnCode = "900";  }

            // 無論推送成功否一彿

            return (sReturnCode);
        }
        #endregion

        #region Post2DHO - 推播資料正式推到名醫實時通
        private string Post2DHO(string sJSONString)
        {
            string sMessage = String.Empty;
            string sAPIUrl = String.Empty;
            string sReturnCode = String.Empty;

            sAPIUrl = "https://aapiprdhcm.dochereonline.com/import-cms-data";

            HttpClient client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, sAPIUrl);
            requestMessage.Content = new StringContent(sJSONString, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.SendAsync(requestMessage).GetAwaiter().GetResult();

            if (response.StatusCode.ToString() == "OK")
            {
                string r = response.Content.ReadAsStringAsync().Result.ToString();
                GetToken1 MS = JsonConvert.DeserializeObject<GetToken1>(r);                
                sReturnCode = MS.status;
            }
            else
            {
                sReturnCode = "999";
            }

            return (sReturnCode);
        }
        #endregion
    }
}