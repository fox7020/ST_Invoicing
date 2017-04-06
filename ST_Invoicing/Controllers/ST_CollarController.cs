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
    public class ST_CollarController : Controller
    {     
        private ST_CollarDAO mST_CollarDAO = new ST_CollarDAO();

        private ST_InStockDAO mST_InStockDAO = new ST_InStockDAO();

        private ST_MaterialDAO mST_MaterialDAO = new ST_MaterialDAO();

        private ST_EmpDAO mST_EmpDAO = new ST_EmpDAO();

        public ActionResult Index()
        {
            List<ST_Collar> dataList = mST_CollarDAO.GetDataList_NotDel();

            foreach (ST_Collar currCollar in dataList)
            {
                ST_Material currMaterial = mST_MaterialDAO.FetchByGuid(currCollar.material_guid);

                currCollar.material_name = currMaterial.item_name;

                ST_Emp currEmp = mST_EmpDAO.FetchByGuid(currCollar.emp_guid);

                currCollar.emp_name = currEmp.emp_name;
            }

            return View(dataList);
        }
      
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ST_Collar data = mST_CollarDAO.FetchBySerno(id);

            ST_Material currMaterial = mST_MaterialDAO.FetchByGuid(data.material_guid);

            data.material_name = currMaterial.item_name;

            ST_Emp currEmp = mST_EmpDAO.FetchByGuid(data.emp_guid);

            data.emp_name = currEmp.emp_name;

            if (data == null)
            {
                return HttpNotFound();
            }
            return View(data);
        }
      
        public ActionResult Create()
        {
            List<ST_Material> materials = mST_MaterialDAO.GetDataList_NotDel();

            List<string> items = new List<string>();

            foreach (ST_Material material in materials)
            {
                items.Add(material.item_name);
            }

            ViewData["material_items"] = items;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ST_Collar data)
        {       
            if (ModelState.IsValid)
            {
                data.guid = Guid.NewGuid();

                ST_Emp currUser = mST_EmpDAO.FetchByAccount(User.Identity.Name);

                ST_Material currMaterial = mST_MaterialDAO.FetchByItemName(data.material_name);

                data.emp_guid = currUser.guid;

                data.material_guid = currMaterial.guid;

                mST_CollarDAO.Insert(data);

                //mST_InStockDAO.DecreseStockCount(currMaterial.guid, data.collar_count);

                return RedirectToAction("Index");
            }

            return View(data);
        }
 

            
        //public JsonResult CheckStockCount(ST_Collar data)
        //{
        //    if (!String.IsNullOrEmpty(data.material_name))
        //    {
        //        ST_Material currMaterial = mST_MaterialDAO.FetchByItemName(data.material_name);

        //        ST_InStock currStock = mST_InStockDAO.FetchByMaterialGuid(currMaterial.guid);

        //        if (currStock.count < data.collar_count)
        //        {
        //            return Json(false, JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return Json(true, JsonRequestBehavior.AllowGet);
        //        }
        //    }

        //    return Json(false, JsonRequestBehavior.AllowGet);
        //}
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {          
                mST_CollarDAO.Dispose();

                mST_InStockDAO.Dispose();

                mST_MaterialDAO.Dispose();

                mST_EmpDAO.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
