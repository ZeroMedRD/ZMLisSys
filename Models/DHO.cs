using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZMLISSys.Models
{
    #region 設定上傳檢驗 Json 格式
    public class LISJSONDATA
    {
        public object data { get; set; }
    }

    public class LISDATA
    {
        public string access_token { get; set; }
        public string nrange { get; set; }
        public List<USER> users { get; set; }
    }

    public class USER
    {
        public string dho_uid { get; set; }
        //public string email { get; set; }
        //public string country_code { get; set; }
        //public string phone { get; set; }
        //public int reg_type { get; set; }            
        public object general { get; set; }
        public List<EXAMS> exams { get; set; }
    }

    public class GENERAL
    {
        public string birthday { get; set; }
        public int gender { get; set; }
        public int marriage { get; set; }
        public int give_birth { get; set; }
        public int smoking { get; set; }
        public int alcohol { get; set; }
        public int betelnut_chewing { get; set; }
        public int coffee { get; set; }
        public int sleep_hrs { get; set; }
        public int dailywork_hrs { get; set; }
        public string personal_history { get; set; }
        public string medication_supplement { get; set; }
        public string family_history { get; set; }
        public int blood_type { get; set; }
        public int update_time { get; set; }
        //public int? bloodtype_rhfactor { get; set; }
    }

    public class EXAMS
    {
        public int resource_id { get; set; }
        public string exam_name { get; set; }
        public string exam_date { get; set; }
        public List<EXAMITEMS> exam_items { get; set; }
    }

    public class EXAMITEMS
    {
        public string exam_item_id { get; set; }
        public string prefered_unit { get; set; }
        public string si_value { get; set; }
        public string cu_value { get; set; }
        public string cu_string { get; set; }
        public string si_string { get; set; }
    }

    public class GetToken1
    {
        public string status { get; set; }
        public string message { get; set; }

        public string error_code { get; set; }

        public object data { get; set; }
    }
    #endregion
}