$(document).ready(function () {
    clickHookup();
})

function clickHookup() {
    $("#ckPLDType").click(function () {
        if (this.checked) {
            //alert("checkbox enable !!");
            $('#grid_lisPatientLaboratoryDetail').data('kendoGrid').dataSource.sort([{ field: 'PLDType', dir: 'asc' }, { field: 'HLISeqno', dir: 'asc' }]);
        }
        else {
            //alert("checkbox disable !!");
            $('#grid_lisPatientLaboratoryDetail').data('kendoGrid').dataSource.sort([{ field: 'HLISeqno', dir: 'asc' }]);            
        }
    });    

    $("#ckSelectPLDType").click(function () {
        var grid = $('#grid_lisPatientLaboratoryDetail').data('kendoGrid');
        var rows = grid.items();

        if (this.checked) {
            var selectString = '';            

            $(rows).each(function (e) {
                if (selectString == '') {
                    selectString = '{"data":[';                    
                }

                var row = this;
                var dataItem = grid.dataItem(row);

                if (dataItem.PLDType != "N") {                    
                    // grid.select(row);           // 此作業會觸發 onChange 事件                    
                    
                    // 取得資料放置到 Json
                    selectString += '{"PLDCode":"' + dataItem.PLDCode + '","PLDValue":"' + dataItem.PLDValue + '" },';
                }
            });

            // 把選擇的資料產出 text
            if (selectString != '') {
                var selectString = selectString.slice(0, -1);
                //selectString += ']'
                selectString += ']}';

                // 把Json資料post至檢驗格式產生 Library並回傳結果,再把結果顯示到<Label>lbCopyText
                $.ajax({
                    url: "/GetData/GetLisTemplate",
                    type: "get",
                    data: {
                        jsonString: selectString
                    },
                    success: function (data) {
                        document.getElementById("lbCopyText").innerHTML = data[0].trim();
                    },
                    error: function (data) {
                        //alert("Fail !!!!" + data);
                    }
                });
            }
            else {
                document.getElementById("lbCopyText").innerHTML = '';
            };
        }
        else {
            $(rows).each(function (e) {
                var row = this;
                //var dataItem = grid.dataItem(row);
                grid.select("tr:eq(1)");
                grid.clearSelection();  

                document.getElementById("lbCopyText").innerHTML = '';
            });
        }
    });
}

function click_event(sCase) {
    //alert(sCase);
}

function onChange(e) {
    ////alert("[" + this.selectedKeyNames().join(", ") + "]");    
    var rows = e.sender.select();
    var selectString = '';    
    rows.each(function (e) {
        if (selectString == '') {
            selectString = '{"data":[';            
        }
        var grid = $("#grid_lisPatientLaboratoryDetail").data("kendoGrid");
        var dataItem = grid.dataItem(this);

        // 取得資料放置到 Json
        selectString += '{"PLDCode":"' + dataItem.PLDCode + '","PLDValue":"' + dataItem.PLDValue + '" },';

        //selectString += dataItem.PLDName + "：" + dataItem.PLDValue + ",";
    })

    if (selectString != '') {
        var selectString = selectString.slice(0, -1);
        //selectString += ']'
        selectString += ']}';

        // 把Json資料post至檢驗格式產生 Library並回傳結果,再把結果顯示到<Label>lbCopyText
        $.ajax({
            url: "/GetData/GetLisTemplate",
            type: "get",
            data: {
                jsonString: selectString
            },
            success: function (data) {
                document.getElementById("lbCopyText").innerHTML = data[0].trim();
            },
            error: function (data) {
                //alert("Fail !!!!" + data);
            }
        });
    }
    else {
        document.getElementById("lbCopyText").innerHTML = '';
    };

    //document.getElementById("lbCopyText").innerText = selectString;
}

function textcopy() {
    /* Get the text field */
    var copyArea = document.getElementById("lbCopyText");
    var range = document.createRange();
    range.selectNode(copyArea);
    window.getSelection().removeAllRanges();
    window.getSelection().addRange(range);

    try {
        // 執行瀏覽器的複製指令，複製上面 copyArea 內選取到的文字
        var copyStatus = document.execCommand('copy');
        //var msg = copyStatus ? 'copied : ' + copyArea : 'failed';
        // 輸出狀態
        //alert(msg);
    } catch (error) {
        alert('Oops!, 拷備資料失敗 !');
    }

    window.getSelection().removeAllRanges(); 
}

function textreset() {
    document.getElementById("lbCopyText").innerText = "";
}

function onDataBound(e) {
    // https://docs.telerik.com/kendo-ui/knowledge-base/style-rows-cells-based-on-data-item-values?_ga=2.225650457.224504676.1613533110-1432293029.1609226287
    // get the index of the PLDValue cell
    var columns = e.sender.columns;
    var columnIndex = this.wrapper.find(".k-grid-header [data-field=" + "PLDValue" + "]").index();

    // iterate the table rows and apply custom row and cell styling
    var rows = e.sender.tbody.children();
    for (var j = 0; j < rows.length; j++) {
        var row = $(rows[j]);
        var dataItem = e.sender.dataItem(row);

        var PLDValue = dataItem.get("PLDValue");
        var PLDType = dataItem.get("PLDType");

        var cell = row.children().eq(columnIndex);
        if (PLDType != "N") {
            cell.addClass("overvalue");
        }
    }
}

$(document).ready(function () {
    wnd4Chart = $("#modalChartWindow").kendoWindow({
        title: "檢驗歷史曲線圖",
        modal: true,
        visible: false,
        resizable: false,
        scrollable: true,
        width: 1000,
        height: 700
    }).data("kendoWindow");
});

function openLISChartWindow(e) {
    //var sAge = document.getElementById("PatientMyAge").innerText;
    // Get grid_lisPatientLaboratoryDateGroup PTRowid
    var grid = $('#grid_lisPatientLaboratoryDateGroup').data('kendoGrid');
    var dsa = grid.dataItem(grid.select());

    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var sHospID = dsa.HospID;
    var sPTRowid = dsa.PTRowid;
    var sPLDCode = dataItem.PLDCode;
    var sPLDName = dataItem.PLDName;
    //alert(sPTRowid + "\n" + sPLDCode + "\n" + sPLDName);

    var wnd4LISChartForm = $("#modalChartWindow").data("kendoWindow");
    wnd4LISChartForm.setOptions({
        title: "【" + sPLDCode + " - " + sPLDName + "】 檢驗歷史曲線圖",
        content: "Loading....."
    }); 

    var chart = $("#LISChart").data("kendoChart");    
    chart.dataSource.read({
        sHospID: sHospID,
        sPTRowid: sPTRowid,
        sPLDCode: sPLDCode
    });
    chart.refresh();

    // 重新讀取 chart 下的 Grid
    $("#grid_LPLDChart").data("kendoGrid").dataSource.read({
        sHospID: sHospID,
        sPTRowid: sPTRowid,
        sPLDCode: sPLDCode
    });
    $("#grid_LPLDChart").data("kendoGrid").refresh()
   
    wnd4LISChartForm.center().open();    
}

// #region 設定 Ctrl + C, Ctrl + V 功能
//$(document).ready(function () {
//    var ctrlDown = false,
//        ctrlKey = 17,
//        cmdKey = 91,
//        vKey = 86,
//        cKey = 67;

//    $(document).keydown(function (e) {
//        if (e.keyCode == ctrlKey || e.keyCode == cmdKey) ctrlDown = true;
//    }).keyup(function (e) {
//        if (e.keyCode == ctrlKey || e.keyCode == cmdKey) ctrlDown = false;
//    });

//    $(".no-copy-paste").keydown(function (e) {
//        if (ctrlDown && (e.keyCode == vKey || e.keyCode == cKey)) return false;
//    });

//    // Document Ctrl + C/V 
//    $(document).keydown(function (e) {
//        if (ctrlDown && (e.keyCode == cKey)) { console.log("Document catch Ctrl+C"); textcopy(); };
//        if (ctrlDown && (e.keyCode == vKey)) console.log("Document catch Ctrl+V");
//    });
//});
// #endregion