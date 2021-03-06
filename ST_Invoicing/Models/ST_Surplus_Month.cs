
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

    public partial class ST_Surplus_Month
    {

        public int serno { get; set; }

        public System.Guid guid { get; set; }

        [Display(Name = "紀錄月份")]
        public System.DateTime rec_month { get; set; }

        [Display(Name = "營業天數")]
        public int work_days { get; set; }

        [Display(Name = "總支出")]
        public int expenditure_month { get; set; }

        [Display(Name = "總收入")]
        public int turnover_month { get; set; }

        [Display(Name = "總盈餘")]
        public int surplus_month { get; set; }

        [Display(Name = "飯桶數(月)")]
        public double rice_month { get; set; }

        [Display(Name = "羹桶數(月)")]
        public double soup_month { get; set; }

        [Display(Name = "魚箱數(月)")]
        public double fish_month { get; set; }

        [Display(Name = "早班700cc碗數(月)")]
        public int month_700_am { get; set; }

        [Display(Name = "晚班700cc碗數(月)")]
        public int month_700_pm { get; set; }

        [Display(Name = "早班850cc碗數(月)")]
        public int month_850_am { get; set; }

        [Display(Name = "晚班850cc碗數(月)")]
        public int month_850_pm { get; set; }

        [Display(Name = "早班點心盒(月)")]
        public int month_meal_am { get; set; }

        [Display(Name = "晚班點心盒(月)")]
        public int month_meal_pm { get; set; }

        [Display(Name = "早班便當盒數量(月)")]
        public int month_box_am { get; set; }

        [Display(Name = "晚班便當盒數量(月)")]
        public int month_box_pm { get; set; }

        [Display(Name = "早班內用大大碗數量(月)")]
        public int month_inner_xl_am { get; set; }

        [Display(Name = "晚班內用大大碗數量(月)")]
        public int month_inner_xl_pm { get; set; }

        [Display(Name = "早班內用大碗數量(月)")]
        public int month_inner_l_am { get; set; }

        [Display(Name = "晚班內用大碗數量(月)")]
        public int month_inner_l_pm { get; set; }

        [Display(Name = "早班內用小碗數量(月)")]
        public int month_inner_s_am { get; set; }

        [Display(Name = "晚班內用小碗數量(月)")]
        public int month_inner_s_pm { get; set; }

        [Display(Name = "食材支出(月)")]
        public int food_month { get; set; }

        [Display(Name = "耗材支出(月)")]
        public int supplie_month { get; set; }

        [Display(Name = "其他支出(月)")]
        public int other_month { get; set; }

        public Nullable<int> personal_month { get; set; }

        [Display(Name = "備註")]
        public string remark { get; set; }

        public Nullable<System.DateTime> deleted_at { get; set; }

        public int del_yn { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "紀錄月份")]
        public string displayMonth { get { return rec_month.Year + "年" + rec_month.Month + "月"; } }

    }

}
