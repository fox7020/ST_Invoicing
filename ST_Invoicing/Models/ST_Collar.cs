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

    public partial class ST_Collar
    {
        public int serno { get; set; }
        public System.Guid guid { get; set; }

        [Required]
        [Display(Name = "提領日期")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public System.DateTime collar_date { get; set; }
        public System.Guid material_guid { get; set; }

        [Required]
        [Range(0, 9999, ErrorMessage = "數值不可小於0")]
        [System.Web.Mvc.Remote("CheckStockCount", "ST_Collar", ErrorMessage = "提領數量大於庫存量", AdditionalFields = "material_name")]
        [Display(Name = "數量")]
        public double collar_count { get; set; }

        [Display(Name = "備註")]
        public string remark { get; set; }
        public Nullable<System.DateTime> deleted_at { get; set; }
        public int del_yn { get; set; }
        public System.Guid emp_guid { get; set; }

        [Required]
        [ScaffoldColumn(false)]
        [Display(Name = "原物料名稱")]
        public string material_name { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "提領員工")]
        public string emp_name { get; set; }
    }
}
