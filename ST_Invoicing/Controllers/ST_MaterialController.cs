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

        // GET: ST_Material
        public ActionResult Index()
        {
            string account = User.Identity.Name;
            return View(mST_MaterialDAO.GetDataList_NotDel());
        }

        // GET: ST_Material/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ST_Material sT_Material = mST_MaterialDAO.FetchBySerno(id);
            if (sT_Material == null)
            {
                return HttpNotFound();
            }
            return View(sT_Material);
        }

        // GET: ST_Material/Create
        public ActionResult Create()
        {

            ViewData["unit_Items"] = GetUnitItem();

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

                mST_MaterialDAO.Insert(data);

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
            data.utem_unit = data.utem_unit.Trim();

            ViewData["unit_Items"] = GetUnitItem();

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

            return unit_Items;
        }
    }
}
