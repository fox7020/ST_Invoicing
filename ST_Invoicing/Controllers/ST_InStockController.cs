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

        private ST_MaterialDAO mST_MaterialDAO = new ST_MaterialDAO();

        // GET: ST_InStock
        public ActionResult Index()
        {
            List<ST_InStock> inStock_List = mST_InStockDAO.GetDataList_NotDel();

            if (inStock_List.Count > 0)
            {
                ST_MaterialDAO mST_MaterialDAO = new ST_MaterialDAO();

                foreach (ST_InStock currInStock in inStock_List)
                {
                    ST_Material cuuMaterial = mST_MaterialDAO.FetchByGuid(currInStock.material_guid);

                    currInStock.material_name = cuuMaterial.item_name;
                }
            }

            return View(inStock_List);
        }

        // GET: ST_InStock/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ST_InStock data = mST_InStockDAO.FetchBySerno(id);

            data.material_name = mST_MaterialDAO.FetchByGuid(data.material_guid).item_name;

            if (data == null)
            {
                return HttpNotFound();
            }
            return View(data);
        }

        // POST: ST_InStock/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ST_InStock data)
        {
            if (ModelState.IsValid)
            {
                mST_InStockDAO.AddStockCount(data.material_guid, data.add_Count);
                return RedirectToAction("Index");
            }
            return View(data);
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
