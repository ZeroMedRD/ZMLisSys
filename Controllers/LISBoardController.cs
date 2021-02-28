using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ZMLISSys.ViewModels;
using ZMLISSys.Models;
using ZMLib;

namespace ZMLISSys.Controllers
{
    public class LISBoardController : Controller
    {
        private LIS_AntifatEntities db_antifat = new LIS_AntifatEntities();
        private LIS_ZMCMSv2Entities db_zmcms = new LIS_ZMCMSv2Entities();
        private ZMLISEntities db_zmlis = new ZMLISEntities();

        ZMClass myClass = new ZMClass();
        public string dbs, dbs2, ic, userid, password;

        public string LIS_HISEntities_String = @"res://*/Models.LIS_HISModel.csdl|res://*/Models.LIS_HISModel.ssdl|res://*/Models.LIS_HISModel.msl";

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
            dbs = eflink.Server;
            dbs2 = eflink.Server2;
            ic = eflink.InitialCatalog;
            userid = eflink.UserID;
            password = eflink.Password;
        }
        #endregion

        // GET: LISBoard
        public ActionResult LISBoardIndex(string HospID, string sUserId)
        {
            if (!String.IsNullOrEmpty(HospID) && !String.IsNullOrEmpty(sUserId))
            {
                return View();
            }

            return View();
        }

        public ActionResult lisPatientLaboratory_Read([DataSourceRequest] DataSourceRequest request, string HospID, string strUserId)
        {
            DataSourceResult result = ("").ToDataSourceResult(request);

            if (!String.IsNullOrEmpty(HospID))
            {
                GetLink();

                LIS_HISEntities db_his = new LIS_HISEntities(myClass.GetSQLConnectionString(dbs2, "his" + HospID, userid, password, LIS_HISEntities_String));

                result = (from lplm in db_his.lisPatientLaboratoryMaster
                          where lplm.PTRowid == strUserId
                          orderby lplm.PLMApplyDate descending
                          select new ViewModel_lisPatientLaboratoryDateGroup()
                          {                              
                              HospID = HospID,
                              PLMRowid = lplm.PLMRowid,
                              PTRowid = lplm.PTRowid,
                              PLMPTCode = lplm.PLMPTCode,
                              PLMSNo = lplm.PLMSNo,
                              PLMApplyDate = (DateTime)lplm.PLMApplyDate
                          }).ToDataSourceResult(request);
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
                if (myClass.DatabaseOrTableExist(dbs2, ic, userid, password, "his" + sHospID, null, 0) == true)
                {
                    LIS_HISEntities db_zmcmsdata = new LIS_HISEntities(myClass.GetSQLConnectionString(dbs2, "his" + sHospID, userid, password, LIS_HISEntities_String));

                    result = (from lpld in db_zmcmsdata.lisPatientLaboratoryDetail where lpld.PLMRowid == sPLMRowid 
                              join lhli in db_zmcmsdata.lisHospitalLaboratoryItem on lpld.PLDCode equals lhli.HLICode
                              select new
                              {                                  
                                  lpld.PLDRowid,
                                  lpld.PLMRowid,
                                  lpld.HLIRowid,
                                  lpld.PLDCode,
                                  lpld.PLDName,
                                  PLDValue = (lpld.PLDValue !=0) ? lpld.PLDValue.ToString() : lpld.PLDStrValue,
                                  lpld.PLDUnit,
                                  lpld.PLDMemo,
                                  lpld.PLDType,
                                  lhli.HLISeqno
                              }).ToDataSourceResult(request);
                }
            }

            return Json(result);
        }

        public ActionResult _GetChart(string sHospID, string sPTRowid, string sPLDCode)
        {
            Array result = null;

            if (!String.IsNullOrEmpty(sHospID) && !String.IsNullOrEmpty(sPTRowid) && !String.IsNullOrEmpty(sPLDCode))
            {
                GetLink();

                LIS_HISEntities db_his = new LIS_HISEntities(myClass.GetSQLConnectionString(dbs2, "his" + sHospID, userid, password, LIS_HISEntities_String));

                var temp_result = (from lplm in db_his.lisPatientLaboratoryMaster where lplm.PTRowid == sPTRowid
                                   join lpld in db_his.lisPatientLaboratoryDetail on lplm.PLMRowid equals lpld.PLMRowid 
                                   where lpld.PLDCode == sPLDCode orderby lplm.PLMApplyDate 
                                   select new
                                   {
                                       PLMApplyDate = (DateTime)lplm.PLMApplyDate,
                                       PLDValue = lpld.PLDValue
                                   }).AsEnumerable();

                result = (from lci in temp_result
                          select new ViewModel_LISChart
                          {
                              PLMApplyDate = lci.PLMApplyDate.ToString("yyyy-MM-dd"),
                              PLDValue = (decimal)lci.PLDValue
                          }).ToArray();
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LPLDChart_Read([DataSourceRequest] DataSourceRequest request, string sHospID, string sPTRowid, string sPLDCode)
        {
            DataSourceResult result = ("").ToDataSourceResult(request);

            if (!String.IsNullOrEmpty(sHospID) && !String.IsNullOrEmpty(sPTRowid) && !String.IsNullOrEmpty(sPLDCode))
            {
                GetLink();

                LIS_HISEntities db_his = new LIS_HISEntities(myClass.GetSQLConnectionString(dbs2, "his" + sHospID, userid, password, LIS_HISEntities_String));

                result = (from lplm in db_his.lisPatientLaboratoryMaster
                          where lplm.PTRowid == sPTRowid
                          join lpld in db_his.lisPatientLaboratoryDetail on lplm.PLMRowid equals lpld.PLMRowid
                          where lpld.PLDCode == sPLDCode
                          orderby lplm.PLMApplyDate
                          select new
                          {
                              lpld.PLDCode,
                              lpld.PLDName,
                              lplm.PLMApplyDate,
                              lpld.PLDValue,
                              PLDType = (lpld.PLDType == "H") ? "↑" : (lpld.PLDType == "L") ? "↓" : "",
                              lpld.PLDUnit,
                              lpld.PLDMemo
                          }).ToDataSourceResult(request);
            }

            return Json(result);
        }
    }
}