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
    
    public partial class ST_Material
    {
        public int serno { get; set; }
        public System.Guid guid { get; set; }
        public string item_name { get; set; }
        public string utem_unit { get; set; }
        public string item_species { get; set; }
        public string reamrk { get; set; }
        public Nullable<System.DateTime> deleted_at { get; set; }
        public int del_yn { get; set; }
    }
}