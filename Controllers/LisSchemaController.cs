using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ZMLisSys.Models;
using ZMLisSys.ViewModels;

namespace ZMLisSys.Controllers
{
    public class LisSchemaController : Controller
    {
        private ZMLisSysEntities db_zmlis = new ZMLisSysEntities();
        private ZMCMSEntities db_zmcms = new ZMCMSEntities();

        // GET: LisSchema
        public ActionResult Index()
        {
            return View();
        }

        #region ACMTracer CRUD        
        public ActionResult LisLab_Read([DataSourceRequest] DataSourceRequest request)
        {
            DataSourceResult result = (from ll in db_zmlis.LisLaboratory select new { ll.LLRowid, ll.LLName }).ToDataSourceResult(request);

            return Json(result);
        }

        #region ACMTracer CRUD        
        public ActionResult LisLab_Read_str([DataSourceRequest] DataSourceRequest request)
        {
            DataSourceResult result = (from ll in db_zmlis.LisLaboratory_str select new { ll.Rowid, ll.Field_name, ll.Field_type, ll.Field_length }).ToDataSourceResult(request);

            return Json(result);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LisLab_Create([DataSourceRequest] DataSourceRequest request, LisLaboratory_ViewModels crud)
        {
            if (ModelState.IsValid)
            {
                var entity = new LisLaboratory
                {
                    LLRowid = Guid.NewGuid().ToString(),
                    LLName = crud.LLName,
                    F_format = crud.F_format
                };

                db_zmlis.LisLaboratory.Add(entity);
                db_zmlis.SaveChanges();
                crud.LLRowid = entity.LLRowid;
            }

            return Json(new[] { crud }.ToDataSourceResult(new DataSourceRequest(), ModelState));
        }
        //public ActionResult ACMTrace_Create(ViewModel_ACMTrace insertData)
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LisLab_Create_str([DataSourceRequest] DataSourceRequest request, LisLaboratory_ViewModels_str crud)
        {
            if (ModelState.IsValid)
            {  
                var entity = new LisLaboratory_str
                {
                    Rowid = Guid.NewGuid().ToString(),
                    LLRowid = Guid.NewGuid().ToString(),
                    Field_name = crud.Field_name,
                    Field_type = crud.Field_type,
                    Field_length = crud.Field_length,
                    Field_HB = crud.Field_HB
                };

                db_zmlis.LisLaboratory_str.Add(entity);
                db_zmlis.SaveChanges();
                crud.Rowid = entity.Rowid;

            }
                        
            return Json(new[] { crud }.ToDataSourceResult(new DataSourceRequest(), ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LisLab_Update([DataSourceRequest] DataSourceRequest request, LisLaboratory_ViewModels crud)
        {
            if (ModelState.IsValid)
            {                
                var entity = new LisLaboratory
                {
                    LLRowid = crud.LLRowid,
                    LLName = crud.LLName,
                    F_format = crud.F_format
                };

                db_zmlis.LisLaboratory.Attach(entity);
                db_zmlis.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                db_zmlis.SaveChanges();
            }

            return Json(new[] { crud }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LisLab_Update_str([DataSourceRequest] DataSourceRequest request, LisLaboratory_ViewModels_str crud)
        {
            if (ModelState.IsValid)
            {
                var entity = new LisLaboratory_str
                {
                    Rowid = crud.Rowid,
                    LLRowid = crud.LLRowid,
                    Field_name = crud.Field_name,
                    Field_type = crud.Field_type,
                    Field_length = crud.Field_length,
                    Field_HB = crud.Field_HB
                };

                db_zmlis.LisLaboratory_str.Attach(entity);
                db_zmlis.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                db_zmlis.SaveChanges();
            }

            return Json(new[] { crud }.ToDataSourceResult(request, ModelState));
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LisLab_Destroy([DataSourceRequest] DataSourceRequest request, LisLaboratory crud)
        {
            if (ModelState.IsValid)
            {
                var entity = new LisLaboratory
                {
                    LLRowid = crud.LLRowid
                };

                db_zmlis.LisLaboratory.Attach(entity);
                db_zmlis.LisLaboratory.Remove(entity);
                db_zmlis.SaveChanges();
            }

            return Json(new[] { crud }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LisLab_Destroy_str([DataSourceRequest] DataSourceRequest request, LisLaboratory_str crud)
        {
            if (ModelState.IsValid)
            {
                var entity = new LisLaboratory_str
                {
                    Rowid = crud.Rowid
                };

                db_zmlis.LisLaboratory_str.Attach(entity);
                db_zmlis.LisLaboratory_str.Remove(entity);
                db_zmlis.SaveChanges();
            }

            return Json(new[] { crud }.ToDataSourceResult(request, ModelState));
        }

        #endregion
        #endregion

        #region 取得參數設定資料
        public JsonResult GetCombo(string stext)
        {

            var result = (from cm in db_zmcms.ComboMaster
                          where cm.CBMClass == stext
                          join cd in db_zmcms.ComboDetail on cm.CBMRowid equals cd.CBMRowid
                          where cd.CBDDisplayFlag == true
                          orderby cd.CBDDisplayOrder
                          select new
                          {
                              CBDRowid = cd.CBDRowid,
                              CBDCode = cd.CBDCode,
                              CBDDescription = cd.CBDDescription,
                              CBDDisplayFlag = cd.CBDDisplayFlag,
                              CBDDisplayOrder = cd.CBDDisplayOrder
                          });

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }

}