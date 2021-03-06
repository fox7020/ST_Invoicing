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

    public partial class ST_Emp
    {
        public int serno { get; set; }
        public System.Guid guid { get; set; }

        [Required]
        [MaxLength(10, ErrorMessage = "姓名長度不可大於{1}個字元")]
        [Display(Name = "姓名")]
        public string emp_name { get; set; }

        [Required]
        [Display(Name = "性別")]
        public string emp_gender { get; set; }

        [Required]
        [Display(Name = "生日")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public System.DateTime emp_birthday { get; set; }

        [Required]
        [Display(Name = "身分證字號")]
        public string emp_identity_num { get; set; }

        [Required]
        [Display(Name = "電話")]
        public string emp_tel { get; set; }

        [Required]
        [Display(Name = "手機")]
        public string emp_phone { get; set; }

        [Required]
        [Display(Name = "聯絡地址")]
        public string emp_address { get; set; }

        [Required]
        [Display(Name = "薪資")]
        public int salary { get; set; }

        [Display(Name = "帳號")]
        [System.Web.Mvc.Remote("CheckUniID", "ST_Emp", ErrorMessage = "此帳號已有人註冊")]
        public string account { get; set; }

        [Display(Name = "密碼")]
        public string password { get; set; }

        [Compare("password")]
        [ScaffoldColumn(false)]
        [Display(Name = "請再次輸入密碼")]
        public string password2 { get; set; }

        [Required]
        [Display(Name = "到職日")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public System.DateTime in_date { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        [Display(Name = "離職日")]
        public Nullable<System.DateTime> out_date { get; set; }

        [Display(Name = "備註")]
        public string remark { get; set; }

        public Nullable<System.DateTime> deleted_at { get; set; }
        public int del_yn { get; set; }




    }
}
