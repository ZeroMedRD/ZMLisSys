using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ZMLISSys.ViewModels
{
    #region ViewModel_LaboratoryClass (laboratory_class)-檢驗檢查分類 (antifat v1.0)
    public class ViewModel_LaboratoryClass
    {
        [Display(Name = "資料序號")]
        public string id { get; set; }

        [Display(Name = "分類代碼")]
        public string code { get; set; }
        [Display(Name = "分類簡碼")]
        public string nick { get; set; }

        [Display(Name = "分類名稱")]
        public string name { get; set; }
    }
    #endregion

    #region ViewModel_LaboratoryItem (laboratory_item)-檢驗檢查項目 (antifat v1.0)
    public class ViewModel_LaboratoryItem
    {
        [Display(Name = "資料序號")]
        public string id { get; set; }

        [Display(Name = "檢驗分類資料序號")]
        public string laboratoryClass_id { get; set; }

        [Display(Name = "檢驗分類")]
        public string laboratoryClass_name { get; set; }

        [Display(Name = "項目代碼")]
        public string code { get; set; }

        [Display(Name = "項目簡碼")]
        public string nick { get; set; }

        [Display(Name = "項目名稱")]
        public string name { get; set; }

        [Display(Name = "中文名稱")]
        public string chnName { get; set; }

        [Display(Name = "單位")]
        public string unit { get; set; }

        [Display(Name = "標準值")]
        public string standard { get; set; }

        [Display(Name = "健保碼")]
        public string nhi_code { get; set; }

        [Display(Name = "檢驗所")]
        public string laboratory_clinic_code { get; set; }

        [Display(Name = "檢驗所")]
        public string laboratory_clinic_name { get; set; }
    }
    #endregion

    #region ViewModel_LisLaboratoryClass (LisLaboratoryClass)-檢驗檢查分類 (ZMLIS)
    public class ViewModel_LisLaboratoryClass
    {
        [Display(Name = "資料序號")]
        public string LLCRowid { get; set; }

        [Display(Name = "分類代碼")]
        public string LLCCode { get; set; }

        [Display(Name = "分類中文名稱")]
        public string LLCTrdCName { get; set; }

        [Display(Name = "分類英文名稱")]
        public string LLCEngName { get; set; }
    }
    #endregion

    #region ViewModel_LisLaboratoryItem (LisLaboratoryItem)-檢驗檢查項目 (ZMLIS)
    public class ViewModel_LisLaboratoryItem
    {
        [Display(Name = "資料序號")]
        public string LLIRowid { get; set; }

        [Display(Name = "分類資料序號")]
        public string LLCRowid { get; set; }

        [Display(Name = "分類名稱")]
        public string LLCTrdCName { get; set; }

        [Display(Name = "檢驗代碼")]
        public string LLINhiCode { get; set; }

        [Display(Name = "中文名稱")]
        public string LLITrdCName { get; set; }

        [Display(Name = "英文名稱")]
        public string LLIEngName { get; set; }

        [Display(Name = "報告名稱")]
        public string LLIRptName { get; set; }

        [Display(Name = "支付點數")]
        public float LLINhiCost { get; set; }

        // 檢驗檢查項目類別 (自費 or 健保)
        [Display(Name = "費用類別")]
        public string LLICostType { get; set; }

        [Display(Name = "費用類別")]
        public string LLICostTypeName { get; set; }

        [Display(Name = "項目類別")]
        public string LLIType { get; set; }

        [Display(Name = "項目類別")]
        public string LLITypeName { get; set; }

        [Display(Name = "單位")]
        public string LLIUnit { get; set; }

        [Display(Name = "歸屬類別")]
        public string LLIKind { get; set; }

        [Display(Name = "歸屬類別")]
        public string LLIKindName { get; set; }

        [Display(Name = "男性標準上限值")]
        public float LLIUp_Male { get; set; }

        [Display(Name = "男性標準下限值")]
        public float LLILo_Male { get; set; }

        [Display(Name = "女性標準上限值")]
        public float LLIUp_Female { get; set; }

        [Display(Name = "女性標準下限值")]
        public float LLILo_Female { get; set; }

        [Display(Name = "轉換率")]
        public string LLIConvertRate { get; set; }

        [Display(Name = "轉換單位")]
        public string LLIConvertUnit { get; set; }
    }
    #endregion

    #region ViewModel_LisLaboratoryMaster (LisLaboratoryMaster)-檢驗所結構主檔 (ZMLIS)
    public class ViewModel_LisLaboratoryMaster
    {
        [Display(Name = "檢驗所資料序號")]
        public string LLMRowid { get; set; }

        [Display(Name = "檢驗所名稱")]
        public string LLMName { get; set; }

        [Display(Name = "檢驗格式")]
        public string LLMFormat { get; set; }
    }
    #endregion

    #region ViewModel_LisLaboratoryDetail (LisLaboratoryDetail)-檢驗所結構明細檔 (ZMLIS)
    public class ViewModel_LisLaboratoryDetail        
    {
        [Display(Name = "檢驗所結構序號")]
        public string LLDRowid { get; set; }

        [Display(Name = "檢驗所資料序號")]
        public string LLMRowid { get; set; }

        [Display(Name = "檢驗所資料序號")]
        public string LLDCode { get; set; }
                
        [Display(Name = "名稱")]
        public string LLDFieldName { get; set; }

        [Display(Name = "型態")]
        public string LLDFieldType { get; set; }

        [Display(Name = "型態")]
        public string LLDFieldTypeName { get; set; }

        [Display(Name = "長度")]
        public int LLDFieldLength { get; set; }

        [Display(Name = "小數點")]
        public int LLDFieldFloatLength { get; set; }

        [Display(Name = "分類")]
        public string LLDFieldKind { get; set; }

        [Display(Name = "分類")]
        public string LLDFieldKindName { get; set; }

        [Display(Name = "備註")]
        public string LLDFieldMemo { get; set; }

        [Display(Name = "起始長度")]
        public int LLDTextStartPos { get; set; }

        [Display(Name = "終止長度")]
        public int LLDTextEndPos { get; set; }

        [Display(Name = "序號")]
        public int LLDSeqno { get; set; }

        [Display(Name = "對應欄位資料序號")]
        public string LLDMappingField { get; set; }

        [Display(Name = "對應欄位")]
        public string LLDMappingFieldName { get; set; }
    }
    #endregion

    #region ViewModel_HospitalLaboratoryItem (HospitalLaboratoryItem)-醫療院所檢驗檢查項目
    public class ViewModel_HospitalLaboratoryItem
    {
        [Display(Name = "醫事機構")]
        public string HospID { get; set; }

        //[Display(Name = "選取")]
        //public bool IsChecked { get; set; }

        #region his.lisHospitalLaboratoryItem
        [Display(Name = "醫療院所檢驗檢查資料序號")]
        public string HLIRowid { get; set; }

        [Display(Name = "檢驗代碼")]
        public string HLICode { get; set; }

        [Display(Name = "檢驗所檢驗名稱")]
        public string HLIName { get; set; }

        [Display(Name = "檢驗所檢驗名稱別名")]
        public string HLITitleName { get; set; }

        [Display(Name = "顯示值範圍")]
        public string HLIDisplayRange { get; set; }        

        [Display(Name = "檢驗對應資料序號")]
        public string LLIRowid { get; set; }

        [Display(Name = "檢驗介接資料序號")]
        public string LLISubRowid { get; set; }
        #endregion

        #region ZMLIS.lisLaboratoryItem
        [Display(Name = "檢驗代碼")]
        public string LLINhiCode { get; set; }

        [Display(Name = "檢驗名稱")]
        public string LLITrdCName01 { get; set; }

        [Display(Name = "檢驗介接名稱")]
        public string LLITrdCName02 { get; set; }

        [Display(Name = "英文名稱")]
        public string LLIEngName { get; set; }

        [Display(Name = "支付點數")]
        public float LLINhiCost { get; set; }        

        [Display(Name = "單位")]
        public string LLIUnit { get; set; }

        [Display(Name = "男高標")]
        public float LLIUp_Male { get; set; }

        [Display(Name = "男低標")]
        public float LLILo_Male { get; set; }      

        [Display(Name = "女高標")]
        public float LLIUp_Female { get; set; }

        [Display(Name = "女低標")]
        public float LLILo_Female { get; set; }

        [Display(Name = "轉換率")]
        public string LLIConvertRate { get; set; }

        [Display(Name = "轉換單位")]
        public string LLIConvertUnit { get; set; }

        [Display(Name = "順序")]
        public int HLISeqno { get; set; }
        #endregion
    }
    #endregion

    #region ViewModel_SysHospital (SysHospital)-醫事機構資料表 (65.52.165.109/Database:ZMCMSv2)
    public class ViewModel_SysHospital
    {
        public string HospRowid { get; set; }

        [Display(Name = "醫事機構代碼")]
        public string HospID { get; set; }

        [Display(Name = "醫事機構名稱")]
        public string HospName { get; set; }

        [Display(Name = "醫院地址")]
        public string HospAddress { get; set; }
    }
    #endregion

    #region ViewModel_lisPatientLaboratoryMaster (lisPatientLaboratoryMaster)-病人檢驗表頭資料 (65.52.165.109/Database:his1234567890)
    public class ViewModel_lisPatientLaboratoryMaster
    {
        [Display(Name = "檢驗表頭序號")]
        public string PLMRowid { get; set; }

        [Display(Name = "病人資料序號")]
        public string PLMPTRowid { get; set; }

        [Display(Name = "身份證字號")]
        public string PLMPTIdno { get; set; }

        [Display(Name = "姓名")]
        public string PLMPTName { get; set; }

        [Display(Name = "出生日期")]
        public string PLMPTBirthday { get; set; }

        [Display(Name = "性別")]
        public string PLMPTGender { get; set; }

        [Display(Name = "原病歷號碼")]
        public string PLMPTCode { get; set; }

        [Display(Name = "原就醫日期")]
        public DateTime? PLMClinicDate { get; set; }

        [Display(Name = "開單日期")]
        public DateTime? PLMApplyDate { get; set; }

        [Display(Name = "開單時間")]
        public string PLMApplyTime { get; set; }

        [Display(Name = "檢驗日期")]
        public DateTime? PLMInspDate { get; set; }

        [Display(Name = "檢驗時間")]
        public string PLMInspTime { get; set; }

        [Display(Name = "報告日期")]
        public DateTime? PLMReportDate { get; set; }

        [Display(Name = "報告時間")]
        public string PLMReportTime { get; set; }

        [Display(Name = "檢驗單號")]
        public string PLMSNo { get; set; }

        [Display(Name = "看診序號")]
        public string PLMReqno { get; set; }

        [Display(Name = "APP")]
        public bool APPFlag { get; set; }
    }
    #endregion

    #region ViewModel_lisPatientLaboratoryDetail (lisPatientLaboratoryMaster)-病人檢驗表身資料 (65.52.165.109/Database:his1234567890)
    public class ViewModel_lisPatientLaboratoryDetail
    {
        [Display(Name = "檢驗表身序號")]
        public string PLDRowid { get; set; }

        [Display(Name = "檢驗表頭序號")]
        public string PLMRowid { get; set; }

        [Display(Name = "檢驗檢查資料序號")]
        public string HLIRowid { get; set; }

        [Display(Name = "順序")]
        public int HLISeqno { get; set; }

        [Display(Name = "檢驗代碼")]
        public string PLDCode { get; set; }

        [Display(Name = "檢驗名稱")]
        public string PLDName { get; set; }
        
        [Display(Name = "檢驗值")]
        public string PLDValue { get; set; }

        //[Display(Name = "檢驗值")]
        //public string PLDStrValue { get; set; }

        [Display(Name = "異常標記")]
        public string PLDType { get; set; }

        [Display(Name = "檢驗單位")]
        public string PLDUnit { get; set; }

        [Display(Name = "檢驗註記")]
        public string PLDMemo { get; set; }

        [Display(Name = "資料順序")]
        public int PLDSeqno { get; set; }
    }
    #endregion

    #region ViewModel_lisPatientLaboratoryDateGroup-病人檢驗依日期群組的檢驗日期表 (65.52.165.109/Database:his1234567890)
    public class ViewModel_lisPatientLaboratoryDateGroup
    {
        //lplm.PLMRowid, lplm.PTRowid, lplm.PLMPTCode, lplm.PLMApplyDate
        [Display(Name = "醫事機構資料序號")]
        public string HospID { get; set; }

        [Display(Name = "檢驗資料序號")]
        public string PLMRowid { get; set; }

        [Display(Name = "病人資料序號")]
        public string PTRowid { get; set; }

        [Display(Name = "病人身份證號")]
        public string PTIdno { get; set; }

        [Display(Name = "病歷號碼")]
        public string PLMPTCode { get; set; }

        [Display(Name = "檢驗單號")]
        public string PLMSNo { get; set; }

        [Display(Name = "檢驗日期")]
        public DateTime PLMApplyDate { get; set; }
    }
    #endregion

    #region ViewModel_lisPatientLaboratoryClassGroup (lisPatientLaboratoryMaster)-病人檢驗依日期群組的檢驗日期表 (65.52.165.109/Database:his1234567890)
    public class ViewModel_lisPatientLaboratoryClassGroup
    {
        //lplm.PLMRowid, lplm.PTRowid, lplm.PLMPTCode, lplm.PLMApplyDate
        [Display(Name = "檢驗項目資料序號")]
        public string HLIRowid { get; set; }

        [Display(Name = "檢驗代碼")]
        public string PLDCode { get; set; }

        [Display(Name = "檢驗名稱")]
        public string HLIName { get; set; }        
    }
    #endregion

    #region ViewModel_SysUploadServer (SysUploadServer)-資料上傳主機狀態 (65.52.165.109/Database:ZMCMSv2)    
    public class ViewModel_SysUploadServer
    {
        [Display(Name = "資料序號")]
        public string USRowid { get; set; }

        [Display(Name = "醫事機構資料序號")]
        public string USHospRowid { get; set; }

        [Display(Name = "醫事機構代碼")]
        public string USHospID { get; set; }

        [Display(Name = "上傳檔案名稱")]
        public string USLoadFilename { get; set; }

        [Display(Name = "上傳日期")]
        public DateTime? USLoadDateTime { get; set; }

        [Display(Name = "預訂執行日期時間")]
        public DateTime? USBookingDatetime { get; set; }

        [Display(Name = "完成日期時間")]
        public DateTime? USFinishDateTime { get; set; }

        [Display(Name = "狀態")]
        public string USServerStatus { get; set; }
        // USServerStatus 定義，字母大寫:
        // S: 待處理
        // P: 處理中
        // E: 已完成
        // F: 處理失敗

        [Display(Name = "上傳資料總筆數")]
        public int USRecordCount { get; set; }

        [Display(Name = "主機執行類別")]
        public string USType { get; set; }
        // USType 定義，字母大寫:
        // H: 申報上傳 (使用者透過介面上載的檔案)
        // L: 檢驗上傳 (使用者透過介面上載的檔案)
        // A: 檢驗上傳 (使用者透過指定時間方式讓主機 Agent程式依時間到期時自動嫁接第三方執行自動匯入)
        // P: 檢驗上傳 (主機 Agent程式依後台管理模式指定時間到期時自動嫁接第三方執行自動匯入)
    }
    #endregion

    #region ViewModel_lisLaboratorySchedule (lisLaboratorySchedule)-檢驗API自動上傳排程設定 (65.52.165.109/Database:ZMCMSv2)    
    public class ViewModel_lisLaboratorySchedule
    {
        [Display(Name = "資料序號")]
        public string LLSRowid { get; set; }

        [Display(Name = "醫事機構資料序號")]
        public string HospRowid { get; set; }

        [Display(Name = "檢驗嫁接資料序號")]
        public string CBDRowid { get; set; }

        [Display(Name = "檢驗嫁接名稱")]
        public string CBDDescription { get; set; }

        [Display(Name = "主機連線位置")]
        public string LLSTcpIp { get; set; }

        [Display(Name = "登入帳號１")]
        public string LLSLogin01 { get; set; }

        [Display(Name = "登入帳號２")]
        public string LLSLogin02 { get; set; }

        [Display(Name = "登入帳號３")]
        public string LLSLogin03 { get; set; }

        [Display(Name = "登入密碼")]
        public string LLSPassword { get; set; }

        [Display(Name = "API連接指令")]
        public string LLSAPIUrl { get; set; }

        [Display(Name = "星期一")]
        public bool LLSMon { get; set; }

        [Display(Name = "星期二")]
        public bool LLSTue { get; set; }

        [Display(Name = "星期三")]
        public bool LLSWed { get; set; }

        [Display(Name = "星期四")]
        public bool LLSThu { get; set; }

        [Display(Name = "星期五")]
        public bool LLSFri { get; set; }

        [Display(Name = "星期六")]
        public bool LLSSat { get; set; }

        [Display(Name = "星期日")]
        public bool LLSSun { get; set; }

        [Display(Name = "排程時間１")]
        public DateTime? LLSTime01 { get; set; }

        [Display(Name = "排程時間２")]
        public DateTime? LLSTime02 { get; set; }

        [Display(Name = "排程時間３")]
        public DateTime? LLSTime03 { get; set; }

        [Display(Name = "排程時間４")]
        public DateTime? LLSTime04 { get; set; }

        [Display(Name = "排程時間５")]
        public DateTime? LLSTime05 { get; set; }

        [Display(Name = "排程描述說明")]
        public string LLSDescription { get; set; }

        [Display(Name = "排程完成後發送訊息位置")]
        public string LLSReceiveMail { get; set; }
    }
    #endregion

    #region ViewModel_Patient (Patient)-病人基本資料表 (zmcms.cloud/Database:his1234567890)
    public class ViewModel_Patient
    {
        [Display(Name = "資料序號")]
        public string id { get; set; }

        [Display(Name = "醫事機構代碼")]
        public string hospID { get; set; }

        [Display(Name = "身份證字號")]
        public string strIdno { get; set; }

        [Display(Name = "病歷號碼")]
        public string strUserAccount { get; set; }

        [Display(Name = "原HIS病歷號碼")]
        public int intRsRecno { get; set; }

        [Display(Name = "姓名")]
        public string strName { get; set; }

        [Display(Name = "電話")]
        public string strTel { get; set; }

        [Display(Name = "行動電話")]
        public string strCell { get; set; }

        [Display(Name = "地址")]
        public string strAddr { get; set; }

        [Display(Name = "收案狀態")]
        public string lChk { get; set; }
    }
    #endregion

    #region ViewModel_LISChart 病人檢驗數值資料 (65.52.165.109/Database:his1234567890)
    public class ViewModel_LISChart
    {
        [Display(Name = "開單日期")]
        public string PLMApplyDate { get; set; }
        
        [Display(Name = "檢驗值")]
        public decimal PLDValue { get; set; }        
    }
    #endregion
}