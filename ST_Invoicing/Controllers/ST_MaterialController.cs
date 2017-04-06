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
    public class ST_MaterialController : Controller
    {
        private ST_MaterialDAO mST_MaterialDAO = new ST_MaterialDAO();

        private ST_InStockDAO mST_InStockDAO = new ST_InStockDAO();

        private ST_VendorDAO mST_VendorDAO = new ST_VendorDAO();

        // GET: ST_Material
        public ActionResult Index()
        {
            string account = User.Identity.Name;

            List<ST_Material> material_List = mST_MaterialDAO.GetDataList_NotDel();

            foreach (ST_Material material in material_List)
            {
                ST_Vendor currVendor = mST_VendorDAO.FetchByGuid(material.vendor_guid);

                material.vendor_name = currVendor.vendor_name;
            }

            ViewData["user"] = Session["user"];

            return View(material_List);
        }

        // GET: ST_Material/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ST_Material data = mST_MaterialDAO.FetchBySerno(id);


            if (data == null)
            {
                return HttpNotFound();
            }

            ST_Vendor currVendor = mST_VendorDAO.FetchByGuid(data.vendor_guid);

            data.vendor_name = currVendor.vendor_name;

            ViewData["user"] = Session["user"];

            return View(data);
        }

        // GET: ST_Material/Create
        public ActionResult Create()
        {
            ViewData["vendor_Items"] = GetVendorItems();

            ViewData["unit_Items"] = GetUnitItem();

            ViewData["user"] = Session["user"];

            return View();
        }

        // POST: ST_Material/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ST_Material data)
        {
            if (ModelState.IsValid)
            {
                data.guid = Guid.NewGuid();

                ST_Vendor currVendor = mST_VendorDAO.FetchByName(data.vendor_name);

                data.vendor_guid = currVendor.guid;

                mST_MaterialDAO.Insert(data);

                /*不跟庫存連動*/
                //ST_InStock new_Stock = new ST_InStock(data.guid);

                //mST_InStockDAO.Insert(new_Stock);

                return RedirectToAction("Index");
            }

            return View(data);
        }

        // GET: ST_Material/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ST_Material data = mST_MaterialDAO.FetchBySerno(id);

            data.item_name = data.item_name.Trim();
            data.item_species = data.item_species.Trim();
            data.item_unit = data.item_unit.Trim();
            data.vendor_name = mST_VendorDAO.FetchByGuid(data.vendor_guid).vendor_name;

            ViewData["unit_Items"] = GetUnitItem();

            ViewData["vendor_Items"] = GetVendorItems();

            ViewData["user"] = Session["user"];

            if (data == null)
            {
                return HttpNotFound();
            }

            return View(data);
        }

        // POST: ST_Material/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ST_Material data)
        {
            if (ModelState.IsValid)
            {
                data.vendor_guid = mST_VendorDAO.FetchByName(data.vendor_name).guid;

                mST_MaterialDAO.Update(data);
                return RedirectToAction("Index");
            }
            return View(data);
        }

        // GET: ST_Material/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ST_Material data = mST_MaterialDAO.FetchBySerno(id);

            if (data == null)
            {
                return HttpNotFound();
            }

            ViewData["user"] = Session["user"];

            return View(data);
        }

        // POST: ST_Material/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ST_Material data = mST_MaterialDAO.FetchBySerno(id);

            mST_MaterialDAO.Soft_Delete(data);

            return RedirectToAction("Index");
        }

        public ActionResult CheckUniItem(ST_Material data)
        {
            if (data.item_name != null)
            {

                if (mST_MaterialDAO.IsUniItem(data) == true)
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);

            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                mST_MaterialDAO.Dispose();
            }
            base.Dispose(disposing);
        }

        private List<string> GetUnitItem()
        {
            List<string> unit_Items = new List<string>();

            unit_Items.Add("箱");
            unit_Items.Add("公斤");
            unit_Items.Add("臺斤");
            unit_Items.Add("克");
            unit_Items.Add("公升");
            unit_Items.Add("桶");
            unit_Items.Add("個");

            return unit_Items;
        }

        private List<string> GetVendorItems()
        {
            List<string> vendor_Items = new List<string>();

            List<ST_Vendor> vendors = new List<ST_Vendor>();

            vendors = mST_VendorDAO.GetDataList_NotDel();

            foreach (ST_Vendor vendor in vendors)
            {
                vendor_Items.Add(vendor.vendor_name);
            }

            return vendor_Items;
        }
    }
}
