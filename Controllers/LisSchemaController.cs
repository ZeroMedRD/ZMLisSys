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
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using ClosedXML.Excel;

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

        public ActionResult Success()
        {
            return View();
        }

        public ActionResult LisLabStr()
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
                                            ll.SMFieldKind,
                                            ll.SMFieldMeno
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
                    SMFieldKind = crud.SMFieldKind,
                    SMFieldMeno = crud.SMFieldMeno
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
                        SMFieldKind = crud.SMFieldKind,
                        SMFieldMeno = crud.SMFieldMeno
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

        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult LisSchema_import([DataSourceRequest] DataSourceRequest request, LisLaboratory_ViewModels_item crud, string sLLRowid)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        var entity = new LisLaboratory_item
        //        {
        //            id = Guid.NewGuid().ToString(),
        //            LLRowid = sLLRowid,
        //            w_no = crud.w_no,
        //            c_data = crud.c_data,
        //            s_data = crud.s_data,
        //            name = crud.name,
        //            sex = crud.sex,
        //            p_id = crud.p_id,
        //            //birth = crud.birth,
        //            //c_id = crud.c_id,
        //            //h_no = crud.h_no,
        //            //item_cname = crud.item_cname,
        //            //item_ename = crud.item_ename,
        //            //nhi_code = crud.nhi_code,
        //            //CHD_V = crud.CHD_V,
        //            //c_type = crud.c_type,
        //            //low = crud.low,
        //            //high = crud.high
        //        };
        //        db_zmlis.LisLaboratory_item.Add(entity);
        //        db_zmlis.SaveChanges();
        //        //crud.SMRowid = entity.SMRowid;

        //    }
        //    return Json(new[] { crud }.ToDataSourceResult(new DataSourceRequest(), ModelState));

        //}

        [HttpPost]
        public ActionResult LisSchema_importEX(HttpPostedFileBase excelfile) 
        {
            if (excelfile == null || excelfile.ContentLength == 0)
            {
                ViewBag.Error = "請選擇excel檔案! < br />";
                return View("Index");
            }
            else 
            {
                if (excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
                {
                    string path = Server.MapPath("~/FileUploads/" + excelfile.FileName);
                    if (System.IO.File.Exists(path))
                        System.IO.File.Exists(path);
                    excelfile.SaveAs(path);
                    //讀取excel資料
                    Excel.Application application = new Excel.Application();
                    Excel.Workbook workbook = application.Workbooks.Open(path);
                    Excel.Worksheet worksheet = workbook.ActiveSheet;
                    Excel.Range range = worksheet.UsedRange;
                    List<LisLaboratory_ViewModels_item> listitem = new List<LisLaboratory_ViewModels_item>();
                    for (int row = 2; row <= range.Rows.Column; row++)
                    {
                        LisLaboratory_ViewModels_item r = new LisLaboratory_ViewModels_item();
                        r.p_id = ((Excel.Range)range.Cells[row, 2]).Text;
                        r.name = ((Excel.Range)range.Cells[row, 3]).Text;
                        r.birth = ((Excel.Range)range.Cells[row, 4]).Text;
                        r.sex = ((Excel.Range)range.Cells[row, 5]).Text;
                        r.c_id = ((Excel.Range)range.Cells[row, 6]).Text;
                        listitem.Add(r);
                    }
                    ViewBag.ListAttendRecord = listitem;
                    return View("Success");
                }
                else
                {
                    ViewBag.Error = "檔案型態不正確! <br />";
                    return View("Index");
                }
            }
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

        #region GetLabStr_取得欄位清單
        public ActionResult GetLabStr(string sLLRowid)
        {
            if (sLLRowid != null)
            {
                var db_temp = from ll in db_zmlis.LisLaboratory_str
                              where ll.LLRowid == sLLRowid
                              select new
                              {
                                  SMRowid = ll.SMRowid,
                                  SMFieldName = ll.SMFieldName

                              };

                return Content(JsonConvert.SerializeObject(db_temp), "application/json");
            }

            return Content("");
        }
        #endregion

        #region GetLabStr_取得欄位篩選後清單
        public ActionResult GetSelectedLabStr(string sLLRowid)
        {
            if (sLLRowid != null)
            {
                var db_temp = from ll in db_zmlis.LisLaboratory_str
                              where ll.LLRowid == sLLRowid
                              orderby ll.SMDisplaySeq
                              select new
                              {
                                  SMRowid = ll.SMRowid,
                                  SMFieldName = ll.SMFieldName
                              };

                return Content(JsonConvert.SerializeObject(db_temp), "application/json");
            }

            return Content("");
        }
        #endregion

        #region GetStr_Creat 欄位存檔+順序
        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult GetStr_Create(string sId, string[] Items)
        //{
        //    // 先刪除該人員全部角色
        //    var delete_record = db_zmlis.LisLaboratory_str.Where(x => x.LLRowid == sId);
        //    db_zmlis.LisLaboratory_str.RemoveRange(delete_record);
        //    db_zmlis.SaveChanges();

        //    // 再把 list 內 insert into Table            
        //    for (var i = 0; i <= Items.Count(); i++)
        //    {
        //        db_zmlis.LisLaboratory_str.Add(
        //            new LisLaboratory_str
        //            {
        //                SMRowid = Guid.NewGuid().ToString(),
        //                SMDisplaySeq = i,
        //                LLRowid = sId          
        //            });

        //        db_zmlis.SaveChanges();
        //    }

        //    return Json(Items);
        //}
        #endregion

    }

}