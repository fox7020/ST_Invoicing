//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace ST_Invoicing.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ST_SurplusDay
    {
        public int serno { get; set; }
        public System.Guid guid { get; set; }
        public System.DateTime rec_date { get; set; }
        public int turnover { get; set; }
        public int count_850 { get; set; }
        public int count_700 { get; set; }
        public int count_meal { get; set; }
        public double count_rice { get; set; }
        public double count_soup { get; set; }
        public string remark { get; set; }
        public Nullable<System.DateTime> deleted_at { get; set; }
        public int del_yn { get; set; }
        public System.Guid emp_guid { get; set; }
    }
}
