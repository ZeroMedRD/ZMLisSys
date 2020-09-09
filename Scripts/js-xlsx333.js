/*
   FileReader共有4种读取方法：
   1.readAsArrayBuffer(file)：将文件读取为ArrayBuffer。
   2.readAsBinaryString(file)：将文件读取为二进制字符串
   3.readAsDataURL(file)：将文件读取为Data URL
   4.readAsText(file, [encoding])：将文件读取为文本，encoding缺省值为'UTF-8'
   */
var wb;//读取完成的数据
var rABS = false; //是否将文件读取为二进制字符串
// var replacer = null;
var aa = [];

function replacer(key, value) {
    // console.log(key + ':' + value);
    return value;
}


function importf(obj) {//导入
    if (!obj.files) {
        return;
    }
    var f = obj.files[0];
    var reader = new FileReader();
    reader.onload = function (e) {
        var data = e.target.result;
        // console.log(data);
        if (rABS) {
            wb = XLSX.read(btoa(fixdata(data)), {//手动转化
                type: 'base64'
            });
        } else {
            wb = XLSX.read(data, {
                type: 'binary'
            });
        }
        var xlsxData = XLSX.utils.sheet_to_json(wb.Sheets[wb.SheetNames[0]]);
        var list1 = getList1(wb);
        xlsxData = AddXlsxData(xlsxData, list1);
        //wb.SheetNames[0]是获取Sheets中第一个Sheet的名字
        //wb.Sheets[Sheet名]获取第一个Sheet的数据
        //aa = JSON.stringify(xlsxData);//array/object to json
        document.getElementById("demo").innerHTML = JSON.stringify(xlsxData, replacer, '\t');
        //document.getElementById("demo").innerHTML = JSON.stringify(XLSX.utils.sheet_to_json(wb.Sheets[wb.SheetNames[0]]));
        //for (x in obj[0]) {
        //    //document.getElementById("demo").innerHTML += x + "<br>";
        //    $('#demo').append(x + "<br>");
        //}

    };
    if (rABS) {
        reader.readAsArrayBuffer(f);
    } else {
        reader.readAsBinaryString(f);
    }
}

//function fixdata(data) { //文件流转BinaryString
//    var o = "",
//        l = 0,
//        w = 10240;
//    for (; l < data.byteLength / w; ++l) o += String.fromCharCode.apply(null, new Uint8Array(data.slice(l * w, l * w + w)));
//    o += String.fromCharCode.apply(null, new Uint8Array(data.slice(l * w)));
//    return o;
//}

// 获取excel第一行的内容
function getList1(wb) {
    var wbData = wb.Sheets[wb.SheetNames[0]]; // 读取的excel单元格内容
    var re = /^[A-Z]1$/; // 匹配excel第一行的内容
    var arr1 = [];
    for (var key in wbData) { // excel第一行内容赋值给数组
        if (wbData.hasOwnProperty(key)) {
            if (re.test(key)) {
                arr1.push(wbData[key].h);
            }
        }
    }
    return arr1;
}

// 增加对应字段空白内容
function AddXlsxData(xlsxData, list1) {
    var addData = null; // 空白字段替换值
    for (let i = 0; i < xlsxData.length; i++) { // 要被JSON的数组
        for (let j = 0; j < list1.length; j++) { // excel第一行内容
            if (!xlsxData[i][list1[j]]) {
                xlsxData[i][list1[j]] = addData;
            }
        }
    }
    return xlsxData;
}