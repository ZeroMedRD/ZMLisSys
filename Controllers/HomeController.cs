using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using ZMLisSys.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PagedList;
using ZMLisSys.ViewModels;
using ZMLisSys.Infrastructure.Helpers;
using System.Web.Script.Serialization;
using System.Collections;

namespace ZMLisSys.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            MenuViewModel viewModel = new MenuViewModel();

            viewModel.Menu += @"<nav class='mt-2'>
                                    <ul class='nav nav-pills nav-sidebar flex-column' data-widget='treeview' role='menu' data-accordion='false'>";

            viewModel.Menu += @"<li class='nav-item'><a href='/Listests/Index' class='nav-link'><i class='nav-icon far fa-circle text-warning'></i><p>檢驗測試</p></a></li>";

            viewModel.Menu += @"</ul></nav>";

            Session["Menu"] = viewModel.Menu;

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Index_Json()
        {
            //    List<JsonHelper> data = new List<JsonHelper>()
            //    {
            //        new JsonHelper(){ID="1",TestName="小白"},
            //        new JsonHelper(){ID="2",TestName="小黑"}
            //    };
            //List<JsonHelper> data = new List<JsonHelper>();
            //JArray ja = new JArray();
            ////List<Dictionary<string, object>> ja = new List<Dictionary<string, object>>();
            //foreach (var item in data)
            //{
            //    JObject jo = new JObject();
            //    //Dictionary<string, object> jo = new Dictionary<string, object>();
            //    jo.Add("ID", item.ID);
            //    jo.Add("TestName", item.TestName);
            //    jo.Add("P_name", item.P_name);
            //    jo.Add("Sex_id", item.Sex_id);
            //    jo.Add("Idno_id", item.Idno_id);
            //    ja.Add(jo);

            //}
            //return Content(JsonConvert.SerializeObject(ja), "application/json");

            ////## 模擬上傳的檔案內容
            //List<JsonHelper> oStr = new List<JsonHelper>();
            ////oStr.Add(new JsonHelper() { ID = 1, Name = "王1" });
            ////oStr.Add(new JsonHelper() { ID = 2, Name = "王2" });

            ////## 將結果回傳
            //return Json(new { isUploaded = true, result = oStr }, "text/html");

            List<JsonHelper> lstStuModel = new List<JsonHelper>()
                {
                    new JsonHelper(){ID="1",TestName="小白"},
                    new JsonHelper(){ID="2",TestName="小黑"}
                };

            //Newtonsoft.Json序列化
            string jsonData = JsonConvert.SerializeObject(lstStuModel);
            //顯示
            Response.Write(jsonData);
            return View();
        }
        [HttpPost]
        public JsonResult GetJsonHelper()
        {
            //List<JsonHelper> lstStuModel = new List<JsonHelper>()
            //    {
            //        new JsonHelper(){ID="1",TestName="小白"},
            //        new JsonHelper(){ID="2",TestName="小黑"}
            //    };

            ////Newtonsoft.Json序列化
            //string jsonData = JsonConvert.SerializeObject(lstStuModel);
            ////顯示
            //Response.Write(jsonData);

            //List<JsonHelper> data = new List<JsonHelper>();
            List<JsonHelper> lstStuModel = new List<JsonHelper>();
            //JArray ja = new JArray();
            List<Dictionary<string, object>> ja = new List<Dictionary<string, object>>();
            foreach (var item in lstStuModel)
            {
                //JObject jo = new JObject();
                Dictionary<string, object> jo = new Dictionary<string, object>();
                jo.Add("ID", item.ID);
                jo.Add("TestName", item.TestName);
                //jo.Add("P_name", item.P_name);
                //jo.Add("Sex_id", item.Sex_id);
                //jo.Add("Idno_id", item.Idno_id);
                ja.Add(jo);
            }
            //return Content(JsonConvert.SerializeObject(ja), "application/json");
            return Json(ja);

        }
        [HttpPost]
        public JsonResult Save(string postData)
        {
            return Json(postData);
        }

        [HttpPost]
        public JsonResult Save2(string order)
        {
            return Json("ok");
        }

        //    ////Json.NET反序列化
        //    //string json = @"{ 'TestName':'C#','Age':'3000','ID':'1','Sex':'女'}";
        //    //JsonHelper descJsonStu = JsonConvert.DeserializeObject<JsonHelper>(json);//反序列化
        //    //Response.Write(string.Format("反序列化： ID={0},TestName={1}", descJsonStu.ID, descJsonStu.TestName));

        //    return View();
        //}
    }



}