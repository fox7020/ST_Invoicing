﻿

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
using System.Data.Entity;
using System.Data.Entity.Infrastructure;


public partial class STDATAEntities : DbContext
{
    public STDATAEntities()
        : base("name=STDATAEntities")
    {

    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        throw new UnintentionalCodeFirstException();
    }


    public virtual DbSet<ST_Collar> ST_Collar { get; set; }

    public virtual DbSet<ST_Emp> ST_Emp { get; set; }

    public virtual DbSet<ST_InStock> ST_InStock { get; set; }

    public virtual DbSet<ST_Material> ST_Material { get; set; }

    public virtual DbSet<ST_Vendor> ST_Vendor { get; set; }

    public virtual DbSet<ST_Personal_Cost> ST_Personal_Cost { get; set; }

    public virtual DbSet<ST_Purchase> ST_Purchase { get; set; }

    public virtual DbSet<ST_SurplusDay> ST_SurplusDay { get; set; }

    public virtual DbSet<ST_Surplus_Month> ST_Surplus_Month { get; set; }

}

}

