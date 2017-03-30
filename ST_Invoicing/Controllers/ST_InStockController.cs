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
        private STDATAEntities db = new STDATAEntities();

        private ST_InStockDAO mST_InStockDAO = new ST_InStockDAO();

        private ST_MaterialDAO mST_MaterialDAO = new ST_MaterialDAO();

        // GET: ST_InStock
        public ActionResult Index()
        {
            return View(db.ST_InStock.ToList());
        }

        // GET: ST_InStock/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ST_InStock sT_InStock = db.ST_InStock.Find(id);
            if (sT_InStock == null)
            {
                return HttpNotFound();
            }
            return View(sT_InStock);
        }

        // GET: ST_InStock/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ST_InStock/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "serno,guid,material_guid,count,remark,deleted_at,del_yn")] ST_InStock sT_InStock)
        {
            if (ModelState.IsValid)
            {
                db.ST_InStock.Add(sT_InStock);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sT_InStock);
        }

        // GET: ST_InStock/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ST_InStock data = db.ST_InStock.Find(id);

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
                mST_InStockDAO.AddStockCount(data.material_guid, data.count);

                return RedirectToAction("Index");
            }
            return View(data);
        }

        // GET: ST_InStock/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ST_InStock sT_InStock = db.ST_InStock.Find(id);
            if (sT_InStock == null)
            {
                return HttpNotFound();
            }
            return View(sT_InStock);
        }

        // POST: ST_InStock/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ST_InStock sT_InStock = db.ST_InStock.Find(id);
            db.ST_InStock.Remove(sT_InStock);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                mST_InStockDAO.Dispose();
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
