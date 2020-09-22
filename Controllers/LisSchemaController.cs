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

        #region LisLab CRUD        
        public ActionResult LisLab_Read([DataSourceRequest] DataSourceRequest request)
        {
            DataSourceResult result = (from ll in db_zmlis.LisLaboratory 
                                       select new { ll.LLRowid, ll.LLName, ll.LLFormat}).ToDataSourceResult(request);

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
                    LLFormat = crud.LLFormat
                };

                db_zmlis.LisLaboratory.Add(entity);
                db_zmlis.SaveChanges();
                crud.LLRowid = entity.LLRowid;
            }

            return Json(new[] { crud }.ToDataSourceResult(new DataSourceRequest(), ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LisLab_Update([DataSourceRequest] DataSourceRequest request, LisLaboratory_ViewModels crud)
        {
            if (crud != null) 
            {
                if (ModelState.IsValid)
                {
                    var entity = new LisLaboratory
                    {
                        LLRowid = crud.LLRowid,
                        LLName = crud.LLName,
                        LLFormat = crud.LLFormat
                    };

                    db_zmlis.LisLaboratory.Attach(entity);
                    db_zmlis.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                    db_zmlis.SaveChanges();
                }
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
        #endregion

        #region LisSchema CRUD        
        public ActionResult LisSchema_Read([DataSourceRequest] DataSourceRequest request, string sLLRowid)
        {
            DataSourceResult result = (from ll in db_zmlis.LisLaboratory_str
                                       where ll.LLRowid == sLLRowid
                                        select new 
                                        { 
                                            ll.SMRowid, 
                                            ll.LLRowid,
                                            ll.SMFieldName, 
                                            ll.SMFieldType, 
                                            ll.SMFieldLength, 
                                            ll.SMFieldKind
                                        }
                                       ).ToDataSourceResult(request);

            return Json(result);
        }
                
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LisSchema_Create([DataSourceRequest] DataSourceRequest request, LisLaboratory_ViewModels_str crud, string sLLRowid)
        {
            
            if (ModelState.IsValid)
            {
                var entity = new LisLaboratory_str
                {
                    SMRowid = Guid.NewGuid().ToString(),
                    LLRowid = sLLRowid,
                    SMFieldName = crud.SMFieldName,
                    SMFieldType = crud.SMFieldType,
                    SMFieldLength = crud.SMFieldLength,
                    SMFieldKind = crud.SMFieldKind
                };
                db_zmlis.LisLaboratory_str.Add(entity);
                db_zmlis.SaveChanges();
                //crud.SMRowid = entity.SMRowid;

            }
            return Json(new[] { crud }.ToDataSourceResult(new DataSourceRequest(), ModelState));

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LisSchema_Update([DataSourceRequest] DataSourceRequest request, LisLaboratory_ViewModels_str crud)
        {
            if (crud != null) 
            {
                if (ModelState.IsValid)
                {
                    var entity = new LisLaboratory_str
                    {
                        SMRowid = crud.SMRowid,
                        LLRowid = crud.LLRowid,
                        SMFieldName = crud.SMFieldName,
                        SMFieldType = crud.SMFieldType,
                        SMFieldLength = crud.SMFieldLength,
                        SMFieldKind = crud.SMFieldKind
                    };

                    db_zmlis.LisLaboratory_str.Attach(entity);
                    db_zmlis.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                    db_zmlis.SaveChanges();
                }           
            }
            return Json(new[] { crud }.ToDataSourceResult(request, ModelState));

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LisSchema_Destroy([DataSourceRequest] DataSourceRequest request, LisLaboratory_str crud)
        {
            if (ModelState.IsValid)
            {
                var entity = new LisLaboratory_str
                {
                    SMRowid = crud.SMRowid
                };

                db_zmlis.LisLaboratory_str.Attach(entity);
                db_zmlis.LisLaboratory_str.Remove(entity);
                db_zmlis.SaveChanges();
            }

            return Json(new[] { crud }.ToDataSourceResult(request, ModelState));
        }
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