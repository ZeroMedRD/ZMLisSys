using System;
using System.Net.Http;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
//using System.Data.Objects.SqlClient;
using System.Data.Entity;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ZMLISSys.ViewModels;
using ZMLISSys.Models;
using ZMLib;
using System.Globalization;
//using System.Net.Http.Headers;
//using Microsoft.Owin.Security.Provider;
//using System.Web.Mvc.Async;

namespace ZMLISSys.Controllers
{
    public class GetDataController : Controller
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

        public ActionResult GetCombo(string sText)
        {
            var result = from cm in db_zmcms.ComboMaster
                          where cm.CBMClass == sText
                          join cd in db_zmcms.ComboDetail on cm.CBMRowid equals cd.CBMRowid
                          orderby cd.CBDDisplayOrder ascending
                          select new
                          {
                              cd.CBDRowid,
                              cd.CBDCode,
                              cd.CBDDescription
                          };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #region PatientBase : 依傳入醫事機構代碼取得病人清單 by HospID, strUserId
        public JsonResult PatientBase(string HospID, string strUserId)
        {
            List<string> myLists = new List<string>();

            if (!String.IsNullOrEmpty(HospID) && !String.IsNullOrEmpty(strUserId))
            {
                GetLink();

                LIS_HISEntities db_his = new LIS_HISEntities(myClass.GetSQLConnectionString(dbs, "his" + HospID, userid, password, LIS_HISEntities_String));

                var db_patient = (from p in db_his.Patient
                                  where p.id == strUserId
                                  select new
                                  {
                                      p.strIdno,
                                      p.strUserAccount,
                                      p.strName,
                                      p.datBirthday,
                                      p.bolisMale,
                                      p.strTel
                                  }).FirstOrDefault();

                if (db_patient != null)
                {
                    myLists.Add(db_patient.strName);
                    myLists.Add(db_patient.bolisMale == true ? "男" : "女");

                    // 取得病人姓名
                    //ViewBag.PatientName = db_patient.strName;
                    // 取得病人姓名
                    //ViewBag.Gender = db_patient.bolisMale == true ? "男" : "女";
                    // 計算年齡
                    CAge myAge = GetCalculateAge((DateTime)db_patient.datBirthday, DateTime.Now);
                    string sAge = myAge.Years + " 歲 " + myAge.Months + " 個月又 " + myAge.Days + " 天 ";
                    string sMyAge = myAge.Years.ToString();

                    myLists.Add(sAge);

                    // 取該病人的量測資料
                    var db_bodymeasure = (from b in db_antifat.body_measure
                                          where b.strUserAccount == db_patient.strIdno &&
                                          (b.floWeight > 0 && b.floHeight > 0)
                                          orderby b.datRecordTime descending
                                          select new
                                          {
                                              b.datRecordTime,
                                              b.floHeight,
                                              b.floWeight
                                          }).FirstOrDefault();

                    if (db_bodymeasure != null)
                    {
                        myLists.Add(((DateTime)db_bodymeasure.datRecordTime).ToString("yyyy-MM-dd"));
                        myLists.Add(db_bodymeasure.floHeight.ToString());
                        myLists.Add(db_bodymeasure.floWeight.ToString());
                    }
                    else
                    {
                        myLists.Add("N/A");
                        myLists.Add("0");
                        myLists.Add("0");
                    }

                    myLists.Add(db_patient.strTel);
                    myLists.Add(sMyAge);
                }
            }

            return Json(myLists, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 依傳入醫事機構代碼取得病人清單 by sSearchString
        public ActionResult GetPatient([DataSourceRequest] DataSourceRequest request, string sSearchString)
        {
            if (Session["HospID"] != null)
            {
                string sHospID = Session["HospID"].ToString();

                GetLink();

                LIS_HISEntities db_his = new LIS_HISEntities(myClass.GetSQLConnectionString(dbs, "his" + sHospID, userid, password, LIS_HISEntities_String));

                if (!String.IsNullOrEmpty(sSearchString))
                {
                    DataSourceResult db_patient = (from p in db_his.Patient
                                                   where p.strUserAccount.Contains(sSearchString) || p.strName.Contains(sSearchString) || p.strIdno.Contains(sSearchString)
                                                   orderby p.strUserAccount
                                                   select new
                                                   {
                                                       hospID = sHospID,
                                                       p.id,
                                                       p.strIdno,
                                                       strUserAccount = p.strUserAccount,
                                                       p.strName
                                                   }).ToDataSourceResult(request);

                    return Json(db_patient);
                }
                else
                {
                    DataSourceResult db_patient = (from p in db_his.Patient
                                                   select new
                                                   {
                                                       hospID = sHospID,
                                                       p.id,
                                                       strUserAccount = p.strUserAccount,
                                                       p.strName
                                                   }).ToDataSourceResult(request);

                    return Json(db_patient);
                }
            }

            return Json("");
        }
        #endregion

        #region 依傳入醫事機構代碼取得該醫事機構檢驗項目清單
        public JsonResult GetLaboratoryItem(string HospID)
        {
            if (string.IsNullOrEmpty(HospID))
            {
                // 依使用者權限取得所屬的醫事機構選項
                var result = (from li in db_antifat.laboratory_item
                                  //where li.laboratory_clinic_code == HospID
                              orderby li.code descending
                              select new { li.id, li.name });
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                // 依傳入的參數取得醫事機構選項
                var result = (from li in db_antifat.laboratory_item
                              where li.laboratory_clinic_code == HospID
                              orderby li.code descending
                              select new { li.id, li.name });
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region GetLaboratoryClinic_取得檢驗所清單
        public JsonResult GetLaboratoryClinic()
        {
            var result = from im in db_antifat.laboratory_clinic orderby im.code select new { im.code, im.name };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region GetLaboratoryClass_取得檢驗大類項目 V1.0
        public JsonResult GetLaboratoryClass()
        {
            var result = from im in db_antifat.laboratory_class orderby im.code select new { im.id, im.name };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region GetlisLaboratoryClass_取得檢驗大類項目 V2.0
        public JsonResult GetlisLaboratoryClass()
        {
            var result = (from llc in db_zmlis.lisLaboratoryClass orderby llc.LLCCode select new { llc.LLCRowid, llc.LLCTrdCName }).ToList();
            result.Insert(0, new { LLCRowid = "", LLCTrdCName = "所有資料" } );            

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 取得 醫事機構名稱選項，若傳入參數 sHospID 空白或 Null 則取所有有開通的醫事機構名單，反之，則取該傳入的代號做為選單取的依據
        public JsonResult GetHospital(string sHospID)
        {
            if (string.IsNullOrEmpty(sHospID))
            {
                // 依使用者權限取得所屬的醫事機構選項
                var result = (from sh in db_zmcms.sysHospital
                              where sh.HospActive == true && sh.HospID != "0000000000"
                              orderby sh.HospID ascending
                              select new {
                                  sh.HospRowid,
                                  sh.HospID,
                                  sh.HospName,
                                  sh.HospAddress
                              });
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                // 依傳入的參數取得醫事機構選項
                var result = (from sh in db_zmcms.sysHospital
                              where sh.HospID == sHospID && sh.HospActive == true && sh.HospID != "0000000000"
                              orderby sh.HospID ascending
                              select new {
                                  sh.HospRowid,
                                  sh.HospID,
                                  sh.HospName,
                                  sh.HospAddress
                              });
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region GetLLIRowid
        public JsonResult GetLLIRowid()
        {
            var result = from lli in db_zmlis.lisLaboratoryItem where lli.LLIKind=="00" select new { lli.LLIRowid, lli.LLITrdCName };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region GetLLISubRowid
        public JsonResult GetLLISubRowid()
        {
            var result = from lli in db_zmlis.lisLaboratoryItem where lli.LLIKind == "01" select new { lli.LLIRowid, lli.LLITrdCName };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 實際年齡計算
        public static CAge GetCalculateAge(DateTime birthDate, DateTime endDate)
        {
            if (birthDate.Date > endDate.Date)
            {
                throw new ArgumentException("birthDate cannot be higher then endDate", "birthDate");
            }

            int years = endDate.Year - birthDate.Year;
            int months = 0;
            int days = 0;

            // Check if the last year, was a full year.
            if (endDate < birthDate.AddYears(years) && years != 0)
            {
                years--;
            }

            // Calculate the number of months.
            birthDate = birthDate.AddYears(years);

            if (birthDate.Year == endDate.Year)
            {
                months = endDate.Month - birthDate.Month;
            }
            else
            {
                months = (12 - birthDate.Month) + endDate.Month;
            }

            // Check if last month was a complete month.
            if (endDate < birthDate.AddMonths(months) && months != 0)
            {
                months--;
            }

            // Calculate the number of days.
            birthDate = birthDate.AddMonths(months);

            days = (endDate - birthDate).Days;
            CAge result;
            result.Years = years;
            result.Months = months;
            result.Days = days;
            return result;
        }
        #endregion

        public JsonResult GetLisTemplate(string jsonString)
        {
            LisTemplate db_data = JsonConvert.DeserializeObject<LisTemplate>(jsonString);

            // 存放結果清單
            List<string> myLists = new List<string>();

            if (db_data != null)
            {
                HospitalParameters hp = new HospitalParameters();

                // 取得醫事機構的參數設定
                if (Session["HospID"] != null && Session["HospRowid"] != null)
                {
                    string sHospRowid = Session["HospRowid"].ToString();
                    string sHospID = Session["HospID"].ToString();

                    // change hospital database
                    GetLink();
                    LIS_HISEntities db_his = new LIS_HISEntities(myClass.GetSQLConnectionString(dbs2, "his" + sHospID, userid, password, LIS_HISEntities_String));
                    
                    var db_hospParam = (from shp in db_zmcms.sysHospitalParam where shp.HospRowid == sHospRowid select shp).First();

                    if (db_hospParam != null)       // 表示有找到數資料
                    {
                        hp.HospLisDateStyle = db_hospParam.HospLisDateStyle;
                        hp.HospLisDateFormat = db_hospParam.HospLisDateFormat;
                        hp.HospLisItemTemp01 = db_hospParam.HospLisItemTemp01;
                        hp.HospLisItemTemp02 = db_hospParam.HospLisItemTemp02;
                        hp.HospLisItemTemp03 = db_hospParam.HospLisItemTemp03;
                        hp.HospLisItemTemp04 = db_hospParam.HospLisItemTemp04;
                        hp.HospLisItemAndValueTemplate = db_hospParam.HospLisItemAndValueTemplate;
                        hp.HospLisDelimiter = db_hospParam.HospLisDelimiter;
                        hp.HospLisEndingSymbol = db_hospParam.HospLisEndingSymbol;
                        hp.HospLisRowCount = db_hospParam.HospLisRowCount;
                        hp.HospLisColumnSpace = db_hospParam.HospLisColumnSpace;
                    }
                    // 帶 Default
                    else
                    {
                        hp.HospLisDateStyle = "yyyy-MM-dd";
                        hp.HospLisDateFormat = "{0}:";
                        hp.HospLisItemTemp01 = false;
                        hp.HospLisItemTemp02 = true;
                        hp.HospLisItemTemp03 = false;
                        hp.HospLisItemTemp04 = false;
                        hp.HospLisItemAndValueTemplate = "{0}:{1}";
                        hp.HospLisDelimiter = ",";
                        hp.HospLisEndingSymbol = ".";
                        hp.HospLisRowCount = 3;
                        hp.HospLisColumnSpace = 25;
                    }

                    // 取得日期格式是否是國曆型態
                    var stwc = (from cbd in db_zmcms.ComboDetail where cbd.CBDCode == hp.HospLisDateFormat select new { CBDDescription = cbd.CBDDescription }).First();

                    DateTime dt = DateTime.Now;
                    CultureInfo culture = new CultureInfo("zh-TW");
                    culture.DateTimeFormat.Calendar = new TaiwanCalendar();
                    string sdt = stwc.CBDDescription.Contains("國") == true ? dt.ToString(hp.HospLisDateFormat, culture) : dt.ToString(hp.HospLisDateFormat);

                    // 日期顯示結果
                    string result = String.Format(hp.HospLisDateStyle, sdt);

                    // 檢驗項目呈現方式
                    int icount = 0;
                    string sHospLisItemTemp = "";
                    string sColumnSpace = "&nbsp;";

                    foreach (var db in db_data.Data)
                    {
                        string sPLDCode = db.PLDCode;
                        string sPLDValue = db.PLDValue;

                        // 取得檢驗代碼、檢驗簡稱、英文全名、中文全名
                        var db_laboratoryItem = (from li in db_his.lisHospitalLaboratoryItem where li.HLICode == sPLDCode select li).First();
                        if (db_laboratoryItem != null)
                        {
                            sHospLisItemTemp += (hp.HospLisItemTemp01 == true) ? String.Format("{0}  ", db_laboratoryItem.HLICode) : "";     // 檢驗代碼
                            sHospLisItemTemp += (hp.HospLisItemTemp02 == true) ? String.Format("{0}  ", db_laboratoryItem.HLIName) : "";     // 檢驗簡稱
                            sHospLisItemTemp += (hp.HospLisItemTemp03 == true) ? String.Format("{0}  ", db_laboratoryItem.HLIName) : "";     // 英文全名
                            sHospLisItemTemp += (hp.HospLisItemTemp04 == true) ? String.Format("{0}  ", db_laboratoryItem.HLIName) : "";     // 中文全名
                        }
                        else
                        {
                            sHospLisItemTemp += (hp.HospLisItemTemp01 == true) ? String.Format("{0}  ", sPLDCode) : "";     // 檢驗代碼
                            sHospLisItemTemp += (hp.HospLisItemTemp02 == true) ? String.Format("{0}  ", sPLDCode) : "";     // 檢驗簡稱
                            sHospLisItemTemp += (hp.HospLisItemTemp03 == true) ? String.Format("{0}  ", sPLDCode) : "";     // 英文全名
                            sHospLisItemTemp += (hp.HospLisItemTemp04 == true) ? String.Format("{0}  ", sPLDCode) : "";     // 中文全名
                        }                        

                        if (icount < hp.HospLisRowCount)
                        {
                            icount++;
                        }
                        else
                        {
                            icount = 1;
                            result += Environment.NewLine + sColumnSpace.Repeat(hp.HospLisColumnSpace);
                        }

                        result += String.Format(hp.HospLisItemAndValueTemplate, sHospLisItemTemp, sPLDValue) + hp.HospLisDelimiter;

                        sHospLisItemTemp = "";
                    }

                    // 去掉最後一個的分隔符號
                    result = result.Substring(0, result.LastIndexOf(hp.HospLisDelimiter));

                    // 加上結尾符號
                    result += hp.HospLisEndingSymbol;

                    myLists.Add(result);
                }
            }

            return Json(myLists, JsonRequestBehavior.AllowGet);
        }
    }

    public struct CAge
    {
        public int Years;
        public int Months;
        public int Days;
    }

    public class LisTemplate
    {
        //public string data { get; set; }        
        public LisData[] Data { get; set; }        
    }

    public class LisData
    {
        //public string data { get; set; }        
        public string PLDCode { get; set; }
        public string PLDValue { get; set; }
    }

    public class HospitalParameters
    {
        public string HospLisDateStyle { get; set; }
        public string HospLisDateFormat { get; set; }
        public bool HospLisItemTemp01 { get; set; }
        public bool HospLisItemTemp02 { get; set; }
        public bool HospLisItemTemp03 { get; set; }
        public bool HospLisItemTemp04 { get; set; }
        public string HospLisItemAndValueTemplate { get; set; }
        public string HospLisDelimiter { get; set; }
        public string HospLisEndingSymbol { get; set; }
        public int HospLisRowCount { get; set; }
        public int HospLisColumnSpace { get; set; }
    }
}