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
//using ZMLisSys.Infrastructure.Helpers;
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

            viewModel.Menu += @"<li class='nav-item'><a href='/LisSchema/Index' class='nav-link'><i class='nav-icon far fa-circle text-warning'></i><p>檢驗測試</p></a></li>";

            viewModel.Menu += @"</ul></nav>";

            Session["Menu"] = viewModel.Menu;

            return View();
        }

        public ActionResult Index_Json()
        {
           

            return View();
        }
    }
}