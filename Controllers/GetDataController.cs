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
        public string dbs, ic, userid, password;

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

        //#region 依傳入醫事機構代碼取得病人清單
        //public ActionResult GetPatient([DataSourceRequest]DataSourceRequest request, string sSearchString)
        //{
        //    if (Session["HospID"] != null)
        //    {
        //        string sHospID = Session["HospID"].ToString();

        //        GetLink();

        //        HISEntities db_his = new HISEntities(myClass.GetSQLConnectionString(dbs, "his" + sHospID, userid, password, @"res://*/Models.HISModel.csdl|res://*/Models.HISModel.ssdl|res://*/Models.HISModel.msl"));

        //        if (!String.IsNullOrEmpty(sSearchString))
        //        {
        //            DataSourceResult db_patient = (from p in db_his.patient
        //                                           where p.strUserAccount.Contains(sSearchString) || p.strName.Contains(sSearchString) || p.strIdno.Contains(sSearchString)
        //                                           orderby p.strUserAccount
        //                                           select new
        //                                           {
        //                                               hospID = sHospID,
        //                                               p.id,
        //                                               p.strIdno,
        //                                               strUserAccount = p.strUserAccount,
        //                                               p.strName
        //                                           }).ToDataSourceResult(request);

        //            return Json(db_patient);
        //        }
        //        else
        //        {
        //            DataSourceResult db_patient = (from p in db_his.patient
        //                                           select new
        //                                           {
        //                                               hospID = sHospID,
        //                                               p.id,
        //                                               strUserAccount = p.strUserAccount,
        //                                               p.strName
        //                                           }).ToDataSourceResult(request);

        //            return Json(db_patient);
        //        }
        //    }

        //    return Json("");
        //}
        //#endregion

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

        #region GetLaboratoryClass_取得檢驗大類項目
        public JsonResult GetLaboratoryClass()
        {
            var result = from im in db_antifat.laboratory_class orderby im.code select new { im.id, im.name };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region GetlisLaboratoryClass_取得檢驗大類項目
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
                var result = (from sh in db_zmcms.SysHospital
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
                var result = (from sh in db_zmcms.SysHospital
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
        //#region 依傳入醫事機構代碼取得病人清單
        //public JsonResult PatientBase(string HospID, string strUserId)
        //{
        //    List<string> myLists = new List<string>();

        //    if (!String.IsNullOrEmpty(HospID) && !String.IsNullOrEmpty(strUserId))
        //    {
        //        GetLink();

        //        HISEntities db_his = new HISEntities(myClass.GetSQLConnectionString(dbs, "his" + HospID, userid, password, @"res://*/Models.HISModel.csdl|res://*/Models.HISModel.ssdl|res://*/Models.HISModel.msl"));

        //        var db_patient = (from p in db_his.patient
        //                          where p.id == strUserId
        //                          select new
        //                          {
        //                              p.strIdno,
        //                              p.strUserAccount,
        //                              p.strName,
        //                              p.datBirthday,
        //                              p.bolisMale,
        //                              p.strTel
        //                          }).FirstOrDefault();

        //        if (db_patient != null)
        //        {
        //            myLists.Add(db_patient.strName);
        //            myLists.Add(db_patient.bolisMale == true ? "男" : "女");

        //            // 取得病人姓名
        //            //ViewBag.PatientName = db_patient.strName;
        //            // 取得病人姓名
        //            //ViewBag.Gender = db_patient.bolisMale == true ? "男" : "女";
        //            // 計算年齡
        //            Age myAge = CalculateAge((DateTime)db_patient.datBirthday, DateTime.Now);
        //            string sAge = myAge.Years + " 歲 " + myAge.Months + " 個月又 " + myAge.Days + " 天 ";
        //            string sMyAge = myAge.Years.ToString();

        //            myLists.Add(sAge);

        //            // 取該病人的量測資料
        //            var db_bodymeasure = (from b in db_antifat.body_measure
        //                                  where b.strUserAccount == db_patient.strIdno &&
        //                                  (b.floWeight > 0 && b.floHeight > 0)
        //                                  orderby b.datRecordTime descending
        //                                  select new
        //                                  {
        //                                      b.datRecordTime,
        //                                      b.floHeight,
        //                                      b.floWeight
        //                                  }).FirstOrDefault();

        //            if (db_bodymeasure != null)
        //            {
        //                myLists.Add(((DateTime)db_bodymeasure.datRecordTime).ToString("yyyy-MM-dd"));
        //                myLists.Add(db_bodymeasure.floHeight.ToString());
        //                myLists.Add(db_bodymeasure.floWeight.ToString());
        //            }
        //            else
        //            {
        //                myLists.Add("N/A");
        //                myLists.Add("0");
        //                myLists.Add("0");
        //            }

        //            myLists.Add(db_patient.strTel);
        //            myLists.Add(sMyAge);
        //        }
        //    }

        //    return Json(myLists, JsonRequestBehavior.AllowGet);
        //}
        //#endregion        
    }
}