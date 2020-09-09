/*
    FileReader共有4種讀取方法：
    1.readAsArrayBuffer(file)：將檔案讀取為ArrayBuffer。
    2.readAsBinaryString(file)：將檔案讀取為二進位制字串
    3.readAsDataURL(file)：將檔案讀取為Data URL
    4.readAsText(file, [encoding])：將檔案讀取為文字，encoding預設值為'UTF-8'
                 */
var wb;//讀取完成的資料
var aa = [];
var text = [];
var rABS = false; //是否將檔案讀取為二進位制字串

function importExcel(obj) {//匯入
    if (!obj.files) {
        return;
    }
    const IMPORTFILE_MAXSIZE = 1 * 2048;//這裡可以自定義控制匯入檔案大小
    var suffix = obj.files[0].name.split(".")[1]
    if (suffix != 'xls' && suffix != 'xlsx') {
        alert('匯入的檔案格式不正確!')
        return
    }
    if (obj.files[0].size / 1024 > IMPORTFILE_MAXSIZE) {
        alert('匯入的表格檔案不能大於2M')
        return
    }
    var f = obj.files[0];
    var reader = new FileReader();
    reader.onload = function (e) {
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
        aa = JSON.stringify(XLSX.utils.sheet_to_json(wb.Sheets[wb.SheetNames[0]]));
        var u = eval('(' + aa + ')');
        document.getElementById("demo").innerHTML = JSON.stringify(XLSX.utils.sheet_to_json(wb.Sheets[wb.SheetNames[0]]));
        //獲取表格中為address的那列存入text中
        for (var i = 0; i < u.length; i++) {
            text.push(u[i].address);
        }

    };
    if (rABS) {
        reader.readAsArrayBuffer(f);
    } else {
        reader.readAsBinaryString(f);
    }
}