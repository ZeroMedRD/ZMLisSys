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

namespace ZMLISSys.Controllers
{
    public class LaboratoryBaseController : Controller
    {
        private LIS_AntifatEntities db_antifat = new LIS_AntifatEntities();
        private ZMLISEntities db_zmlis = new ZMLISEntities();
        private LIS_ZMCMSv2Entities db_zmcms = new LIS_ZMCMSv2Entities();

        // GET: LaboratoryItem
        public ActionResult LBIndexV1()
        {
            return View();
        }

        public ActionResult LBIndexV2()
        {
            return View();
        }

        #region LaboratoryClass CRUD
        public ActionResult LaboratoryClass_Read([DataSourceRequest] DataSourceRequest request)
        {
            DataSourceResult result = (from lc in db_antifat.laboratory_class
                                       select new
                                       {
                                           id = lc.id,
                                           code = lc.code,
                                           nick = lc.nick,
                                           name = lc.name
                                       }
                                       ).ToDataSourceResult(request);

            return Json(result);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LaboratoryClass_Create([DataSourceRequest] DataSourceRequest request, ViewModel_LaboratoryClass crud)
        {
            if (ModelState.IsValid)
            {
                var entity = new laboratory_class
                {
                    id = Guid.NewGuid().ToString(),
                    code = crud.code,
                    nick = crud.nick,
                    name = crud.name
                };

                db_antifat.laboratory_class.Add(entity);
                db_antifat.SaveChanges();
                crud.id = entity.id;
            }

            return Json(new[] { crud }.ToDataSourceResult(new DataSourceRequest(), ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LaboratoryClass_Update([DataSourceRequest] DataSourceRequest request, ViewModel_LaboratoryClass crud)
        {
            if (ModelState.IsValid)
            {
                var entity = new laboratory_class
                {
                    id = crud.id,
                    code = crud.code,
                    nick = crud.nick,
                    name = crud.name
                };

                db_antifat.laboratory_class.Attach(entity);
                db_antifat.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                db_antifat.SaveChanges();
            }

            return Json(new[] { crud }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LaboratoryClass_Destory([DataSourceRequest] DataSourceRequest request, laboratory_class crud)
        {
            if (ModelState.IsValid)
            {
                var entity = new laboratory_class
                {
                    id = crud.id
                };

                db_antifat.laboratory_class.Attach(entity);
                db_antifat.laboratory_class.Remove(entity);
                db_antifat.SaveChanges();
            }

            return Json(new[] { crud }.ToDataSourceResult(request, ModelState));
        }
        #endregion

        #region LaboratoryItem CRUD
        public ActionResult LaboratoryItem_Read([DataSourceRequest] DataSourceRequest request)
        {
            DataSourceResult result = (from li in db_antifat.laboratory_item
                                       join lc in db_antifat.laboratory_class on li.laboratoryClass_id equals lc.id into ps
                                       from lc in ps.DefaultIfEmpty()
                                       join lcc in db_antifat.laboratory_clinic on li.laboratory_clinic_code equals lcc.code into ps1
                                       from lcc in ps1.DefaultIfEmpty()
                                       select new
                                       {
                                           id = li.id,
                                           laboratoryClass_id = li.laboratoryClass_id,
                                           laboratoryClass_name = lc.name,
                                           code = li.code,
                                           nick = li.nick,
                                           name = li.name,
                                           chnName = li.chnName,
                                           unit = li.unit,
                                           standard = li.standard,
                                           nhi_code = li.nhi_code,
                                           laboratory_clinic_code = li.laboratory_clinic_code,
                                           laboratory_clinic_name = lcc.name
                                       }
                                       ).ToDataSourceResult(request);

            return Json(result);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LaboratoryItem_Create([DataSourceRequest] DataSourceRequest request, ViewModel_LaboratoryItem crud, string slaboratoryClass_id)
        {
            if (ModelState.IsValid)
            {
                var entity = new laboratory_item
                {
                    id = Guid.NewGuid().ToString(),
                    laboratoryClass_id = slaboratoryClass_id,
                    code = crud.code,
                    nick = crud.nick,
                    name = crud.name,
                    chnName = crud.chnName,
                    unit = crud.unit,
                    standard = crud.standard,
                    nhi_code = crud.nhi_code,
                    laboratory_clinic_code = crud.laboratory_clinic_code
                };

                db_antifat.laboratory_item.Add(entity);
                db_antifat.SaveChanges();
                crud.id = entity.id;
            }

            return Json(new[] { crud }.ToDataSourceResult(new DataSourceRequest(), ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LaboratoryItem_Update([DataSourceRequest] DataSourceRequest request, ViewModel_LaboratoryItem crud)
        {
            if (ModelState.IsValid)
            {
                var entity = new laboratory_item
                {
                    id = crud.id,
                    laboratoryClass_id = crud.laboratoryClass_id,
                    code = crud.code,
                    nick = crud.nick,
                    name = crud.name,
                    chnName = crud.chnName,
                    unit = crud.unit,
                    standard = crud.standard,
                    nhi_code = crud.nhi_code,
                    laboratory_clinic_code = crud.laboratory_clinic_code
                };

                db_antifat.laboratory_item.Attach(entity);
                db_antifat.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                db_antifat.SaveChanges();
            }

            return Json(new[] { crud }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LaboratoryItem_Destory([DataSourceRequest] DataSourceRequest request, laboratory_item crud)
        {
            if (ModelState.IsValid)
            {
                var entity = new laboratory_item
                {
                    id = crud.id
                };

                db_antifat.laboratory_item.Attach(entity);
                db_antifat.laboratory_item.Remove(entity);
                db_antifat.SaveChanges();
            }

            return Json(new[] { crud }.ToDataSourceResult(request, ModelState));
        }
        #endregion

        #region LisLaboratoryClass CRUD
        public ActionResult LisLaboratoryClass_Read([DataSourceRequest] DataSourceRequest request)
        {
            DataSourceResult result = (from llc in db_zmlis.lisLaboratoryClass
                                       select new
                                       {
                                           LLCRowid = llc.LLCRowid,
                                           LLCCode = llc.LLCCode,
                                           LLCTrdCName = llc.LLCTrdCName,
                                           LLCEngName = llc.LLCEngName
                                       }).ToDataSourceResult(request);

            return Json(result);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LisLaboratoryClass_Create([DataSourceRequest] DataSourceRequest request, ViewModel_LisLaboratoryClass crud)
        {
            if (ModelState.IsValid)
            {
                var entity = new lisLaboratoryClass
                {
                    LLCRowid = Guid.NewGuid().ToString(),
                    LLCCode = crud.LLCCode,
                    LLCTrdCName = crud.LLCTrdCName,
                    LLCEngName = crud.LLCEngName
                };

                db_zmlis.lisLaboratoryClass.Add(entity);
                db_zmlis.SaveChanges();
                crud.LLCRowid = entity.LLCRowid;
            }

            return Json(new[] { crud }.ToDataSourceResult(new DataSourceRequest(), ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LisLaboratoryClass_Update([DataSourceRequest] DataSourceRequest request, ViewModel_LisLaboratoryClass crud)
        {
            if (ModelState.IsValid)
            {
                var entity = new lisLaboratoryClass
                {
                    LLCRowid = crud.LLCRowid,
                    LLCCode = crud.LLCCode,
                    LLCTrdCName = crud.LLCTrdCName,
                    LLCEngName = crud.LLCEngName                    
                };

                db_zmlis.lisLaboratoryClass.Attach(entity);
                db_zmlis.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                db_zmlis.SaveChanges();
            }

            return Json(new[] { crud }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LisLaboratoryClass_Destory([DataSourceRequest] DataSourceRequest request, lisLaboratoryClass crud)
        {
            if (ModelState.IsValid)
            {
                var entity = new lisLaboratoryClass
                {
                    LLCRowid = crud.LLCRowid
                };

                db_zmlis.lisLaboratoryClass.Attach(entity);
                db_zmlis.lisLaboratoryClass.Remove(entity);
                db_zmlis.SaveChanges();
            }

            return Json(new[] { crud }.ToDataSourceResult(request, ModelState));
        }
        #endregion

        #region LisLaboratoryItem CRUD
        public ActionResult LisLaboratoryItem_Read([DataSourceRequest] DataSourceRequest request, string sLLCRowid)
        {
            var db_lli = (from lli in db_zmlis.lisLaboratoryItem where lli.LLCRowid == sLLCRowid select lli).ToList();

            var db_LLICostType = (from cm in db_zmcms.ComboMaster where cm.CBMClass == "LLICostType" 
                                  join cd in db_zmcms.ComboDetail on cm.CBMRowid equals cd.CBMRowid
                                  orderby cd.CBDDisplayOrder
                                  select new { cd.CBDRowid, cd.CBDDescription }).ToList();

            var db_LLIType = (from cm in db_zmcms.ComboMaster
                                  where cm.CBMClass == "LLIType"
                                  join cd in db_zmcms.ComboDetail on cm.CBMRowid equals cd.CBMRowid
                                  orderby cd.CBDDisplayOrder
                                  select new { cd.CBDRowid, cd.CBDDescription }).ToList();

            var db_LLIKind = (from cm in db_zmcms.ComboMaster
                              where cm.CBMClass == "LLIKind"
                              join cd in db_zmcms.ComboDetail on cm.CBMRowid equals cd.CBMRowid
                              orderby cd.CBDDisplayOrder
                              select new { cd.CBDRowid, cd.CBDDescription }).ToList();

            DataSourceResult result = (from lli in db_lli
                                       join t1 in db_LLICostType on lli.LLICostType equals t1.CBDRowid into ps1
                                       from rs1 in ps1.DefaultIfEmpty()
                                       join t2 in db_LLIType on lli.LLIType equals t2.CBDRowid into ps2
                                       from rs2 in ps2.DefaultIfEmpty()
                                       join t3 in db_LLIKind on lli.LLIKind equals t3.CBDRowid into ps3
                                       from rs3 in ps3.DefaultIfEmpty()
                                       orderby lli.LLINhiCode
                                       select new ViewModel_LisLaboratoryItem()
                                       {
                                           LLIRowid = lli.LLIRowid,
                                           LLCRowid = lli.LLCRowid,
                                           LLINhiCode = lli.LLINhiCode,
                                           LLITrdCName = lli.LLITrdCName,
                                           LLIEngName = lli.LLIEngName,
                                           LLIRptName = lli.LLIRptName,
                                           LLINhiCost = lli.LLINhiCost == null ? 0 : (float)lli.LLINhiCost,
                                           LLICostType = lli.LLICostType,
                                           LLICostTypeName = (rs1 == null || rs1.CBDRowid == null ? "N/A" : rs1.CBDDescription),
                                           LLIType = lli.LLIType,
                                           LLITypeName = (rs2 == null || rs2.CBDRowid == null ? "N/A" : rs2.CBDDescription),
                                           LLIUnit = lli.LLIUnit,
                                           LLIKind = lli.LLIKind,
                                           LLIKindName = (rs3 == null || rs3.CBDRowid == null ? "N/A" : rs3.CBDDescription),
                                           LLIUp_Male = lli.LLIUp_Male == null ? 0 : (float)lli.LLIUp_Male,
                                           LLILo_Male = lli.LLILo_Male == null ? 0 : (float)lli.LLILo_Male,
                                           LLIUp_Female = lli.LLIUp_Female == null ? 0 : (float)lli.LLIUp_Female,
                                           LLILo_Female = lli.LLILo_Female == null ? 0 : (float)lli.LLILo_Female,
                                           LLIConvertRate = lli.LLIConvertRate,
                                           LLIConvertUnit = lli.LLIConvertUnit
                                       }).ToDataSourceResult(request);

            return Json(result);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LisLaboratoryItem_Create([DataSourceRequest] DataSourceRequest request, ViewModel_LisLaboratoryItem crud, string sLLCRowid)
        {
            if (ModelState.IsValid)
            {
                var entity = new lisLaboratoryItem
                {
                    LLIRowid = Guid.NewGuid().ToString(),
                    LLCRowid = sLLCRowid,
                    LLINhiCode = crud.LLINhiCode,
                    LLITrdCName = crud.LLITrdCName,
                    LLIEngName = crud.LLIEngName,
                    LLIRptName = crud.LLIRptName,
                    LLINhiCost = crud.LLINhiCost,
                    LLICostType = crud.LLICostType,
                    LLIType = crud.LLIType,
                    LLIUnit = crud.LLIUnit,
                    LLIKind = crud.LLIKind,
                    LLIUp_Male = crud.LLIUp_Male,
                    LLILo_Male = crud.LLILo_Male,
                    LLIUp_Female = crud.LLIUp_Female,
                    LLILo_Female = crud.LLILo_Female,
                    LLIConvertRate = crud.LLIConvertRate,
                    LLIConvertUnit = crud.LLIConvertUnit
                };

                db_zmlis.lisLaboratoryItem.Add(entity);
                db_zmlis.SaveChanges();                
            }

            return Json(new[] { crud }.ToDataSourceResult(new DataSourceRequest(), ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LisLaboratoryItem_Update([DataSourceRequest] DataSourceRequest request, ViewModel_LisLaboratoryItem crud)
        {
            if (ModelState.IsValid)
            {
                var entity = new lisLaboratoryItem
                {
                    LLIRowid = crud.LLIRowid,
                    LLCRowid = crud.LLCRowid,
                    LLINhiCode = crud.LLINhiCode,
                    LLITrdCName = crud.LLITrdCName,
                    LLIEngName = crud.LLIEngName,
                    LLIRptName = crud.LLIRptName,
                    LLINhiCost = crud.LLINhiCost,
                    LLICostType = crud.LLICostType,
                    LLIType = crud.LLIType,
                    LLIUnit = crud.LLIUnit,
                    LLIKind = crud.LLIKind,
                    LLIUp_Male = crud.LLIUp_Male,
                    LLILo_Male = crud.LLILo_Male,
                    LLIUp_Female = crud.LLIUp_Female,
                    LLILo_Female = crud.LLILo_Female,
                    LLIConvertRate = crud.LLIConvertRate,
                    LLIConvertUnit = crud.LLIConvertUnit
                };

                db_zmlis.lisLaboratoryItem.Attach(entity);
                db_zmlis.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                db_zmlis.SaveChanges();
            }

            return Json(new[] { crud }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LisLaboratoryItem_Destory([DataSourceRequest] DataSourceRequest request, lisLaboratoryItem crud)
        {
            if (ModelState.IsValid)
            {
                var entity = new lisLaboratoryItem
                {
                    LLIRowid = crud.LLIRowid
                };

                db_zmlis.lisLaboratoryItem.Attach(entity);
                db_zmlis.lisLaboratoryItem.Remove(entity);
                db_zmlis.SaveChanges();
            }

            return Json(new[] { crud }.ToDataSourceResult(request, ModelState));
        }
        #endregion

        #region LisLaboratoryItemAll CRUD
        public ActionResult LisLaboratoryItemAll_Read([DataSourceRequest] DataSourceRequest request)
        {
            var db_lli = (from lli in db_zmlis.lisLaboratoryItem select lli).ToList();            

            var db_LLICostType = (from cm in db_zmcms.ComboMaster
                                  where cm.CBMClass == "LLICostType"
                                  join cd in db_zmcms.ComboDetail on cm.CBMRowid equals cd.CBMRowid
                                  orderby cd.CBDDisplayOrder
                                  select new { cd.CBDRowid, cd.CBDDescription }).ToList();

            var db_LLIType = (from cm in db_zmcms.ComboMaster
                              where cm.CBMClass == "LLIType"
                              join cd in db_zmcms.ComboDetail on cm.CBMRowid equals cd.CBMRowid
                              orderby cd.CBDDisplayOrder
                              select new { cd.CBDRowid, cd.CBDDescription }).ToList();

            var db_LLIKind = (from cm in db_zmcms.ComboMaster
                              where cm.CBMClass == "LLIKind"
                              join cd in db_zmcms.ComboDetail on cm.CBMRowid equals cd.CBMRowid
                              orderby cd.CBDDisplayOrder
                              select new { cd.CBDRowid, cd.CBDDescription }).ToList();

            DataSourceResult result = (from lli in db_lli
                                       join t1 in db_LLICostType on lli.LLICostType equals t1.CBDRowid into ps1
                                       from rs1 in ps1.DefaultIfEmpty()
                                       join t2 in db_LLIType on lli.LLIType equals t2.CBDRowid into ps2
                                       from rs2 in ps2.DefaultIfEmpty()
                                       join t3 in db_LLIKind on lli.LLIKind equals t3.CBDRowid into ps3
                                       from rs3 in ps3.DefaultIfEmpty()
                                       orderby lli.LLINhiCode
                                       select new ViewModel_LisLaboratoryItem()
                                       {
                                           LLIRowid = lli.LLIRowid,
                                           LLCRowid = lli.LLCRowid,
                                           LLINhiCode = lli.LLINhiCode,
                                           LLITrdCName = lli.LLITrdCName,
                                           LLIEngName = lli.LLIEngName,
                                           LLIRptName = lli.LLIRptName,
                                           LLINhiCost = lli.LLINhiCost == null ? 0 : (float)lli.LLINhiCost,
                                           LLICostType = lli.LLICostType,
                                           LLICostTypeName = (rs1 == null || rs1.CBDRowid == null ? "N/A" : rs1.CBDDescription),
                                           LLIType = lli.LLIType,
                                           LLITypeName = (rs2 == null || rs2.CBDRowid == null ? "N/A" : rs2.CBDDescription),                                           
                                           LLIUnit = lli.LLIUnit,
                                           LLIKind = lli.LLIKind,
                                           LLIKindName = (rs3 == null || rs3.CBDRowid == null ? "N/A" : rs3.CBDDescription),
                                           LLIUp_Male = lli.LLIUp_Male == null ? 0 : (float)lli.LLIUp_Male,
                                           LLILo_Male = lli.LLILo_Male == null ? 0 : (float)lli.LLILo_Male,
                                           LLIUp_Female = lli.LLIUp_Female == null ? 0 : (float)lli.LLIUp_Female,
                                           LLILo_Female = lli.LLILo_Female == null ? 0 : (float)lli.LLILo_Female,
                                           LLIConvertRate = lli.LLIConvertRate,
                                           LLIConvertUnit = lli.LLIConvertUnit
                                       }).ToDataSourceResult(request);

            return Json(result);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LisLaboratoryItemAll_Create([DataSourceRequest] DataSourceRequest request, ViewModel_LisLaboratoryItem crud)
        {
            if (ModelState.IsValid)
            {
                var entity = new lisLaboratoryItem
                {                    
                    LLIRowid = Guid.NewGuid().ToString(),
                    LLCRowid = crud.LLCRowid,
                    LLINhiCode = crud.LLINhiCode,
                    LLITrdCName = crud.LLITrdCName,
                    LLIEngName = crud.LLIEngName,
                    LLIRptName = crud.LLIRptName,
                    LLINhiCost = crud.LLINhiCost,
                    LLICostType = crud.LLICostType,
                    LLIType = crud.LLIType,
                    LLIUnit = crud.LLIUnit,
                    LLIKind = crud.LLIKind,
                    LLIUp_Male = crud.LLIUp_Male,
                    LLILo_Male = crud.LLILo_Male,
                    LLIUp_Female = crud.LLIUp_Female,
                    LLILo_Female = crud.LLILo_Female,
                    LLIConvertRate = crud.LLIConvertRate,
                    LLIConvertUnit = crud.LLIConvertUnit
                };

                db_zmlis.lisLaboratoryItem.Add(entity);
                db_zmlis.SaveChanges();
            }

            return Json(new[] { crud }.ToDataSourceResult(new DataSourceRequest(), ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LisLaboratoryItemAll_Update([DataSourceRequest] DataSourceRequest request, ViewModel_LisLaboratoryItem crud)
        {
            if (ModelState.IsValid)
            {
                var entity = new lisLaboratoryItem
                {
                    LLIRowid = crud.LLIRowid,
                    LLCRowid = crud.LLCRowid,
                    LLINhiCode = crud.LLINhiCode,
                    LLITrdCName = crud.LLITrdCName,
                    LLIEngName = crud.LLIEngName,
                    LLIRptName = crud.LLIRptName,
                    LLINhiCost = crud.LLINhiCost,
                    LLICostType = crud.LLICostType,
                    LLIType = crud.LLIType,
                    LLIUnit = crud.LLIUnit,
                    LLIKind = crud.LLIKind,
                    LLIUp_Male = crud.LLIUp_Male,
                    LLILo_Male = crud.LLILo_Male,
                    LLIUp_Female = crud.LLIUp_Female,
                    LLILo_Female = crud.LLILo_Female,
                    LLIConvertRate = crud.LLIConvertRate,
                    LLIConvertUnit = crud.LLIConvertUnit
                };

                db_zmlis.lisLaboratoryItem.Attach(entity);
                db_zmlis.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                db_zmlis.SaveChanges();
            }

            return Json(new[] { crud }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LisLaboratoryItemAll_Destory([DataSourceRequest] DataSourceRequest request, lisLaboratoryItem crud)
        {
            if (ModelState.IsValid)
            {
                var entity = new lisLaboratoryItem
                {
                    LLIRowid = crud.LLIRowid
                };

                db_zmlis.lisLaboratoryItem.Attach(entity);
                db_zmlis.lisLaboratoryItem.Remove(entity);
                db_zmlis.SaveChanges();
            }

            return Json(new[] { crud }.ToDataSourceResult(request, ModelState));
        }
        #endregion
    }
}