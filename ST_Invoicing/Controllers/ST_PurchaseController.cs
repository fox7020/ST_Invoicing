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
    public class ST_PurchaseController : Controller
    {
        private ST_MaterialDAO mST_MaterialDAO = new ST_MaterialDAO();

        private ST_VendorDAO mST_VendorDAO = new ST_VendorDAO();

        private ST_PurcahseDAO mST_PurcahseDAO = new ST_PurcahseDAO();

        private ST_EmpDAO mST_EmpDAO = new ST_EmpDAO();

        // GET: ST_Purchase
        public ActionResult Index()
        {
            List<ST_Purchase> purchase_List = new List<ST_Purchase>();

            purchase_List = mST_PurcahseDAO.GetThisMonthData(DateTime.Today);

            SetOtherProperty(ref purchase_List);
        
            purchase_List = purchase_List.OrderBy(o => o.purchase_date).ToList();

            ViewData["user"] = Session["user"];
            
            return View(purchase_List);
        }

        [HttpPost]
        public ActionResult Index(string daterange)
        {
            DateTime start_Date = DateTime.Parse(daterange.Substring(0, 10));

            DateTime end_Date = DateTime.Parse(daterange.Substring(13, 10));

            List<ST_Purchase> rslt = mST_PurcahseDAO.GetDataByDateRange(start_Date, end_Date);

            SetOtherProperty(ref rslt);

            List<ST_Purchase> SortedList = rslt.OrderBy(o => o.purchase_date).ToList();

            ViewData["user"] = Session["user"];

            return View(SortedList);
        }

        // GET: ST_Purchase/Create
        public ActionResult Create()
        {

            ViewData["material_Items"] = GetMaterialItems();

            ViewData["vendor_Items"] = GetVendorItems();

            ViewData["user"] = Session["user"];

            return View();
        }

        // POST: ST_Purchase/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Purchase_DayAll data)
        {
            if (ModelState.IsValid && data.purchase_List != null)
            {
                List<ST_Purchase> prucahse_List = data.purchase_List.Cast<ST_Purchase>().ToList();

                foreach (ST_Purchase currPurchase in prucahse_List)
                {
                    currPurchase.guid = Guid.NewGuid();

                    currPurchase.purchase_date = data.rec_date;

                    currPurchase.vendor_guid = mST_VendorDAO.FetchByName(currPurchase.vendor_name).guid;

                    currPurchase.material_guid = mST_MaterialDAO.FetchByItemName(currPurchase.item_name).guid;

                    currPurchase.emp_guid = mST_EmpDAO.FetchByAccount(User.Identity.Name).guid;

                    mST_PurcahseDAO.Insert(currPurchase);
                }

                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "至少新增一筆採購項目");

            ViewData["material_Items"] = GetMaterialItems();

            ViewData["vendor_Items"] = GetVendorItems();

            ViewData["user"] = Session["user"];

            return View(data);
        }

        // GET: ST_Purchase/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ST_Purchase data = mST_PurcahseDAO.FetchBySerno(id);

            if (data == null)
            {
                return HttpNotFound();
            }

            data.special_mark = data.special_mark.Trim();

            data.item_name = mST_MaterialDAO.FetchByGuid(data.material_guid).item_name;

            data.vendor_name = mST_VendorDAO.FetchByGuid(data.vendor_guid).vendor_name;

            ViewData["material_Items"] = GetMaterialItems();

            ViewData["vendor_Items"] = GetVendorItems();

            ViewData["user"] = Session["user"];

            return View(data);
        }

        // POST: ST_Purchase/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ST_Purchase data)
        {
            if (ModelState.IsValid)
            {
                data.vendor_guid = mST_VendorDAO.FetchByName(data.vendor_name).guid;

                mST_PurcahseDAO.Update(data);

                return RedirectToAction("Index");
            }
            return View(data);
        }

        // GET: ST_Purchase/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ST_Purchase data = mST_PurcahseDAO.FetchBySerno(id);

            if (data == null)
            {
                return HttpNotFound();
            }

            data.special_mark = data.special_mark.Trim();

            data.item_name = mST_MaterialDAO.FetchByGuid(data.material_guid).item_name;

            data.vendor_name = mST_VendorDAO.FetchByGuid(data.vendor_guid).vendor_name;

            data.emp_name = mST_EmpDAO.FetchByGuid(data.emp_guid).emp_name;

            ViewData["user"] = Session["user"];

            return View(data);
        }

        // POST: ST_Purchase/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ST_Purchase data = mST_PurcahseDAO.FetchBySerno(id);

            if (data != null)
            {
                mST_PurcahseDAO.Delete(data);
            }


            return RedirectToAction("Index");
        }

        public ActionResult GetItemView()
        {

            ViewData["material_Items"] = GetMaterialItems();

            ViewData["vendor_Items"] = GetVendorItems();

            return PartialView("ItemView");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                mST_MaterialDAO.Dispose();
                mST_VendorDAO.Dispose();
                mST_PurcahseDAO.Dispose();
                mST_EmpDAO.Dispose();
            }
            base.Dispose(disposing);
        }

        private List<string> GetMaterialItems()
        {
            List<ST_Material> materials = mST_MaterialDAO.GetDataList_NotDel();

            List<string> material_Items = new List<string>();

            foreach (ST_Material material in materials)
            {
                material_Items.Add(material.item_name);
            }

            return material_Items;
        }

        private List<string> GetVendorItems()
        {
            List<ST_Vendor> vendors = mST_VendorDAO.GetDataList_NotDel();

            List<string> vendor_Items = new List<string>();

            foreach (ST_Vendor vendor in vendors)
            {
                vendor_Items.Add(vendor.vendor_name);
            }

            return vendor_Items;
        }

        private void SetOtherProperty(ref List<ST_Purchase> dataList)
        {
            foreach (ST_Purchase purchase in dataList)
            {
                purchase.item_name = mST_MaterialDAO.FetchByGuid(purchase.material_guid).item_name;

                purchase.vendor_name = mST_VendorDAO.FetchByGuid(purchase.vendor_guid).vendor_name;

                purchase.emp_name = mST_EmpDAO.FetchByGuid(purchase.emp_guid).emp_name;


                if (purchase.special_mark.Trim() == "是")
                {
                    purchase.font_Color = "red";
                }
                else
                {
                    purchase.font_Color = "black";
                }
            }
        }

    }
}
