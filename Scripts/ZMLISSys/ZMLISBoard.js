$(document).ready(function () {
    clickHookup();
})

// 定義一全變數,區分單選複選的方式
var bGVar = false;
var gPTRowid = "";

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

        bGVar = true;

        // 先清除選擇的結果,再開始判斷異常值的選擇
        $(rows).each(function (e) {
            var row = this;
            //var dataItem = grid.dataItem(row);
            grid.select("tr:eq(1)");
            grid.clearSelection();

            document.getElementById("lbCopyText").innerHTML = '';
        });

        if (this.checked) {
            var selectString = '';            

            $(rows).each(function (e) {
                if (selectString == '') {
                    selectString = '{"data":[';                    
                }

                var row = this;
                var dataItem = grid.dataItem(row);

                if (dataItem.PLDType != "N") {                    
                    grid.select(row);           // 此作業會觸發 onChange 事件                    
                    
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
                    url: "/ZMLisSys/GetData/GetLisTemplate",
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

        bGVar = false;
    });

    $("#grid_lisBoardClassDetail").on("dblclick", "tr", function () {
        var grid = $("#grid_lisBoardClassDetail").data("kendoGrid");
        var selectedRows = grid.dataItem($(this));

        var class_grid = $("#grid_lisPatientLaboratoryClassGroup").data("kendoGrid");
        //var class_selectedRows = class_grid.dataItem(this);

        //var grid = $("#grid").data("kendoGrid");
        // 取得左側所選到的項目
        var selected = [];
        class_grid.select().each(function () {
            selected.push(class_grid.dataItem(this));            
        });

        // test
        //selected.forEach(class_items => {
        //    alert(class_items.PLDCode);
        //});
        
        // 定義項目陣列並把右側所選的檢驗值放至 itemList 陣列中
        var itemList = [];        
        selectedRows.forEach(items => {
            itemList.push(items);            
        })

        // 把二邊的資料做關聯
        var selectString = '';
        var i = 1;
        selected.forEach(class_items => {
            if (selectString == '') {
                selectString = '{"data":[{"PLDCode":"PLMApplyDate","PLDValue":"' + itemList[0] + '" },';
            }

            // 測試結果
            //alert(class_items.PLDCode + '  ' + itemList[i]);
            // 取得資料放置到 Json
            selectString += '{"PLDCode":"' + class_items.PLDCode + '","PLDValue":"' + itemList[i] + '" },';

            i++;
        });

        // 把選擇的資料產出 text
        if (selectString != '') {
            var selectString = selectString.slice(0, -1);
            //selectString += ']'
            selectString += ']}';

            // 把Json資料post至檢驗格式產生 Library並回傳結果,再把結果顯示到<Label>lbCopyText
            $.ajax({
                url: "/ZMLisSys/GetData/GetLisTemplate",
                type: "get",
                data: {
                    jsonString: selectString
                },
                success: function (data) {
                    document.getElementById("lbClassCopyText").innerHTML = data[0].trim();
                },
                error: function (data) {
                    //alert("Fail !!!!" + data);
                }
            });
        }
        else {
            document.getElementById("lbClassCopyText").innerHTML = '';
        };

        var selectString = '';         
    });
}

function RefreshData(pHospID, pPTRowid) {
    //alert(pHospID + '      ' +  pPTRowid);
    // 設定目前選擇的病人資料序號
    gPTRowid = pPTRowid;
    
    // Reload ViewBag
    $.ajax({
        url: "/ZMLisSys/GetData/PatientBase",
        type: "GET",
        data: {
            HospID: pHospID,
            strUserId: pPTRowid
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
    //$("#grid_lisPatientLaboratoryDateGroup").data("kendoGrid").
    //    dataSource.read(
    //        {
    //            HospID: pHospID,
    //            strUserId: pPTRowid
    //        });
    //$("#grid_lisPatientLaboratoryDateGroup").data("kendoGrid").clearSelection();

    // 產生病人的檢驗項目
    //$("#grid_lisPatientLaboratoryClassGroup").data("kendoGrid").
    //    dataSource.read(
    //        {
    //            HospID: pHospID,
    //            strUserId: pPTRowid
    //        });
    //$("#grid_lisPatientLaboratoryClassGroup").data("kendoGrid").clearSelection();

    // 產生病人檢驗 POVIT
    //$("#grid_lisBoardClassDetail").data("kendoGrid").
    //    dataSource.read(
    //        {
    //            HospID: pHospID,
    //            sPTRowid: pPTRowid,
    //            sJsonString: null
    //        });
    //$("#grid_lisBoardClassDetail").data("kendoGrid").clearSelection();


    // 檢驗明細資料需清除
    //$("#grid_lisPatientLaboratoryDetail").data("kendoGrid").
    //    dataSource.read(
    //        {
    //            sHospID: pHospID,
    //            sPLMRowid: ""
    //        });

    document.getElementById("lbCopyText").innerText = "";   
}

function click_event(sCase) {
    //alert(sCase);

    switch (sCase) {
        case 'lisdate':            
            document.getElementById('lpdg').style.display = 'block';
            document.getElementById('lplg').style.display = 'none';
            document.getElementById('divPLDType').style.display = 'block';
            document.getElementById('divSelectPLDType').style.display = 'block';
            document.getElementById('divAppPush').style.display = 'block';
            break;
        case 'lisclass':
            document.getElementById('lpdg').style.display = 'none';
            document.getElementById('lplg').style.display = 'block';
            document.getElementById('divPLDType').style.display = 'none';
            document.getElementById('divSelectPLDType').style.display = 'none';
            document.getElementById('divAppPush').style.display = 'none';
            break;
        case 'lischart':
            document.getElementById('lpdg').style.display = 'none';
            document.getElementById('lplg').style.display = 'block';
            document.getElementById('divPLDType').style.display = 'none';
            document.getElementById('divSelectPLDType').style.display = 'none';
            document.getElementById('divAppPush').style.display = 'none';

            break;
    }
}

function onChange(e) {
    ////alert("[" + this.selectedKeyNames().join(", ") + "]");    
    var class_grid = $("#grid_lisPatientLaboratoryDateGroup").data("kendoGrid");
    var class_selected = class_grid.dataItem(class_grid.select());
    var sPLMApplyDate = formatDate(class_selected.PLMApplyDate);
    //alert(class_selected.PLMApplyDate);

    var rows = e.sender.select();
    var selectString = '';    
    if (bGVar == false) {
        rows.each(function (e) {
            if (selectString == '') {
                selectString = '{"data":[{"PLDCode":"PLMApplyDate","PLDValue":"' + sPLMApplyDate + '" },';
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
                url: "/ZMLisSys/GetData/GetLisTemplate",
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

    }

    //document.getElementById("lbCopyText").innerText = selectString;
}

// #region lisPatientLaboratoryClassGroup_onChange-檢驗表列式明細左側檢驗清單點選事件
function lisPatientLaboratoryClassGroup_onChange(e) {
    // 清除 Clipboard 資料
    document.getElementById("lbClassCopyText").innerHTML = '';

    var rows = e.sender.select();
    var selectString = '';
    var chart_selected = [];
    var grid = $("#grid_lisPatientLaboratoryClassGroup").data("kendoGrid");
    var data  = grid.dataSource.data();
    var totalNumber = data.length;
    for (var i = 0; i < totalNumber; i++) {
        var currentDataItem = data[i];
        document.getElementById('div_' + currentDataItem.HLIRowid).style.display = 'none';        
    }
    
    rows.each(function (e) {
        if (selectString == '') {
            selectString = '{"data":[';
        }
        //var grid = $("#grid_lisPatientLaboratoryClassGroup").data("kendoGrid");
        var dataItem = grid.dataItem(this);

        // 取得資料放置到 Json
        selectString += '{"PLDCode":"' + dataItem.PLDCode + '" },';
        //selectString += dataItem.PLDName + "：" + dataItem.PLDValue + ",";

        // 取得資料放置到 Array 給 chart 使用
        chart_selected.push(dataItem.HLIRowid);
        document.getElementById('div_' + dataItem.HLIRowid).style.display = 'block';
    })   

    //chart_selected.forEach(items => {
    //    //alert('div_' + items);
    //    //document.getElementById("'" + items + "'").style.display = "block";
    //    document.getElementById('div_' + items).style.display = 'block';
    //})

    if (selectString != '') {
        var selectString = selectString.slice(0, -1);
        //selectString += ']'
        selectString += ']}';

        // 取得病人的 id
        var grid_patient = $('#grid_Patient').data('kendoGrid');
        var patient_selectedItem = grid_patient.dataItem(grid_patient.select());
        pPTRowid = patient_selectedItem.id;

        if (pPTRowid == null) {
            pPTRowid = gPTRowid;
        }
        //alert(patient_selectedItem.id);

        // https://www.telerik.com/forums/to-add-new-column-in-grid-dynamically
        // 把Json資料post至 controller 並且重繪 Grid

        var grid_lbcd = $('#grid_lisBoardClassDetail').data('kendoGrid');

        //var ArrayData = [{
        //        'field': '檢驗日期'
        //    },
        //    {
        //        'field': 'Diastolic B.P.'
        //    },
        //    {
        //        'field': 'Lymphocyte'
        //    }];
        var ArrayData = [{
            'field': ''
        }];
        
        var options = kendo.stringify(grid_lbcd.getOptions());
        var savedOptions = JSON.parse(options);       
        //savedOptions.columns.splice(0, 3, { field: '檢驗日期' }, { field: 'Hemoglobin' }, { field: 'Lymphocyte' });        
        savedOptions.columns.splice(0, 1000,
            ArrayData.forEach(element => {
                field: element.field
            })
            //{ field: ArrayData[0].field }, { field: ArrayData[1].field }, { field: ArrayData[2].field }            
        );        
        grid_lbcd.setOptions(savedOptions);

        $('#grid_lisBoardClassDetail').data('kendoGrid').dataSource.read({
            HospID: null,
            sPTRowid: pPTRowid,
            sJsonString: selectString
        });

        // 處理圖表
        chart_selected.forEach(items => {
            //alert('div_' + items);
            //document.getElementById("'" + items + "'").style.display = "block";
            document.getElementById('div_' + items).style.display = 'block';
            var chart = $("#chart_" + items).data("kendoChart");
            chart.dataSource.read({
                sHospID: null,
                sPTRowid: pPTRowid,
                sHLIRowid: items
            });
            chart.refresh();
        });
    }
    else {
        //https://www.itranslater.com/qa/details/2126821039865857024
        //var dataSource = new kendo.data.DataSource();        
        $('#grid_lisBoardClassDetail').data('kendoGrid').dataSource.data([{}]);
        //$('#grid_lisBoardClassDetail').data('kendoGrid').setDataSource();
        //$('#grid_lisBoardClassDetail').data('kendoGrid').destroy();
    };
}
// #endregion

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

function resizeClassDetailGrid(e) {
    for (var i = 0; i < this.columns.length; i++) {
        this.autoFitColumn(i);
    }

    //$('#grid_lisBoardClassDetail table tr').dblclick(function (e) {
    //    //selectedRows.rows(row => {
    //    //    columns.forEach(column => {
    //    //        alert(row[column]);
    //    //    });
    //    //});        
    //});
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

function formatDate(date) {
    var d = new Date(date),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();

    if (month.length < 2)
        month = '0' + month;
    if (day.length < 2)
        day = '0' + day;

    return [year, month, day].join('-');
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