using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ZMLISSys.Models;
using ZMLISSys.ViewModels;
//using System.IO;
//using Excel = Microsoft.Office.Interop.Excel;
//using ClosedXML.Excel;

namespace ZMLISSys.Controllers
{
    public class LISSchemaController : Controller
    {
        private ZMLISEntities db_zmlis = new ZMLISEntities();
        private LIS_ZMCMSv2Entities db_zmcms = new LIS_ZMCMSv2Entities();

        // GET: LisSchema
        public ActionResult LSIndex()
        {
            return View();
        }

        public ActionResult Success()
        {
            return View();
        }
       
        #region LisLaboratoryMaster CRUD        
        public ActionResult LisLaboratoryMaster_Read([DataSourceRequest] DataSourceRequest request)
        {
            DataSourceResult result = (from ll in db_zmlis.lisLaboratoryMaster
                                       select new { ll.LLMRowid, ll.LLMName, ll.LLMFormat}).ToDataSourceResult(request);

            return Json(result);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LisLaboratoryMaster_Create([DataSourceRequest] DataSourceRequest request, ViewModel_LisLaboratoryMaster crud)
        {
            if (ModelState.IsValid)
            {
                var entity = new lisLaboratoryMaster
                {
                    LLMRowid = Guid.NewGuid().ToString(),
                    LLMName = crud.LLMName,
                    LLMFormat = crud.LLMFormat
                };

                db_zmlis.lisLaboratoryMaster.Add(entity);
                db_zmlis.SaveChanges();
                crud.LLMRowid = entity.LLMRowid;
            }

            return Json(new[] { crud }.ToDataSourceResult(new DataSourceRequest(), ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LisLaboratoryMaster_Update([DataSourceRequest] DataSourceRequest request, ViewModel_LisLaboratoryMaster crud)
        {
            if (crud != null) 
            {
                if (ModelState.IsValid)
                {
                    var entity = new lisLaboratoryMaster
                    {
                        LLMRowid = crud.LLMRowid,
                        LLMName = crud.LLMName,
                        LLMFormat = crud.LLMFormat
                    };

                    db_zmlis.lisLaboratoryMaster.Attach(entity);
                    db_zmlis.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                    db_zmlis.SaveChanges();
                }
            }
            

            return Json(new[] { crud }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LisLaboratoryMaster_Destroy([DataSourceRequest] DataSourceRequest request, lisLaboratoryMaster crud)
        {
            if (ModelState.IsValid)
            {
                var entity = new lisLaboratoryMaster
                {
                    LLMRowid = crud.LLMRowid
                };

                db_zmlis.lisLaboratoryMaster.Attach(entity);
                db_zmlis.lisLaboratoryMaster.Remove(entity);
                db_zmlis.SaveChanges();
            }

            return Json(new[] { crud }.ToDataSourceResult(request, ModelState));
        }
        #endregion

        #region LisLaboratoryDetail CRUD        
        public ActionResult LisLaboratoryDetail_Read([DataSourceRequest] DataSourceRequest request, string sLLMRowid)
        {
            var db_LisLaboratoryDetail = (from ll in db_zmlis.lisLaboratoryDetail where ll.LLMRowid == sLLMRowid select ll).ToList();

            var db_LLDFieldType = (from cm in db_zmcms.ComboMaster
                                    where cm.CBMClass == "ZMLisSys_LLDFieldType"
                                    join cd in db_zmcms.ComboDetail on cm.CBMRowid equals cd.CBMRowid
                                    orderby cd.CBDDisplayOrder
                                    select new { cd.CBDRowid, cd.CBDCode, cd.CBDDescription }).ToList();

            var db_LLDFieldKind = (from cm in db_zmcms.ComboMaster
                                    where cm.CBMClass == "ZMLisSys_LLDFieldKind"
                                    join cd in db_zmcms.ComboDetail on cm.CBMRowid equals cd.CBMRowid
                                    orderby cd.CBDDisplayOrder
                                    select new { cd.CBDRowid, cd.CBDCode, cd.CBDDescription }).ToList();

            var db_LLDMappingField = (from cm in db_zmcms.ComboMaster
                                      where cm.CBMClass == "ZMLisSys_LLDMappingField"
                                      join cd in db_zmcms.ComboDetail on cm.CBMRowid equals cd.CBMRowid
                                      orderby cd.CBDDisplayOrder
                                      select new { cd.CBDRowid, cd.CBDCode, cd.CBDDescription }).ToList();


            //var LLDFieldType = (from cm in db_zmcms.ComboMaster where cm.CBMClass == "ZMLisSys_LLDFieldType" join cd in db_zmcms.ComboDetail on cm.CBMRowid equals cd.CBMRowid select new { cd.CBDCode, cd.CBDDescription }).ToList();
            //var db_LLDFieldKind = (from cm in db_zmcms.ComboMaster where cm.CBMClass == "ZMLisSys_LLDFieldKind" join cd in db_zmcms.ComboDetail on cm.CBMRowid equals cd.CBMRowid select new { cd.CBDCode, cd.CBDDescription }).ToList();
            //var db_LisLaboratoryDetail = (from ll in db_zmlis.LisLaboratoryDetail where ll.LLMRowid == sLLMRowid select ll).ToList();

            DataSourceResult result = (from ll in db_LisLaboratoryDetail
                                       join t1 in db_LLDFieldType on ll.LLDFieldType equals t1.CBDRowid into ps1
                                       from rs1 in ps1.DefaultIfEmpty()
                                       join t2 in db_LLDFieldKind on ll.LLDFieldKind equals t2.CBDCode into ps2
                                       from rs2 in ps2.DefaultIfEmpty()
                                       join t3 in db_LLDMappingField on ll.LLDMappingField equals t3.CBDRowid into ps3
                                       from rs3 in ps3.DefaultIfEmpty()
                                       orderby ll.LLDSeqno ascending
                                       select new
                                       {
                                           ll.LLDRowid,
                                           ll.LLMRowid,
                                           ll.LLDCode,
                                           ll.LLDFieldName,
                                           ll.LLDFieldType,
                                           LLDFieldTypeName = (rs1 == null || rs1.CBDRowid == null ? "N/A" : rs1.CBDDescription),
                                           ll.LLDFieldLength,
                                           ll.LLDFieldFloatLength,
                                           ll.LLDFieldKind,
                                           LLDFieldKindName = (rs2 == null || rs2.CBDRowid == null ? "N/A" : rs2.CBDDescription),
                                           ll.LLDFieldMemo,
                                           ll.LLDTextStartPos,
                                           ll.LLDTextEndPos,
                                           ll.LLDSeqno,
                                           ll.LLDMappingField,
                                           LLDMappingFieldName = (rs3 == null || rs3.CBDRowid == null ? "N/A" : rs3.CBDDescription),
                                       }).ToDataSourceResult(request);

            return Json(result);
        }
                
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LisLaboratoryDetail_Create([DataSourceRequest] DataSourceRequest request, ViewModel_LisLaboratoryDetail crud, string sLLMRowid)
        {
            
            if (ModelState.IsValid)
            {
                var entity = new lisLaboratoryDetail
                {
                    LLDRowid = Guid.NewGuid().ToString(),
                    LLMRowid = sLLMRowid,
                    LLDCode = crud.LLDCode,
                    LLDFieldName = crud.LLDFieldName,
                    LLDFieldType = crud.LLDFieldType,
                    LLDFieldLength = crud.LLDFieldLength,
                    LLDFieldFloatLength = crud.LLDFieldFloatLength,
                    LLDFieldKind = crud.LLDFieldKind,
                    LLDFieldMemo = crud.LLDFieldMemo,
                    LLDTextStartPos = crud.LLDTextStartPos,
                    LLDTextEndPos = crud.LLDTextEndPos,
                    LLDSeqno = crud.LLDSeqno,
                    LLDMappingField = crud.LLDMappingField
                };
                db_zmlis.lisLaboratoryDetail.Add(entity);
                db_zmlis.SaveChanges();
                //crud.SMRowid = entity.SMRowid;

            }
            return Json(new[] { crud }.ToDataSourceResult(new DataSourceRequest(), ModelState));

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LisLaboratoryDetail_Update([DataSourceRequest] DataSourceRequest request, ViewModel_LisLaboratoryDetail crud)
        {
            if (crud != null) 
            {
                if (ModelState.IsValid)
                {
                    var entity = new lisLaboratoryDetail
                    {
                        LLDRowid = crud.LLDRowid,
                        LLMRowid = crud.LLMRowid,
                        LLDCode = crud.LLDCode,
                        LLDFieldName = crud.LLDFieldName,
                        LLDFieldType = crud.LLDFieldType,
                        LLDFieldLength = crud.LLDFieldLength,
                        LLDFieldFloatLength = crud.LLDFieldFloatLength,
                        LLDFieldKind = crud.LLDFieldKind,
                        LLDFieldMemo = crud.LLDFieldMemo,
                        LLDTextStartPos = crud.LLDTextStartPos,
                        LLDTextEndPos = crud.LLDTextEndPos,
                        LLDSeqno = crud.LLDSeqno,
                        LLDMappingField = crud.LLDMappingField
                    };

                    db_zmlis.lisLaboratoryDetail.Attach(entity);
                    db_zmlis.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                    db_zmlis.SaveChanges();
                }
            }
            return Json(new[] { crud }.ToDataSourceResult(request, ModelState));

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LisLaboratoryDetail_Destroy([DataSourceRequest] DataSourceRequest request, lisLaboratoryDetail crud)
        {
            if (ModelState.IsValid)
            {
                var entity = new lisLaboratoryDetail
                {
                    LLDRowid = crud.LLDRowid
                };

                db_zmlis.lisLaboratoryDetail.Attach(entity);
                db_zmlis.lisLaboratoryDetail.Remove(entity);
                db_zmlis.SaveChanges();
            }

            return Json(new[] { crud }.ToDataSourceResult(request, ModelState));
        }
        #endregion

        #region GetLaboratorySchema_取得檢驗所定義的欄位清單資料
        public ActionResult GetLaboratorySchema(string sLLMRowid)
        {
            if (sLLMRowid != null)
            {
                var db_temp = from ll in db_zmlis.lisLaboratoryDetail
                              where ll.LLMRowid == sLLMRowid
                              join ldm in db_zmlis.lisDataMapping on ll.LLDRowid equals ldm.LLDRowid into ps
                              from ldm in ps.DefaultIfEmpty()
                              where ldm.LLDRowid == null
                              orderby ll.LLDSeqno
                              select new
                              {
                                  ll.LLDRowid,
                                  ll.LLDCode,                                  
                                  ll.LLDFieldName,
                                  ll.LLDSeqno
                              };

                return Content(JsonConvert.SerializeObject(db_temp), "application/json");
            }

            return Content("");
        }
        #endregion

        #region GetSelectedLaboratorySchema_取得檢驗所定義的欄位清單資料
        public ActionResult GetSelectedLaboratorySchema(string sLLMRowid)
        {
            if (sLLMRowid != null)
            {
                var db_temp = (from ll in db_zmlis.lisLaboratoryDetail where ll.LLMRowid == sLLMRowid
                               join ldm in db_zmlis.lisDataMapping on ll.LLDRowid equals ldm.LLDRowid                               
                               orderby ldm.LDMSeqno
                               select new
                               {
                                   ldm.LDMRowid,
                                   ldm.LLDRowid,
                                   ll.LLDFieldName,
                                   ll.LLDSeqno
                               });

                return Content(JsonConvert.SerializeObject(db_temp), "application/json");
            }

            return Content("");
        }
        #endregion
    }
}