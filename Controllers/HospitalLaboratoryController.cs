using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ZMLISSys.Models;
using ZMLISSys.ViewModels;
using ZMLib;

namespace ZMLISSys.Controllers
{
    public class HospitalLaboratoryController : Controller
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

        // GET: LaboratoryHospital
        public ActionResult LHIndex()
        {
            return View();
        }

        public ActionResult H2LIndex()
        {
            return View();
        }

        #region HospitalLaboratoryItem CRUD
        public ActionResult HosptialLaboratoryItem_Read([DataSourceRequest] DataSourceRequest request, string sHospID, string sLLCRowid, string sSearchString)
        {
            DataSourceResult result = ("").ToDataSourceResult(request);

            if (String.IsNullOrEmpty(sHospID) != true)
            {
                GetLink();

                // 判斷資料庫是否存在
                if (myClass.DatabaseOrTableExist(dbs, ic, userid, password, "his" + sHospID, null, 0) == true)
                {
                    LIS_HISEntities db_zmcmsdata = new LIS_HISEntities(myClass.GetSQLConnectionString(dbs, "his" + sHospID, userid, password, @"res://*/Models.LIS_HISModel.csdl|res://*/Models.LIS_HISModel.ssdl|res://*/Models.LIS_HISModel.msl"));

                    List<lisLaboratoryItem> db_lli = new List<lisLaboratoryItem>();

                    if (!String.IsNullOrEmpty(sLLCRowid))
                    {
                        db_lli = (from lli in db_zmlis.lisLaboratoryItem where lli.LLCRowid == sLLCRowid select lli).ToList();
                    }
                    else if (!String.IsNullOrEmpty(sSearchString))
                    {
                        db_lli = (from lli in db_zmlis.lisLaboratoryItem
                                  where (lli.LLINhiCode.Contains(sSearchString) ||
                                         lli.LLITrdCName.Contains(sSearchString) ||
                                         lli.LLIEngName.Contains(sSearchString)) 
                                  select lli
                                  ).ToList();
                    }
                    else
                    {
                        db_lli = (from lli in db_zmlis.lisLaboratoryItem select lli).ToList();
                    }

                    var db = from lli in db_lli
                             join llc in db_zmlis.lisLaboratoryClass on lli.LLCRowid equals llc.LLCRowid into ps1
                             from llc in ps1.DefaultIfEmpty()
                             select new
                             {
                                 LLIRowid = lli.LLIRowid,
                                 LLCRowid = lli.LLCRowid,
                                 LLCTrdCName = llc.LLCTrdCName,
                                 LLINhiCode = lli.LLINhiCode,
                                 LLITrdCName = lli.LLITrdCName,
                                 LLIEngName = lli.LLIEngName,
                                 LLINhiCost = (float)lli.LLINhiCost,
                                 LLICostType = lli.LLICostType,
                                 LLIType = lli.LLIType,
                                 LLIUnit = lli.LLIUnit
                             };

                    result = (from lli in db
                              join t1 in db_zmcmsdata.lisHospitalLaboratoryItem on lli.LLIRowid equals t1.LLIRowid into ps1
                              from rs1 in ps1.DefaultIfEmpty()
                              orderby lli.LLINhiCode ascending
                              select new ViewModel_HospitalLaboratoryItem()
                              {
                                  HospID = sHospID,
                                  IsChecked = rs1 == null ? false : true,
                                  LLIRowid = lli.LLIRowid,
                                  LLCRowid = lli.LLCRowid,
                                  LLCTrdCName = lli.LLCTrdCName,
                                  HLIRowid = rs1 == null ? "" : rs1.HLIRowid,
                                  LLINhiCode = lli.LLINhiCode,
                                  LLITrdCName = lli.LLITrdCName + " " + lli.LLIEngName,
                                  LLIEngName = lli.LLIEngName,
                                  LLINhiCost = (float)lli.LLINhiCost,
                                  LLIUnit = lli.LLIUnit,
                                  HLICode = rs1 == null ? "" : rs1.HLICode,
                                  HLIDisplayRange = rs1 == null ? "" : rs1.HLIDisplayRange,
                                  HLILoMale = rs1 == null || rs1.HLILoMale == null ? 0 : (float)rs1.HLILoMale,
                                  HLIUpMale = rs1 == null || rs1.HLIUpMale == null ? 0 : (float)rs1.HLIUpMale,
                                  HLILoFemale = rs1 == null || rs1.HLILoFemale == null ? 0 : (float)rs1.HLILoFemale,
                                  HLIUpFemale = rs1 == null || rs1.HLIUpFemale == null ? 0 : (float)rs1.HLIUpFemale
                              }).ToDataSourceResult(request);
                }
            }

            return Json(result);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult HosptialLaboratoryItem_Create([DataSourceRequest] DataSourceRequest request, ViewModel_HospitalLaboratoryItem crud, string sHospID, string sRowid)
        {
            GetLink();

            LIS_HISEntities db_zmcmsdata = new LIS_HISEntities(myClass.GetSQLConnectionString(dbs, "his" + sHospID, userid, password, @"res://*/Models.LIS_HISModel.csdl|res://*/Models.LIS_HISModel.ssdl|res://*/Models.LIS_HISModel.msl"));

            if (ModelState.IsValid)
            {
                var entity = new lisHospitalLaboratoryItem
                {
                    HLIRowid = Guid.NewGuid().ToString(),
                    LLIRowid = sRowid
                };

                db_zmcmsdata.lisHospitalLaboratoryItem.Add(entity);
                db_zmcmsdata.SaveChanges();
            }
            

            return Json(new[] { crud }.ToDataSourceResult(new DataSourceRequest(), ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult HosptialLaboratoryItem_Update([DataSourceRequest] DataSourceRequest request, ViewModel_HospitalLaboratoryItem crud)
        {
            if (Session["HospID"] != null)
            {
                string sHospID = Session["HospID"].ToString();

                GetLink();

                LIS_HISEntities db_zmcmsdata = new LIS_HISEntities(myClass.GetSQLConnectionString(dbs, "his" + sHospID, userid, password, @"res://*/Models.LIS_HISModel.csdl|res://*/Models.LIS_HISModel.ssdl|res://*/Models.LIS_HISModel.msl"));

                if (ModelState.IsValid)
                {
                    var entity = new lisHospitalLaboratoryItem
                    {
                        LLIRowid = crud.LLIRowid,
                        HLIRowid = crud.HLIRowid,                        
                        HLICode = crud.HLICode,
                        HLIDisplayRange = crud.HLIDisplayRange,
                        HLILoMale = crud.HLILoMale,
                        HLIUpMale = crud.HLIUpMale,
                        HLILoFemale = crud.HLILoFemale,
                        HLIUpFemale = crud.HLIUpFemale
                    };

                    db_zmcmsdata.lisHospitalLaboratoryItem.Attach(entity);
                    db_zmcmsdata.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                    db_zmcmsdata.SaveChanges();
                }
            }

            return Json(new[] { crud }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]        
        public ActionResult HosptialLaboratoryItem_Destory([DataSourceRequest] DataSourceRequest request, string sHospID, string sRowid)
        {
            GetLink();
            
            LIS_HISEntities db_zmcmsdata = new LIS_HISEntities(myClass.GetSQLConnectionString(dbs, "his" + sHospID, userid, password, @"res://*/Models.LIS_HISModel.csdl|res://*/Models.LIS_HISModel.ssdl|res://*/Models.LIS_HISModel.msl"));

            if (ModelState.IsValid)
            {
                var entity = new lisHospitalLaboratoryItem
                {
                    HLIRowid = sRowid
                };

                db_zmcmsdata.lisHospitalLaboratoryItem.Attach(entity);
                db_zmcmsdata.lisHospitalLaboratoryItem.Remove(entity);
                db_zmcmsdata.SaveChanges();
            }

            return Json("");   //new[] { crud }.ToDataSourceResult(request, ModelState)
        }
        #endregion

        #region SysHospital Read        
        public ActionResult SysHospital_Read([DataSourceRequest] DataSourceRequest request, string sSearchString)
        {
            //DataSourceResult result = ("").ToDataSourceResult(request);

            if (!String.IsNullOrEmpty(sSearchString))
            {
                DataSourceResult result = (from sh in db_zmcms.SysHospital
                          where sh.HospActive == true && sh.HospID != "0000000000" &&                          
                          (sh.HospID.Contains(sSearchString) ||
                           sh.HospAddress.Contains(sSearchString) ||
                           sh.HospName.Contains(sSearchString)) 
                           orderby sh.HospID ascending select new
                           {
                               sh.HospRowid,
                               sh.HospID,
                               sh.HospName,
                               sh.HospAddress
                           }).ToDataSourceResult(request);

                return Json(result);
            }
            else
            {
                DataSourceResult result = (from sh in db_zmcms.SysHospital
                      where sh.HospActive == true && sh.HospID != "0000000000"
                      orderby sh.HospID ascending                     
                      select new
                      {
                              sh.HospRowid,
                              sh.HospID,
                              sh.HospName,
                              sh.HospAddress
                      }).ToDataSourceResult(request);

                return Json(result);
            }            
        }
        #endregion

        #region  HospitalLaboratory CRUD
        public ActionResult HospitalLaboratorySchedule_Read([DataSourceRequest] DataSourceRequest request, string sHospRowid)
        {
            DataSourceResult result =
                (from ls in db_zmlis.lisLaboratorySchedule where ls.HospRowid == sHospRowid select ls).ToDataSourceResult(request);

            return Json(result);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LISSchedule_Create([DataSourceRequest] DataSourceRequest request, lisLaboratorySchedule crud, string sHospRowid)
        {
            string sRowid = Guid.NewGuid().ToString();

            if (!String.IsNullOrWhiteSpace(sHospRowid) && ModelState.IsValid)
            {
                var entity = new lisLaboratorySchedule
                {
                    LLSRowid = sRowid,
                    HospRowid = sHospRowid,
                    CBDRowid = crud.CBDRowid,
                    LLSMon = crud.LLSMon,
                    LLSTue = crud.LLSTue,
                    LLSWed = crud.LLSWed,
                    LLSThu = crud.LLSThu,
                    LLSFri = crud.LLSFri,
                    LLSSat = crud.LLSSat,
                    LLSSun = crud.LLSSun,
                    LLSTime01 = crud.LLSTime01,
                    LLSTime02 = crud.LLSTime02,
                    LLSTime03 = crud.LLSTime03,
                    LLSTime04 = crud.LLSTime04,
                    LLSTime05 = crud.LLSTime05,
                    LLSDescription = crud.LLSDescription,
                    LLSReceiveMail = crud.LLSReceiveMail
                };

                db_zmlis.lisLaboratorySchedule.Add(entity);
                db_zmlis.SaveChanges();

                //insert_LS.LLSRowid = entity.LLSRowid;
            }

            return Json(new[] { crud }.ToDataSourceResult(new DataSourceRequest(), ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LISSchedule_Update([DataSourceRequest] DataSourceRequest request, lisLaboratorySchedule crud)
        {
            if (ModelState.IsValid)
            {
                var entity = new lisLaboratorySchedule
                {
                    LLSRowid = crud.LLSRowid,
                    HospRowid = crud.HospRowid,
                    CBDRowid = crud.CBDRowid,                    
                    LLSMon = crud.LLSMon,
                    LLSTue = crud.LLSTue,
                    LLSWed = crud.LLSWed,
                    LLSThu = crud.LLSThu,
                    LLSFri = crud.LLSFri,
                    LLSSat = crud.LLSSat,
                    LLSSun = crud.LLSSun,
                    LLSTime01 = crud.LLSTime01,
                    LLSTime02 = crud.LLSTime02,
                    LLSTime03 = crud.LLSTime03,
                    LLSTime04 = crud.LLSTime04,
                    LLSTime05 = crud.LLSTime05,
                    LLSDescription = crud.LLSDescription,
                    LLSReceiveMail = crud.LLSReceiveMail
                };

                db_zmlis.lisLaboratorySchedule.Attach(entity);
                db_zmlis.Entry(entity).State = EntityState.Modified;
                db_zmlis.SaveChanges();
            }

            return Json(new[] { crud }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LISSchedule_Destroy([DataSourceRequest] DataSourceRequest request, lisLaboratorySchedule crud)
        {
            if (ModelState.IsValid)
            {
                var entity = new lisLaboratorySchedule
                {
                    LLSRowid = crud.LLSRowid
                };

                db_zmlis.lisLaboratorySchedule.Attach(entity);
                db_zmlis.lisLaboratorySchedule.Remove(entity);
                db_zmlis.SaveChanges();
            }

            return Json(new[] { crud }.ToDataSourceResult(request, ModelState));
        }
        #endregion
    }
}