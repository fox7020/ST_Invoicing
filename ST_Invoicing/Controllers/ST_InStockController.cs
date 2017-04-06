using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ST_Invoicing.Models;

namespace ST_Invoicing.Controllers
{
    [Authorize]
    public class ST_InStockController : Controller
    {
        private ST_InStockDAO mST_InStockDAO = new ST_InStockDAO();      
     
        // GET: ST_InStock
        public ActionResult Index()
        {
            List<ST_InStock> inStock_List = new List<ST_InStock>();

            inStock_List.Add(mST_InStockDAO.FetchLastDateData());

            ViewData["user"] = Session["user"];

            return View(inStock_List);
        }     

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                mST_InStockDAO.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
