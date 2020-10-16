function openImportSchema() {  
    //e.preventDefault();
    wnd4ImportSchema.center().open();
   
}   

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
        //LoadContentFrom: ("GetLabStr", "LisSchema")
    }).data("kendoWindow");

    //隱藏未選檢驗所的新增欄位按鈕
    $("#addRecord").hide();
    $("#ImportSchema").hide();
    $("#addRecord").unbind('click').click(function (e) {
        e.preventDefault();
        var gridLisLab = $("#grid_LisLaboratory").data("kendoGrid");
        //var gridLisLabstr = $("#grid_LisLaboratory_str").data("kendoGrid");
        var dsa = gridLisLab.dataItem(gridLisLab.select());

        if (dsa == null)
        {
            alert("請選擇檢驗所");
            window.location.reload();
            $("#addRecord").hide();
            $("#ImportSchema").hide();
        }
        
    });
});


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

function LisLaboratory_Grid_OnRowSelect(e) {
    // 取得資料前,先把條件變數做整理    
    var grid = e.sender;
    var dsa = grid.dataItem(grid.select());
    // 重新讀取個案追區塊
    $("#grid_LisLaboratory_str").data("kendoGrid").
        dataSource.read(
            {
                sLLRowid: dsa.LLRowid,
            });
    
    if (dsa.LLRowid != null) {
        $("#addRecord").show();
        $("#ImportSchema").show();
    }
    
}

//取得檢驗Rowid 並且連動
function getLLRowid() {
    var grid = $('#grid_LisLaboratory').data('kendoGrid');
    var dsa = grid.dataItem(grid.select());

    if (dsa != null) {
        $("#addRecord").show();
        var request =
        {
            sLLRowid: dsa.LLRowid
        };

    }
    else
    {
        $("#addRecord").hide();

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
