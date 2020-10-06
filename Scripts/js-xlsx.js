/*
  FileReader共有4種讀取方法：
  1.readAsArrayBuffer(file)：將檔案讀取為ArrayBuffer。
  2.readAsBinaryString(file)：將檔案讀取為二進位制字串
  3.readAsDataURL(file)：將檔案讀取為Data URL
  4.readAsText(file, [encoding])：將檔案讀取為文字，encoding預設值為'UTF-8'
               */
var x;
var aa = [];
var wb;//讀取完成的資料
var rABS = false; //是否將檔案讀取為二進位制字串



function replacer(key, value) {
    // console.log(key + ':' + value);
    return value;
}

function importExcel(obj) {//匯入
    if (!obj.files) {
        return;
    }
    const IMPORTFILE_MAXSIZE = 1 * 2048;//這裡可以自定義控制匯入檔案大小
    var suffix = obj.files[0].name.split(".")[1]
    if (suffix != 'xls' && suffix != 'xlsx' && suffix != 'txt' && suffix != 'json' ) {
        alert('匯入的檔案格式不正確!')
        return
    }
    if (obj.files[0].size / 2048 > IMPORTFILE_MAXSIZE) {
        alert('匯入的表格檔案不能大於2M')
        return
    }
    //取得上傳第一個檔案
    var f = obj.files[0];
    //使用FileReader讀檔
    var reader = new FileReader();
    //onload觸發事件
    reader.onload = function (e) {
        //對象內資料
        var data = e.target.result;  
        if (rABS) {
            wb = XLSX.read(btoa(fixdata(data)), {//手動轉化
                type: 'base64'
            });
        } else {
            wb = XLSX.read(data, {
                type: 'binary'
            });
        }   
        //wb.SheetNames[0]是獲取Sheets中第一個Sheet的名字
        //wb.Sheets[Sheet名]獲取第一個Sheet的資料
        aa = JSON.stringify(XLSX.utils.sheet_to_row_object_array(wb.Sheets[wb.SheetNames[0]]));//array/object to json
        //var obj = JSON.parse(XLSX.utils.sheet_to_json(wb.Sheets[wb.SheetNames[0]], sheet2JSONOpts));//json to array/object
        var obj = JSON.parse(aa);
        document.getElementById("demo").innerHTML = aa;

        alert(aa);
        //var cc = $('#demo').val();
        //alert(cc);
        //循環遍歷對象key屬性
        for (x in obj[0]) {
            $('#demo').append("<br>"+ x );
        } 
    };
    if (rABS) {
        reader.readAsArrayBuffer(f);
    } else {
        reader.readAsBinaryString(f);
    }

    function fixdata(data) { //文件流转BinaryString
        var o = "",
            l = 0,
            w = 10240;
        for (; l < data.byteLength / w; ++l) o += String.fromCharCode.apply(null, new Uint8Array(data.slice(l * w, l * w + w)));
        o += String.fromCharCode.apply(null, new Uint8Array(data.slice(l * w)));
        return o;
    }

}

function GetData() {
    //var str = 'hello';
    //var age = 87;
    //var aaa = str + age;
    //$('#demo2').html(aaa);

    alert(aa);

    ////跨域請求公開資料
    //const cors = 'https://cors-anywhere.herokuapp.com/';
    //const url = 'https://tw.rter.info/capi.php?=1568944322585';
    //$.get(`${cors}${url}`, (res) => {
    //    //console.log(res);
    //    var thisdata = JSON.parse(res);//轉json陣列
    //    console.log(typeof (thisdata));
    //    $('#content111').html(thisdata);
    //})
}

function getFormInfo() {
    //var postData = aa;
    //console.log(params);
    $.ajax({
        //url: '@Url.Action("GetJsonHelper", "Home")',
        url: '/Home/GetJsonHelper',
        type: 'post',
        data: JSON.stringify(aa), 
        cache: false,
        async: true,
        dataType: 'application/json; charset=utf-8',
        success: function (data) {
            console.log(data);
            alert(data);
            //$('.info').html(data);
        },
        error: function (err) {
        }
    });
}


function getFormInfo2() {
    var postData = aa;
    //console.log(params);
    $.ajax({
        url: '/Home/GetJsonHelper',
        type: 'post',
        data: postData,
        cache: false,
        async: true,
        dataType: 'application/json; charset=utf-8',
        success: function (data)
        {
            alert(data);
        },
        error: function (req, status, error)
        {
            alert("error! " + status + error + req);
        } 
    });
}



