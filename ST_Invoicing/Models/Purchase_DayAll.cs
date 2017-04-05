using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace ST_Invoicing.Models
{
    public class Purchase_DayAll
    {
        [Required]
        [Display(Name = "採購日期")]
        public DateTime rec_date { get; set; }

      
        [ScaffoldColumn(false)]
        public virtual ICollection<ST_Purchase> purchase_List { get; set; }
    }
}