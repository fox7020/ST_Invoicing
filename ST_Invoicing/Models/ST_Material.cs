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

    public partial class ST_Material
    {
        public int serno { get; set; }

        public System.Guid guid { get; set; }


        [Display(Name = "原物料名稱")]
        [Required]
        [System.Web.Mvc.Remote("CheckUniItem", "ST_Material", ErrorMessage = "此名稱已輸入過", AdditionalFields = "serno")]
        [MaxLength(10, ErrorMessage = "原物料名稱長度不可大於{1}個字元")]
        public string item_name { get; set; }


        [Display(Name = "單位")]
        [Required]
        public string utem_unit { get; set; }


        [Display(Name = "種類")]
        [Required]
        public string item_species { get; set; }

        [Display(Name = "備註")]
        public string reamrk { get; set; }

        public Nullable<System.DateTime> deleted_at { get; set; }

        public int del_yn { get; set; }
    }
}
