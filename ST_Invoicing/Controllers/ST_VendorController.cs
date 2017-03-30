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
    public class ST_VendorController : Controller
    {

        private ST_VendorDAO mST_VendorDAO = new ST_VendorDAO();

        // GET: ST_Vendor
        public ActionResult Index()
        {
            return View(mST_VendorDAO.GetDataList_NotDel());
        }

        // GET: ST_Vendor/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ST_Vendor sT_Vendor = mST_VendorDAO.FetchBySerno(id);
            if (sT_Vendor == null)
            {
                return HttpNotFound();
            }
            return View(sT_Vendor);
        }

        // GET: ST_Vendor/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ST_Vendor/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ST_Vendor data)
        {
            if (ModelState.IsValid)
            {
                data.guid = Guid.NewGuid();

                mST_VendorDAO.Insert(data);

                return RedirectToAction("Index");
            }

            return View(data);
        }

        // GET: ST_Vendor/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ST_Vendor data = mST_VendorDAO.FetchBySerno(id);

            CleanData(ref data);

            if (data == null)
            {
                return HttpNotFound();
            }
            return View(data);
        }

        // POST: ST_Vendor/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ST_Vendor data)
        {
            if (ModelState.IsValid)
            {
                mST_VendorDAO.Update(data);

                return RedirectToAction("Index");
            }
            return View(data);
        }

        // GET: ST_Vendor/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ST_Vendor data = mST_VendorDAO.FetchBySerno(id);      

            if (data == null)
            {
                return HttpNotFound();
            }
            return View(data);
        }

        // POST: ST_Vendor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ST_Vendor data = mST_VendorDAO.FetchBySerno(id);

            CleanData(ref data);

            mST_VendorDAO.Soft_Delete(data);

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                mST_VendorDAO.Dispose();
            }
            base.Dispose(disposing);
        }

        public JsonResult CheckUniFormNum(ST_Vendor data)
        {
            if (data.uniform_num != null)
            {
                if (mST_VendorDAO.IsUniform_Num(data) == true)
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
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        private void CleanData(ref ST_Vendor data)
        {
            data.vendor_name = data.vendor_name.Trim();
            data.contact_person = data.contact_person.Trim();
            data.vendor_tel1 = data.vendor_tel1.Trim();

            if (data.vendor_tel2 != null)
            {
                data.vendor_tel2 = data.vendor_tel2.Trim();
            }

            data.address = data.address.Trim();

            if (data.uniform_num != null)
            {
                data.uniform_num = data.uniform_num.Trim();
            }

            data.payment_mathod = data.payment_mathod.Trim();

            if (data.remark != null)
            {
                data.remark = data.remark.Trim();
            }
        }
    }
}
