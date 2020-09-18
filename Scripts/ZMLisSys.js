var wnd, wndConfirmMessage   //, wnd4ReferralOutForm, wnd4ReferralInForm;
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

    wnd4insert = $("#modalInsertWindow").kendoWindow({
        title: "模組確認",
        modal: true,
        visible: false,
        resizable: false,
        width: 300
    }).data("kendoWindow");

    $("#addRecord").unbind('click').click(function (e) {
        e.preventDefault();

        var gridLab = $('#grid_LisLaboratory').data('kendoGrid');
        var gridSch = $('#grid_LisLaboratory_str').data('kendoGrid');
        var dsa = gridSch.dataItem(gridSch.select());


        if (dsa != null)
        {
            
        }
        else
        {
            alert("請選擇檢驗所或新增!!");
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

function LisLaboratory_Grid_OnRowSelect(e) {
    // 取得資料前,先把條件變數做整理    
    var grid = e.sender;
    var dsa = grid.dataItem(grid.select());
    //alert(dsa.LLRowid);
    
    // 重新讀取個案追區塊
    $("#grid_LisLaboratory_str").data("kendoGrid").
        dataSource.read(
            {
                sLLRowid: dsa.LLRowid,
            });
}

//取得檢驗Rowid 並且連動
function getLLRowid() {
    var grid = $('#grid_LisLaboratory').data('kendoGrid');
    var dsa = grid.dataItem(grid.select());

    var request = {
        sLLRowid: dsa.LLRowid
    };

    return request;
}
