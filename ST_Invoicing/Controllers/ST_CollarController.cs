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
        private STDATAEntities db = new STDATAEntities();

        private ST_CollarDAO mST_CollarDAO = new ST_CollarDAO();

        private ST_InStockDAO mST_InStockDAO = new ST_InStockDAO();

        private ST_MaterialDAO mST_MaterialDAO = new ST_MaterialDAO();

        private ST_EmpDAO mST_EmpDAO = new ST_EmpDAO();

        // GET: ST_Collar
        public ActionResult Index()
        {
            return View(db.ST_Collar.ToList());
        }

        // GET: ST_Collar/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ST_Collar sT_Collar = db.ST_Collar.Find(id);
            if (sT_Collar == null)
            {
                return HttpNotFound();
            }
            return View(sT_Collar);
        }

        // GET: ST_Collar/Create
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

        // POST: ST_Collar/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ST_Collar data)
        {
            //提領要先檢查庫存量
            if (ModelState.IsValid)
            {
                data.guid = Guid.NewGuid();

                ST_Emp currUser = mST_EmpDAO.FetchByAccount(User.Identity.Name);

                ST_Material currMaterial = mST_MaterialDAO.FetchByItemName(data.material_name);

                data.emp_guid = currUser.guid;

                data.material_guid = currMaterial.guid;

                mST_CollarDAO.Insert(data);

                mST_InStockDAO.DecreseStockCount(currMaterial.guid, data.collar_count);

                return RedirectToAction("Index");
            }

            return View(data);
        }

        // GET: ST_Collar/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ST_Collar sT_Collar = db.ST_Collar.Find(id);
            if (sT_Collar == null)
            {
                return HttpNotFound();
            }
            return View(sT_Collar);
        }

        // POST: ST_Collar/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "serno,guid,collar_date,material_guid,collar_count,remark,deleted_at,del_yn,emp_guid")] ST_Collar sT_Collar)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sT_Collar).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sT_Collar);
        }

        // GET: ST_Collar/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ST_Collar sT_Collar = db.ST_Collar.Find(id);
            if (sT_Collar == null)
            {
                return HttpNotFound();
            }
            return View(sT_Collar);
        }

        // POST: ST_Collar/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ST_Collar sT_Collar = db.ST_Collar.Find(id);
            db.ST_Collar.Remove(sT_Collar);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public JsonResult CheckStockCount(ST_Collar data)
        {
            if (!String.IsNullOrEmpty(data.material_name))
            {
                ST_Material currMaterial = mST_MaterialDAO.FetchByItemName(data.material_name);

                ST_InStock currStock = mST_InStockDAO.FetchByMaterialGuid(currMaterial.guid);

                if (currStock.count < data.collar_count)
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(false, JsonRequestBehavior.AllowGet);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();

                mST_CollarDAO.Dispose();

                mST_InStockDAO.Dispose();

                mST_MaterialDAO.Dispose();

                mST_EmpDAO.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
