using System;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
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
        public ActionResult LISBoardIndex(string HospID, string PTRowid)
        {            
            ViewBag.HospID = HospID;
            ViewBag.PTRowid = PTRowid;

            DataTable data = GetDataTable(HospID, PTRowid, null);

            // 取得檢驗類別的筆數以利產生動態圖表            
            if (Session["HospID"] != null)
            {
                string sHospID = Session["HospID"].ToString();

                GetLink();

                LIS_HISEntities db_his = new LIS_HISEntities(myClass.GetSQLConnectionString(dbs2, "his" + sHospID, userid, password, LIS_HISEntities_String));

                var db_lhli = (from lhli in db_his.lisHospitalLaboratoryItem select new ClassItem { HLIRowid = lhli.HLIRowid }).ToList();
                ViewBag.ClassItem = db_lhli;
            }
            else
            {
                ViewBag.ClassItem = null;
            }

            if (!String.IsNullOrEmpty(HospID) && !String.IsNullOrEmpty(PTRowid))
            {   
                return View(data);
            }

            return View(data);
        }

        public class ClassItem
        {
            public string HLIRowid { get; set; }
        }

        private DataTable GetDataTable(string HospID, string sPTRowid, string sJsonString)
        {
            //string sHospID = "3532040438";
            //sPTRowid = "8bb5060f-dcff-448f-8c59-8351802232a5";            
            if (String.IsNullOrEmpty(HospID) && Session["HospID"] != null)
            {
                HospID = Session["HospID"].ToString();
            }

            var dataTable = new DataTable();

            if (!String.IsNullOrEmpty(HospID) && !String.IsNullOrEmpty(sJsonString))
            {
                string sPattern = @"[\W*]";

                GetLink();

                LIS_HISEntities db_his = new LIS_HISEntities(myClass.GetSQLConnectionString(dbs2, "his" + HospID, userid, password, LIS_HISEntities_String));

                // 序列化
                LisClassTemplate db_data = JsonConvert.DeserializeObject<LisClassTemplate>(sJsonString);

                // 存放結果清單
                List<string> myLists = new List<string>();

                if (db_data != null)
                {
                    if (String.IsNullOrEmpty(HospID))
                    {
                        HospID = Session["HospID"].ToString();
                    }

                    if (!String.IsNullOrEmpty(sPTRowid))
                    {
                        string sCommand = @"SELECT FORMAT(PLMApplyDate,'yyyy-MM-dd') '檢驗日期', ";
                        string sIN = String.Empty;
                        foreach (var db in db_data.Data)
                        {
                            string sPLDCode = db.PLDCode;

                            // 取得檢驗項目名稱(簡稱)
                            var db_lisHospitalLaboratoryItem = (from lhli in db_his.lisHospitalLaboratoryItem where lhli.HLICode == sPLDCode select new { lhli.HLITitleName }).First();

                            string sPLDName = "";
                            if (db_lisHospitalLaboratoryItem != null)
                            {
                                sPLDName = db_lisHospitalLaboratoryItem.HLITitleName.Trim();
                                sPLDName = Regex.Replace(sPLDName, sPattern, "_");                                

                                sCommand += "[" + sPLDCode + "] '" + sPLDName + "', ";
                                //sCommand += "[" + sPLDCode + "], ";
                                sIN += "[" + sPLDCode + "],";
                            }                            
                        }

                        sCommand = sCommand.Trim().TrimEnd(',');
                        sIN = sIN.TrimEnd(',');
                        sCommand += @" FROM (
                                        SELECT PLMApplyDate, PLDCode, PLDValue FROM lisPatientLaboratoryMaster lplm 
                                        join lisPatientLaboratoryDetail lpld on lplm.PLMRowid = lpld.PLMRowid
	                                    where lplm.PTRowid = '{0}' 
                                        group by lplm.PLMApplyDate, PLDCode, PLDValue
                                    ) as GroupTable 
                                    PIVOT 
                                    (
                                        MAX(PLDValue)
                                        FOR PLDCode IN (" + sIN + @")
                                    ) AS PivotTable";

                        string sConn = string.Format(ConfigurationManager.ConnectionStrings["HIS_ConnectionString"].ConnectionString, "his" + HospID);
                        string sSQLString = String.Format(sCommand, sPTRowid);
                        using (var dataAdapter = new SqlDataAdapter(sSQLString, sConn))
                        {
                            dataAdapter.Fill(dataTable);
                            dataAdapter.FillSchema(dataTable, SchemaType.Mapped);
                        }
                    }
                }
            }

            return dataTable;
        }

        #region _LisPatientLaboratoryDateGroup-檢驗日期明細
        public ActionResult lisPatientLaboratory_Read([DataSourceRequest] DataSourceRequest request, string HospID, string strUserId)
        {
            DataSourceResult result = ("").ToDataSourceResult(request);

            if (!String.IsNullOrEmpty(HospID))
            {
                GetLink();

                LIS_HISEntities db_his = new LIS_HISEntities(myClass.GetSQLConnectionString(dbs2, "his" + HospID, userid, password, LIS_HISEntities_String));

                result = (from lplm in db_his.lisPatientLaboratoryMaster
                          where lplm.PTRowid == strUserId
                          join pt in db_his.Patient on lplm.PTRowid equals pt.id
                          orderby lplm.PLMApplyDate descending
                          select new ViewModel_lisPatientLaboratoryDateGroup()
                          {                              
                              HospID = HospID,
                              PLMRowid = lplm.PLMRowid,
                              PTRowid = lplm.PTRowid,
                              PTIdno = pt.strIdno,
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
        #endregion

        public ActionResult _GetChart(string sHospID, string sPTRowid, string sHLIRowid)
        {
            Array result = null;

            if (String.IsNullOrEmpty(sHospID) && Session["HospID"] != null)
            {
                sHospID = Session["HospID"].ToString();
            }

            if (!String.IsNullOrEmpty(sHospID) && !String.IsNullOrEmpty(sPTRowid) && !String.IsNullOrEmpty(sHLIRowid))
            {
                GetLink();

                LIS_HISEntities db_his = new LIS_HISEntities(myClass.GetSQLConnectionString(dbs2, "his" + sHospID, userid, password, LIS_HISEntities_String));

                var temp_result = (from lplm in db_his.lisPatientLaboratoryMaster where lplm.PTRowid == sPTRowid
                                   join lpld in db_his.lisPatientLaboratoryDetail on lplm.PLMRowid equals lpld.PLMRowid
                                   join lhli in db_his.lisHospitalLaboratoryItem on lpld.PLDCode equals lhli.HLICode
                                   where lhli.HLIRowid == sHLIRowid
                                   orderby lplm.PLMApplyDate 
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
        

        #region lisLaboratoryClassItem_Read-檢驗表列式明細
        // 檢驗表列式明細是依病人的歷年檢驗的所有項目做匯整(Group)並列出左側項目供使用者單選或複選或全選以利右側介面以橫向顯示病人的驗驗值
        public ActionResult lisLaboratoryClassItem_Read([DataSourceRequest] DataSourceRequest request, string HospID, string strUserId)
        {
            DataSourceResult result = ("").ToDataSourceResult(request);

            if (!String.IsNullOrEmpty(HospID))
            {
                GetLink();

                LIS_HISEntities db_his = new LIS_HISEntities(myClass.GetSQLConnectionString(dbs2, "his" + HospID, userid, password, LIS_HISEntities_String));

                var r1 = (from lplm in db_his.lisPatientLaboratoryMaster
                          where lplm.PTRowid == strUserId
                          join lpld in db_his.lisPatientLaboratoryDetail on lplm.PLMRowid equals lpld.PLMRowid
                          select new { lpld.PLDCode }).Distinct().ToList();

                result = (from r in r1 join lhli in db_his.lisHospitalLaboratoryItem on r.PLDCode equals lhli.HLICode 
                          select new ViewModel_lisPatientLaboratoryClassGroup()
                          {
                              HLIRowid = lhli.HLIRowid,
                              PLDCode = r.PLDCode,
                              HLIName = lhli.HLIName 
                          }).ToDataSourceResult(request);                
            }

            return Json(result);
        }

        public ActionResult lisPatientLaboratoryPOVIT_Read([DataSourceRequest] DataSourceRequest request, string HospID, string sPTRowid, string sJsonString)
        {
            var POVITdataTable = new DataTable();

            //string sHospID = "3532040438";
            //string sPTRowid = "8bb5060f-dcff-448f-8c59-8351802232a5";            

            if (!String.IsNullOrEmpty(sPTRowid))
            {
                POVITdataTable = GetDataTable(HospID, sPTRowid, sJsonString);
            }

            return Json(POVITdataTable.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        #endregion
    }

    public class LisClassTemplate
    {
        //public string data { get; set; }        
        public SelectClassData[] Data { get; set; }
    }

    public class SelectClassData
    {  
        public string PLDCode { get; set; }          
    }
}