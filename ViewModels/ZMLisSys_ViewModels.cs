using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ZMLisSys.ViewModels
{
    public class LisLaboratory_ViewModels
    {
        [Display(Name = "檢驗所資料序號")]
        public string LLRowid { get; set; }

        [Display(Name = "檢驗所名稱")]
        public string LLName { get; set; }

        [Display(Name = "檢驗格式")]
        public string F_format { get; set; }
    }

    public class LisLaboratory_ViewModels_str
    {
        [Display(Name = "檢驗所結構序號")]
        public string Rowid { get; set; }

        [Display(Name = "檢驗所資料序號")]
        public string LLRowid { get; set; }

        [Display(Name = "名稱")]
        public string Field_name { get; set; }

        [Display(Name = "格式")]
        public string Field_type { get; set; }

        [Display(Name = "長度")]
        public string Field_length { get; set; }

        [Display(Name = "表頭表身")]
        public string Field_HB { get; set; }
    }

    public class LisLaboratory_ViewModels_head
    {
        [Display(Name = "檢驗所表頭序號")]
        public string id { get; set; }

        [Display(Name = "檢驗所表頭資料序號")]
        public string LLRowid { get; set; }

        [Display(Name = "檢驗單號")]
        public string w_no { get; set; }

        [Display(Name = "開單日期")]
        public string c_data { get; set; }

        [Display(Name = "檢驗日期")]
        public string s_data { get; set; }

        [Display(Name = "姓名")]
        public string name { get; set; }

        [Display(Name = "性別")]
        public string sex { get; set; }

        [Display(Name = "身分證")]
        public string p_id { get; set; }

        [Display(Name = "生日")]
        public string birth { get; set; }

        [Display(Name = "病歷號")]
        public string c_id { get; set; }

        [Display(Name = "醫事機構代碼")]
        public string h_no { get; set; }
    }

    public class LisLaboratory_ViewModels_body
    {
        [Display(Name = "檢驗所表身序號")]
        public string id { get; set; }

        [Display(Name = "檢驗所表身資料序號")]
        public string LLRowid { get; set; }

        [Display(Name = "檢驗單號")]
        public string w_no { get; set; }

        [Display(Name = "開單日期")]
        public string c_data { get; set; }

        [Display(Name = "檢驗日期")]
        public string s_data { get; set; }

        [Display(Name = "檢驗名稱(中文)")]
        public string item_cname { get; set; }

        [Display(Name = "檢驗名稱(英文)")]
        public string item_ename { get; set; }

        [Display(Name = "健保碼")]
        public string nhi_code { get; set; }

        [Display(Name = "檢驗數值")]
        public string CHD_V { get; set; }

        [Display(Name = "單位")]
        public string c_type { get; set; }

        [Display(Name = "檢驗異常")]
        public string low { get; set; }

        [Display(Name = "檢驗標準")]
        public string high { get; set; }
    }
}