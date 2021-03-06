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
    using System.ComponentModel.DataAnnotations;

    public partial class ST_InStock
    {
        public int serno { get; set; }
        public System.Guid guid { get; set; }

        [Display(Name = "日期")]
        public System.DateTime rec_date { get; set; }

        [Display(Name = "850cc庫存量")]
        public int count_850 { get; set; }
        public int use_850 { get; set; }
        public int add_850 { get; set; }

        [Display(Name = "700cc庫存量")]
        public int count_700 { get; set; }
        public int use_700 { get; set; }
        public int add_700 { get; set; }

        [Display(Name = "點心盒庫存量")]
        public int count_meal { get; set; }
        public int use_meal { get; set; }
        public int add_meal { get; set; }

        [Display(Name = "便當盒庫存量")]
        public int count_box { get; set; }
        public int use_box { get; set; }

        public int add_box { get; set; }

        [Display(Name = "備註")]
        public string remark { get; set; }
        public Nullable<System.DateTime> deleted_at { get; set; }
        public int del_yn { get; set; }

        public string display_Date { get { return rec_date.ToString("yyyy-MM-dd") + " " + rec_date.DayOfWeek; } }
    }
}
