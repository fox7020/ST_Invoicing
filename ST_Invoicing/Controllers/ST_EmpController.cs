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
    public class ST_EmpController : Controller
    {
        private STDATAEntities db = new STDATAEntities();

        // GET: ST_Emp
        public ActionResult Index()
        {
            return View(db.ST_Emp.ToList());
        }

        // GET: ST_Emp/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ST_Emp sT_Emp = db.ST_Emp.Find(id);
            if (sT_Emp == null)
            {
                return HttpNotFound();
            }
            return View(sT_Emp);
        }

        // GET: ST_Emp/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ST_Emp/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "serno,guid,emp_name,emp_gender,emp_birthday,emp_identity_num,emp_tel,emp_phone,emp_address,salary,id,password,in_date,out_date,remark,deleted_at,del_yn")] ST_Emp sT_Emp)
        {
            if (ModelState.IsValid)
            {
                db.ST_Emp.Add(sT_Emp);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sT_Emp);
        }

        // GET: ST_Emp/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ST_Emp sT_Emp = db.ST_Emp.Find(id);
            if (sT_Emp == null)
            {
                return HttpNotFound();
            }
            return View(sT_Emp);
        }

        // POST: ST_Emp/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "serno,guid,emp_name,emp_gender,emp_birthday,emp_identity_num,emp_tel,emp_phone,emp_address,salary,id,password,in_date,out_date,remark,deleted_at,del_yn")] ST_Emp sT_Emp)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sT_Emp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sT_Emp);
        }

        // GET: ST_Emp/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ST_Emp sT_Emp = db.ST_Emp.Find(id);
            if (sT_Emp == null)
            {
                return HttpNotFound();
            }
            return View(sT_Emp);
        }

        // POST: ST_Emp/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ST_Emp sT_Emp = db.ST_Emp.Find(id);
            db.ST_Emp.Remove(sT_Emp);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
