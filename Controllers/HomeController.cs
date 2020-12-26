using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using ZMLISSys.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PagedList;
using ZMLISSys.ViewModels;
//using ZMLisSys.Infrastructure.Helpers;
using System.Web.Script.Serialization;
using System.Collections;

namespace ZMLISSys.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            MenuViewModel viewModel = new MenuViewModel();

            viewModel.Menu += @"<nav class='mt-2'>
                                    <ul class='nav nav-pills nav-sidebar flex-column' data-widget='treeview' role='menu' data-accordion='false'>
                                        <li class='nav-item'>
                                            <a href='/LaboratoryBase/LBIndexV1' class='nav-link'>
                                                <i class='nav-icon far fa-circle text-warning'></i><p>檢驗項目主檔維護 V1.0</p>
                                            </a>
                                        </li>
                                        <li class='nav-item'>
                                            <a href='/LaboratoryBase/LBIndexV2' class='nav-link'>
                                                <i class='nav-icon far fa-circle text-warning'></i><p>檢驗項目主檔維護 V2.0</p>
                                            </a>
                                        </li>
                                        <li class='nav-item'>
                                            <a href='/HospitalLaboratory/LHIndex' class='nav-link'>
                                                <i class='nav-icon far fa-circle text-warning'></i><p>檢驗檢查項目維護</p>
                                            </a>
                                        </li>
                                        <li class='nav-item'>
                                            <a href='/LISSchema/LSIndex' class='nav-link'>
                                                <i class='nav-icon far fa-circle text-warning'></i><p>檢驗所檢驗結構維護</p>
                                            </a>
                                        </li>
                                        <li class='nav-item'>
                                            <a href='/HospitalLaboratory/H2LIndex' class='nav-link'>
                                                <i class='nav-icon far fa-circle text-warning'></i><p>醫事機構檢驗所對應</p>
                                            </a>
                                        </li>
                                        <li class='nav-item'>
                                            <a href='/LISUpload/LUIndex' class='nav-link'>
                                                <i class='nav-icon far fa-circle text-warning'></i><p>檢驗資料匯入作業</p>
                                            </a>
                                        </li>
                                    </ul>
                                </nav>";

            Session["Menu"] = viewModel.Menu;

            Session["HospID"] = "3532040438";
            Session["HospRowid"] = "9B366860-0837-4817-BF93-53530B5913D2";
            Session["UserRowid"] = "a7aa1465-4566-4142-ae09-ffdc9d107a25";          // ufoujoey@gmail.com

            return View();
        }

        public ActionResult Index_Json()
        {
            return View();
        }
    }
}