//var url_HosptialLaboratoryItem_Create = "/ZMLisSys/HospitalLaboratory/HosptialLaboratoryItem_Create";
var url_HosptialLaboratoryItem_Create = "/ZMLisSys/HospitalLaboratory/HosptialLaboratoryItem_Create";
var url_HosptialLaboratoryItem_Destory = "/ZMLisSys/HospitalLaboratory/HosptialLaboratoryItem_Destory";

// #region Grid Datasource 資料讀取錯誤時的處理方式
function error_handler(e) {
    if (e.errors) {
        var message = "Errors:\n";
        $.each(e.errors, function (key, value) {
            if ('errors' in value) {
                $.each(value.errors, function () {
                    message += this + "\n";
                });
            }
        });
        alert(message);
    }
}
// #endregion

function sync_handler(e) { this.read(); }

// #region 同步 LisLaboratoryClass 資料並顯示在 Grid 上
function Sync_grid_LisLaboratoryClass(e) {
    this.read();
}
// #endregion

// #region 同步 lisLaboratoryItem 資料並顯示在 Grid 上
function Sync_grid_HospitalLaboratoryItem(e) {
    var grid = $('#grid_SysHospital').data('kendoGrid');
    var dsa = grid.dataItem(grid.select());

    if (dsa != null) {
        this.read({ sHospID: dsa.HospID });
    }     
}
// #endregion

// #region 同步 LisLaboratoryItem 資料並顯示在 Grid 上
function Sync_grid_lisLaboratoryItem(e) {
    var grid = $('#grid_LisLaboratoryClass').data('kendoGrid');
    var dsa = grid.dataItem(grid.select());

    if (dsa != null) {
        this.read({ sLLCRowid: dsa.LLCRowid });        
    }
}
// #endregion

// #region 同步 lisLaboratorySchedule 資料並顯示在 Grid 上
function grid_lisLaboratorySchedule_Sync(e) {
    var grid = $('#grid_SysHospital').data('kendoGrid');
    var dsa = grid.dataItem(grid.select());

    if (dsa != null) {
        this.read({ sHospRowid: dsa.HospRowid });
    } 
}

// #region 同步 HospitalLaboratoryItem 資料並顯示在 Grid 上
//function Sync_grid_HospitalLaboratoryItem(e) {
//    var grid = $('#grid_HospitalLaboratoryClass').data('kendoGrid');
//    var dsa = grid.dataItem(grid.select());

//    if (dsa != null) {
//        this.read({ sLLCRowid: dsa.LLCRowid });
//    }
//}
// #endregion

// #region 檢驗類別過濾選擇
function lisLaboratoryClassChange() {
    var value = this.value(),
        grid = $("#grid_HospitalLaboratoryItem").data("kendoGrid");
    
    if (value == '') {
        grid.dataSource.filter({});
    }
    else {
        grid.dataSource.filter({ field: "LLCRowid", operator: "eq", value: value });
    }
}
// #endregion

// #region 使用者輸入自訂搜尋 lisLaboratoryItem
function getlisLaboratoryItem() {
    var tbSearch = document.getElementById("searchName").value;
    var grid_HospitalLaboratoryItem = $("#grid_HospitalLaboratoryItem").data("kendoGrid");
        
    var grid_sysHospital = $('#grid_SysHospital').data('kendoGrid');
    var dsa_sysHospital = grid_sysHospital.dataItem(grid_sysHospital.select());

    grid_HospitalLaboratoryItem.dataSource.read(
        {
            sHospID: dsa_sysHospital.HospID,
            sLLCRowid: "",
            sSearchString: tbSearch
        });
}
// #endregion

function getHospID() {
    var grid_sysHospital = $('#grid_SysHospital').data('kendoGrid');
    var dsa_sysHospital = grid_sysHospital.dataItem(grid_sysHospital.select());

    return { sHospID: dsa_sysHospital.HospID };
}

// #region 使用者輸入自訂搜尋 sysHospital
function getSysHospital() {
    var tbSearch = document.getElementById("searchHospital").value;
    var grid_sysHospital = $("#grid_SysHospital").data("kendoGrid");

    grid_sysHospital.dataSource.read(
        {                        
            sSearchString: tbSearch
        });

    // 回第一頁    
    grid_sysHospital.one("dataBound", function (e) {
        var dataSource = e.sender.dataSource;
        //var items = dataSource.data().length;
        //var pageSize = dataSource.pageSize();
        //var pageNum = parseInt(items / pageSize) + 1;
        dataSource.page(1);
    });

    // grid_HospitalLaboratoryItem 要清空內容
    var grid_HospitalLaboratoryItem = $("#grid_HospitalLaboratoryItem").data("kendoGrid");
    //var dsa_sysHospital = grid_sysHospital.dataItem(grid_sysHospital.select());

    grid_HospitalLaboratoryItem.dataSource.read(
        {
            sHospID: "",
            sLLCRowid: "",
            sSearchString: ""
        });
}
// #endregion

function grid_sysHospitalonRead() {
    var tbSearch = document.getElementById("searchHospital").value;
    return { sSearchString : tbSearch };
}

function openImportSchema() {
    //e.preventDefault();
    wnd4ImportSchema.center().open();
}

// #region 各種訊息視窗定義
var wnd, wndConfirmMessage, wnd4ImportSchema   //, wnd4ReferralInForm;
$(document).ready(function () {
    wnd = $("#modalWindow").kendoWindow({
        title: "刪除資料確認",
        modal: true,
        visible: false,
        resizable: false,
        width: 300
    }).data("kendoWindow");

    wndConfirmMessage = $("#ConfirmFormWindow").kendoWindow({
        title: "訊息",
        modal: true,
        visible: false,
        resizable: false,
        width: 300
    }).data("kendoWindow");

    wnd4ImportSchema = $("#modalImportSchema").kendoWindow({
        title: "資料結構解析",
        modal: true,
        visible: false,
        resizable: false,
        width: 1000,
        LoadContentFrom: ("GetLabStr", "LisSchema")
    }).data("kendoWindow");

    //隱藏未選檢驗所的新增欄位按鈕
    $("#addRecord").hide();    
    $("#ImportSchema").hide();
    $("#addRecord").unbind('click').click(function (e) {
        e.preventDefault();
        var gridLisLab = $("#grid_LisLaboratory").data("kendoGrid");
        //var gridLisLabstr = $("#grid_LisLaboratory_str").data("kendoGrid");
        var dsa = gridLisLab.dataItem(gridLisLab.select());

        if (dsa == null) {
            alert("請選擇檢驗所");
            window.location.reload();
            $("#addRecord").hide();            
            $("#ImportSchema").hide();
        }
    });

    // 隱藏未選醫事機構的新增欄位按鈕
    $("#add_lisLaboratorySchedule").hide();
    $("#add_lisLaboratorySchedule").unbind('click').click(function (e) {
        e.preventDefault();
        var grid_sysHospital = $("#grid_SysHospital").data("kendoGrid");
        //var gridLisLabstr = $("#grid_LisLaboratory_str").data("kendoGrid");
        var dsa = grid_sysHospital.dataItem(grid_sysHospital.select());

        if (dsa == null) {
            alert("請選擇檢驗所");
            window.location.reload();
            $("#add_lisLaboratorySchedule").hide();        
        }
    });

    $("#switchView").unbind('click').click(function (e) {
        e.preventDefault();
        //alert(document.getElementById("switchView").innerText);
        if (document.getElementById("switchView").innerText == "開啟所有檢查項目") {            
            $('#grid_LisLaboratoryItemAll').data('kendoGrid').dataSource.read();            
            document.getElementById("switchView").innerText = "關閉所有檢查項目";
            $("#grid_LisLaboratoryItem").hide();
            $("#grid_LisLaboratoryItemAll").show();
        }
        else {
            document.getElementById("switchView").innerText = "開啟所有檢查項目";
            $("#grid_LisLaboratoryItem").show();
            $("#grid_LisLaboratoryItemAll").hide();
        }        
    });
});
// #endregion

function openWindow(e) {
    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");
    wnd.center().open();

    $("#yes").click(function () {
        grid.removeRow(row);
        wnd.close();
    });

    $("#no").click(function () {
        wnd.close();
    });
}

function grid_import(e) {
    e.preventDefault();
    var grid = $('#grid_LisLaboratory_str').data('kendoGrid');
    var dsa = grid.dataItem(grid.select());

    if (e.model.isNew()) {
        document.getElementById("SMRowid").value = dsa.SMRowid;
    }
}

function grid_edit_str(e) {
    e.preventDefault();
    var grid = $('#grid_LisLaboratory_str').data('kendoGrid');
    var dsa = grid.dataItem(grid.select());

    if (e.model.isNew()) {
        document.getElementById("SMRowid").value = dsa.SMRowid;
    }
}

function LisLaboratoryMaster_Grid_OnRowSelect(e) {
    // 取得資料前,先把條件變數做整理    
    var grid = e.sender;
    var dsa = grid.dataItem(grid.select());

    // 重新讀取檢驗所所屬結構
    $("#grid_lisLaboratoryDetail").data("kendoGrid").
        dataSource.read(
            {
                sLLMRowid: dsa.LLMRowid,
            });

    // 重新讀取 Mapping 結構
    $("#SourceSchemaListBox").data("kendoListBox").
        dataSource.read(
            {
                sLLMRowid: dsa.LLMRowid,
            });

    if (dsa.LLMRowid != null) {
        $("#addRecord").show();
        $("#ImportSchema").show();
    }
}

//取得檢驗Rowid 並且連動
function getLLMRowid() {
    var grid = $('#grid_lisLaboratoryMaster').data('kendoGrid');
    var dsa = grid.dataItem(grid.select());

    if (dsa != null) {
        $("#addRecord").show();
        var request =
        {
            sLLMRowid: dsa.LLMRowid
        };

    }
    else {
        $("#addRecord").hide();
    }

    return request;
}

//取得檢驗Rowid 並且連動
function getHospRowid() {
    var grid = $('#grid_SysHospital').data('kendoGrid');
    var dsa = grid.dataItem(grid.select());

    if (dsa != null) {
        $("#add_lisLaboratorySchedule").show();
        var request =
        {
            sHospRowid: dsa.HospRowid
        };
    }
    else {
        alert("請選擇檢驗所");
        window.location.reload();
        $("#add_lisLaboratorySchedule").hide();         
    }

    return request;
}

function onReorder(e) {
    e.preventDefault();
    var dataSource = e.sender.dataSource;

    var dataItem = e.dataItems[0]
    var index = dataSource.indexOf(dataItem) + e.offset;
    dataSource.remove(dataItem);
    dataSource.insert(index, dataItem);
    e.sender.wrapper.find("[data-uid='" + dataItem.uid + "']").addClass("k-state-selected");
}

// #region grid_LisLaboratoryClass_OnRowSelect : 檢驗檢查分類 Grid 選擇資料錄時的處理
function grid_LisLaboratoryClass_OnRowSelect(e) {
    // 取得資料前,先把條件變數做整理
    var grid = e.sender;
    var dsa = grid.dataItem(grid.select());

    if (dsa != null) {
        $("#addRecord").show();
        $("#exportExcel").show();

        var grid = $("#grid_LisLaboratoryItem").data("kendoGrid");
        // 重新讀取個案追蹝區塊
        grid.dataSource.read({
            sLLCRowid: dsa.LLCRowid
        });

        // 每次換頁會自動轉至第一頁
        grid.one("dataBound", function (e) {
            var dataSource = e.sender.dataSource;
            //var items = dataSource.data().length;
            //var pageSize = dataSource.pageSize();
            //var pageNum = parseInt(items / pageSize) + 1;
            dataSource.page(1);
        });
    }
    else {
        $("#addRecord").hide();
        $("#exportExcel").hide();
    }

    return;
}
// #endregion

// #region getLLCRowid :取得選擇的檢驗項目的 Rowid
function getLLCRowid() {
    var grid = $('#grid_LisLaboratoryClass').data('kendoGrid');
    var dsa = grid.dataItem(grid.select());

    if (dsa == null) {
        alert("請先選擇檢驗類別項目 !!");
    }
    else {
        var request = { sLLCRowid: dsa.LLCRowid };
    }
    
    return request;
}
// #endregion

// #region grid_SysHospital_OnRowSelect : 在醫療院所的檢驗檢查分類 Grid 選擇資料錄時的處理
function grid_SysHospital_OnRowSelect(e) {
    // 取得資料前,先把條件變數做整理
    var grid = e.sender;
    var dsa = grid.dataItem(grid.select());

    if (dsa != null) {        
        // 把過濾條件重置
        if (document.getElementById("ddl_lisLaboratoryClass") != null) {
            $("#ddl_lisLaboratoryClass").data("kendoDropDownList").select(0);
        }

        if (document.getElementById("searchName") != null) {
            document.getElementById("searchName").value = "";
        }

        if (document.getElementById("grid_HospitalLaboratoryItem") != null) {
            var grid = $("#grid_HospitalLaboratoryItem").data("kendoGrid");

            grid.dataSource.read({
                sHospID: dsa.HospID
                //sLLCRowid: "",
                //sSearchString: ""
            });

            // 每次換頁會自動轉至第一頁
            grid.one("dataBound", function (e) {
                var ds = e.sender.dataSource;
                //var items = ds.data().length;
                //var pageSize = ds.pageSize();
                //var pageNum = parseInt(items / pageSize) + 1;
                ds.page(1);
                //http://aisoftwarellc.com/blog/post/kendo-hacks---clear-filter-while-adding-new-rows-in-a-kendo-grid/2024
                if (grid.dataSource) grid.dataSource.filter([]);
            });
        }

        // 下面是醫事機構接檢驗所排程資料的 Grid
        if (document.getElementById("grid_lisLaboratorySchedule") != null) {
            var grid_LLS = $("#grid_lisLaboratorySchedule").data("kendoGrid");
            grid_LLS.dataSource.read({
                sHospRowid: dsa.HospRowid
            });

            //grid_LLS.one("dataBound", function (e) {
            //    var ds = e.sender.dataSource;               
            //    ds.page(1);
            //    //http://aisoftwarellc.com/blog/post/kendo-hacks---clear-filter-while-adding-new-rows-in-a-kendo-grid/2024
            //    if (grid_LLS.dataSource) grid_LLS.dataSource.filter([]);
            //});

            $("#add_lisLaboratorySchedule").show();
        }
    }
    else {
        //$("#addRecord").hide();
        $("#exportExcel").hide();
    }

    return;
}
// #endregion

// #region grid_HospitalLaboratoryClass_OnRowSelect : 在醫療院所的檢驗檢查分類 Grid 選擇資料錄時的處理 (20201215-取消此功能)
function grid_HospitalLaboratoryClass_OnRowSelect(e) {
    // 取得資料前,先把條件變數做整理
    var grid = e.sender;
    var dsa = grid.dataItem(grid.select());

    if (dsa != null) {        
        //$("#exportExcel").show();

        var grid = $("#grid_HospitalLaboratoryItem").data("kendoGrid");
        // 重新讀取個案追蹝區塊
        grid.dataSource.read({
            sLLCRowid: dsa.LLCRowid
        });

        // 每次換頁會自動轉至第一頁
        grid.one("dataBound", function (e) {
            var dataSource = e.sender.dataSource;
            //var items = dataSource.data().length;
            //var pageSize = dataSource.pageSize();
            //var pageNum = parseInt(items / pageSize) + 1;
            dataSource.page(1);
        });
    }
    else {
        //$("#addRecord").hide();
        $("#exportExcel").hide();
    }

    return;
}
// #endregion

// #region ChkData : 醫療院所的檢驗檢查分類 checkbox 勾選時的處理方式
function ChkData(e) {
    var row = $(e).closest("tr");
    var dataItem = $("#grid_HospitalLaboratoryItem").data("kendoGrid").dataItem(row);

    // 取得 Hospital 的選擇資料
    var grid = $('#grid_SysHospital').data('kendoGrid');
    var dsa = grid.dataItem(grid.select());

    var pHLIRowid = dataItem.HLIRowid;
    var pHospID = dsa.HospID
    //var pLLIRowid = dataItem.LLIRowid;
    //var bChecked = dataItem.IsChecked;
    var pRowid = "";
    var docase = "";

    if (pHLIRowid == "" || pHLIRowid == null) {        
        docase = "/ZMLISSys/HospitalLaboratory/HosptialLaboratoryItem_Create";        
        pRowid = dataItem.LLIRowid;
    }
    else {
        docase = "/ZMLISSys/HospitalLaboratory/HosptialLaboratoryItem_Destory";        
        pRowid = dataItem.HLIRowid;
    }

    if (docase != "") {
        $.ajax({
            url: docase,
            type: "Post",
            data: {
                sHospID: pHospID,
                sRowid: pRowid
            },
            success: function () {
                //alert("Success !!!!");
                //var grid = $('#grid_HospitalLaboratoryClass').data('kendoGrid');
                //var dsa = grid.dataItem(grid.select());
                var grid = $('#grid_SysHospital').data('kendoGrid');
                var dsa = grid.dataItem(grid.select());

                if (dsa != null) {
                    $('#grid_HospitalLaboratoryItem').data('kendoGrid').dataSource.read(
                        {
                            sHospID: pHospID
                            //sLLCRowid: dsa.LLCRowid
                        });
                }
            },
            error: function (data) {
                //alert("Fail !!!!" + data);
            }
        });
    }
}
// #endregion

// #region Autocolumnwidth : 控制 Grid 依資料的長度自動調整寬度
function Autocolumnwidth(e) {
    var grid = e.sender;
    for (var i = 0; i < grid.columns.length; i++) {
        grid.autoFitColumn(i);
    }
}
// #endregion

// #region HospitalLaboratoryItem_dataBound : 
//function HospitalLaboratoryItem_dataBound(e) {
//    var grid = this;

//    grid.tbody.find("tr[role='row']").each(function () {
//        var model = grid.dataItem(this);

//        if (model.IsChecked == false) {
//            $(this).find(".k-grid-edit").click(false).addClass("lfmouseover");
//            //$(this).find(".k-grid-delete").click(false).addClass("lfmouseover");
//        }        
//    });
//}
// #endregion

// #region resizeGrid : 重繪 Grid 內容
function resizeGrid() {
    //Define Elements Needed
    var header = $("#header-content");
    var content = $("#main-content");
    var grid = $("#grid_AspNetUser");

    //Other variables
    var minimumAcceptableGridHeight = 250; //This is roughly 5 rows
    var otherElementsHeight = 0;

    //Get Window Height
    var windowHeight = $(window).innerHeight();

    //Get Header Height if its existing
    var hasHeader = header.length;
    var headerHeight = hasHeader ? header.outerHeight(true) : 0;

    //Get the Grid Element and Areas Inside It
    var contentArea = grid.find(".k-grid-content");  //This is the content Where Grid is located
    var otherGridElements = grid.children().not(".k-grid-content"); //This is anything ather than the Grid iteslf like header, commands, etc
    console.debug(otherGridElements);

    //Calcualte all Grid elements height
    otherGridElements.each(function () {
        otherElementsHeight += $(this).outerHeight(true);
    });

    //Get other elements same level as Grid
    var parentDiv = grid.parent("div");

    var hasMainContent = parentDiv.length;
    if (hasMainContent) {
        var otherSiblingElements = content.children()
            .not("#grid_AspNetUser")
            .not("script");

        //Calculate all Sibling element height
        otherSiblingElements.each(function () {
            otherElementsHeight += $(this).outerHeight(true);
        });
    }

    //Padding you want to apply below your page
    var bottomPadding = 210;

    //Check if Calculated height is below threshold
    var calculatedHeight = windowHeight - headerHeight - otherElementsHeight - bottomPadding;
    var finalHeight = calculatedHeight < minimumAcceptableGridHeight ? minimumAcceptableGridHeight : calculatedHeight;

    //Apply the height for the content area
    contentArea.height(finalHeight);
}
// #endregion

function grid_lisPatientLaboratoryMasterRead() {    
    // 取得資料前,先把條件變數做整理
    var HospIDValue = $("#ddlSysHospital").data("kendoDropDownList").value();
    var startDateTimeValue = $("#startDateTime").data("kendoDatePicker").value();
    var endDateTimeValue = $("#endDateTime").data("kendoDatePicker").value();

    return { sHospID: HospIDValue, startDate: startDateTimeValue, endDate: endDateTimeValue };
}

// #region GetSysUploadServerSelected : 取得選擇的醫事機構代碼
function GetLaboratoryData() {
    // 取得資料前,先把條件變數做整理
    var HospIDValue = $("#ddlSysHospital").data("kendoDropDownList").value();
    var startDateTimeValue = $("#startDateTime").data("kendoDatePicker").value();
    var endDateTimeValue = $("#endDateTime").data("kendoDatePicker").value();

    // 產生表頭資料
    $("#grid_lisPatientLaboratoryMaster").data("kendoGrid").dataSource.read({ sHospID: HospIDValue, startDate: startDateTimeValue, endDate: endDateTimeValue });

    // 清除表身的 Grid 資料            
    $("#grid_lisPatientLaboratoryDetail").data("kendoGrid").dataSource.read('');

    // 產生伺服器排程資料
    $("#grid_SysUploadServer").data("kendoGrid").dataSource.read(HospIDValue);

    $("#btnAppPush").hide();
}
// #endregion

function lisPatientLaboratoryMaster_Grid_OnRowSelect(e) {
    // 取得資料前,先把條件變數做整理
    var HospIDValue = $("#ddlSysHospital").data("kendoDropDownList").value();
    var grid = e.sender;
    var dsa = grid.dataItem(grid.select());

    // 重新讀取檢驗所所屬結構
    $("#grid_lisPatientLaboratoryDetail").data("kendoGrid").
        dataSource.read(
            {
                sHospID: HospIDValue,
                sPLMRowid: dsa.PLMRowid
            });

    $("#btnAppPush").show();
}

function lplmPushStatus(e) {
    // get the index of the PLDValue cell
    var columns = e.sender.columns;
    var columnIndex = this.wrapper.find(".k-grid-header [data-field=" + "PLMPTIdno" + "]").index();

    // iterate the table rows and apply custom row and cell styling
    var rows = e.sender.tbody.children();
    for (var j = 0; j < rows.length; j++) {
        var row = $(rows[j]);
        var dataItem = e.sender.dataItem(row);

        var bAPPFlag = dataItem.get("APPFlag");       

        var cell = row.children().eq(columnIndex);
        if (bAPPFlag == true) {
            cell.addClass("colorAPP");
        }
    }
}

function grid_lisPatientLaboratoryDateGroup_OnRowSelect(e) {
    // 取得資料前,先把條件變數做整理
    var grid = e.sender;
    var dsa = grid.dataItem(grid.select());

    //alert(dsa.HospID + "    " + dsa.PLMRowid);

    if (dsa != null) {
        var grid = $("#grid_lisPatientLaboratoryDetail").data("kendoGrid");                
        // 重新讀取
        grid.dataSource.read({
            sHospID: dsa.HospID,
            sPLMRowid: dsa.PLMRowid
        });        
        grid.clearSelection();

        // 每次換頁會自動轉至第一頁
        grid.one("dataBound", function (e) {
            var dataSource = e.sender.dataSource;
            //var items = dataSource.data().length;
            //var pageSize = dataSource.pageSize();
            //var pageNum = parseInt(items / pageSize) + 1;
            dataSource.page(1);
        });

        document.getElementById('divAppPush').style.display = 'block';
        document.getElementById("lbCopyText").innerText = "";
    }
    else {
        //$("#addRecord").hide();
        //$("#exportExcel").hide();
    }

    return;
}

// #region Patient_Grid_OnRowSelect : 右側邉條輔助查詢功能
function Patient_Grid_OnRowSelect(e) {
    // 取得資料前,先把條件變數做整理
    var grid = e.sender;
    var selectedRow = grid.select();
    var dataItem = grid.dataItem(selectedRow);

    //alert(dataItem.hospID + '\n' + dataItem.id);
    var HospID = dataItem.hospID;
    var strUserId = dataItem.id;

    // Reload ViewBag
    $.ajax({
        url: "/ZMLisSys/GetData/PatientBase",
        type: "GET",
        data: {
            HospID: HospID,
            strUserId: strUserId
        },
        success: function (data) {
            document.getElementById("PatientName").innerText = data[0].trim();
            document.getElementById("PatientGender").innerText = data[1].trim();
            document.getElementById("PatientAge").innerText = data[2].trim();
            document.getElementById("PatientMeasureDate").innerText = data[3].trim();
            document.getElementById("PatientHeight").innerText = data[4].trim();
            document.getElementById("PatientWeight").innerText = data[5].trim();
            //document.getElementById("PatientTel").innerText = data[6].trim();
            document.getElementById("PatientMyAge").innerText = data[7].trim();
        }
    });

    // 產生病人的檢驗歷史日期
    $("#grid_lisPatientLaboratoryDateGroup").data("kendoGrid").
        dataSource.read(
            {
                HospID: HospID,
                strUserId: strUserId
            });
    $("#grid_lisPatientLaboratoryDateGroup").data("kendoGrid").clearSelection();

    // 產生病人的檢驗項目
    $("#grid_lisPatientLaboratoryClassGroup").data("kendoGrid").
        dataSource.read(
            {
                HospID: HospID,
                strUserId: strUserId
            });
    $("#grid_lisPatientLaboratoryClassGroup").data("kendoGrid").clearSelection();

    // 產生病人檢驗 POVIT
    $("#grid_lisBoardClassDetail").data("kendoGrid").
        dataSource.read(
            {
                HospID: HospID,
                sPTRowid: strUserId,
                sJsonString: null
            });
    $("#grid_lisBoardClassDetail").data("kendoGrid").clearSelection();


    // 檢驗明細資料需清除
    $("#grid_lisPatientLaboratoryDetail").data("kendoGrid").
        dataSource.read(
            {                
                sHospID: HospID,
                sPLMRowid: ""
            });

    document.getElementById("lbCopyText").innerText = "";    
}
// #endregion

// #region getFilterPatient : 依查詢條件內容搜尋病人基本資料
function getFilterPatient(sHospID) {
    var str = document.getElementById("searchPatient").value;

    //if (code.replace(/(^s*)|(s*$)/g, "").length == 0) { code = "0" };
    //const parsed = parseInt(code, 10);
    //if (isNaN(parsed)) { parsed = 0; }

    // 重新讀取病人列表清單
    $("#grid_Patient").data("kendoGrid").
        dataSource.read(
            {
                sHospID: sHospID,
                sSearchString: str
            });
    return;
}
// #endregion

// #region GetSysUploadServerSelected : 取得選擇的醫事機構代碼
function GetSysUploadServerSelected(e) {
    var HospIDValue = $("#ddlSysHospital").data("kendoDropDownList").value();
    return { sHospID: HospIDValue };
}
// #endregion

function onClose() {
    $("#uploadButton").show();
    var grid = $("#grid_SysUploadServer").data("kendoGrid");
    grid.dataSource.read();
}

function onCancel() {
}

function onDialogClose() {
    $("#showDialogBtn").fadeIn();
}

function onDialogOpen() {
    $("#showDialogBtn").fadeOut();
}

function showDialog() {
    $("#dialogWindow").data("kendoDialog").open();
    //$('#dialogWindow').data("kendoDialog").open();
}

function onUpload(e) {
    var HospIDValue = $("#ddlSysHospital").data("kendoDropDownList").value();

    e.data = { HospID: HospIDValue };
}

function PushDHO(sMode) {
    var HospID = "";
    var PLMRowid = "";
    var UserId = "";

    if (sMode == "D") {
        var lplm_grid = $("#grid_lisPatientLaboratoryMaster").data("kendoGrid");
        var dataItem = lplm_grid.dataItem(lplm_grid.select());

        // 取得資料前,先把條件變數做整理
        var HospID = $("#ddlSysHospital").data("kendoDropDownList").value();
        var PLMRowid = dataItem.PLMRowid;
        var UserId = dataItem.PLMPTIdno;
    }
    else if (sMode == "B") {
        var lpldg_grid = $("#grid_lisPatientLaboratoryDateGroup").data("kendoGrid");
        var dataItem = lpldg_grid.dataItem(lpldg_grid.select());

        // 取得資料前,先把條件變數做整理
        var HospID = dataItem.HospID;
        var PLMRowid = dataItem.PLMRowid;
        var UserId = dataItem.PTIdno;
    }

    //alert(HospID + '   \n' + PLMRowid + '    \n' + UserId);

    // Call Push DHO Controller
    if (HospID != "") {
        $.ajax({
            url: "/ZMLisSys/LISUpload/PushDHO",
            type: "GET",
            data: {
                HospID: HospID,
                UserId: UserId,
                PLMRowid: PLMRowid,
            },
            success: function (data) {
                alert(data);
                //document.getElementById("PatientName").innerText = data[0].trim();
                //document.getElementById("PatientGender").innerText = data[1].trim();
                //document.getElementById("PatientAge").innerText = data[2].trim();
                //document.getElementById("PatientMeasureDate").innerText = data[3].trim();
                //document.getElementById("PatientHeight").innerText = data[4].trim();
                //document.getElementById("PatientWeight").innerText = data[5].trim();
                ////document.getElementById("PatientTel").innerText = data[6].trim();
                //document.getElementById("PatientMyAge").innerText = data[7].trim();

                wndConfirmMessage.center().open();
            }
        });        
    }
    else {
        // 顯示傳送失敗訊息
        wndConfirmMessage.center().open();
    };

    
}