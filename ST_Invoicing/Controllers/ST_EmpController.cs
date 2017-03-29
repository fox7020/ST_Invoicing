using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ST_Invoicing.Models;
using System.Data.Entity.Infrastructure;

namespace ST_Invoicing.Controllers
{
    public class ST_EmpController : Controller
    {
        //private STDATAEntities db = new STDATAEntities();

        private ST_EmpDAO mST_EmpDAO = new ST_EmpDAO();

        // GET: ST_Emp
        public ActionResult Index()
        {        
            return View(mST_EmpDAO.GetDataList_NotDel());
        }

        // GET: ST_Emp/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ST_Emp sT_Emp = mST_EmpDAO.FetchBySerno(id);
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
        public ActionResult Create(ST_Emp data)
        {
            if (ModelState.IsValid)
            {
                data.guid = Guid.NewGuid();

                mST_EmpDAO.Insert(data);
             
                return RedirectToAction("Index");
            }

            return View(data);
        }

        // GET: ST_Emp/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ST_Emp sT_Emp = mST_EmpDAO.FetchBySerno(id);
            if (sT_Emp == null)
            {
                return HttpNotFound();
            }

            sT_Emp.emp_name = sT_Emp.emp_name.Trim();
            sT_Emp.emp_gender = sT_Emp.emp_gender.Trim();
            sT_Emp.emp_identity_num = sT_Emp.emp_identity_num.Trim();
            sT_Emp.emp_tel = sT_Emp.emp_tel.Trim();
            sT_Emp.emp_phone = sT_Emp.emp_phone.Trim();
            sT_Emp.emp_address = sT_Emp.emp_address.Trim();
            sT_Emp.account = sT_Emp.account.Trim();
            sT_Emp.password = sT_Emp.password.Trim();
            sT_Emp.password2 = sT_Emp.password.Trim();
            return View(sT_Emp);
        }

        // POST: ST_Emp/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ST_Emp sT_Emp)
        {

            if (ModelState.IsValid)
            {             
                mST_EmpDAO.Update(sT_Emp);

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
            ST_Emp sT_Emp = mST_EmpDAO.FetchBySerno(id);
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
            ST_Emp data = mST_EmpDAO.FetchBySerno(id);
           
            mST_EmpDAO.Soft_Delete(data);

            return RedirectToAction("Index");
        }

        public ActionResult CheckUniID(ST_Emp data)
        {

            if (data.account != null)
            {
                if (data.serno != 0)
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }

                if (mST_EmpDAO.IsUniID(data.account) == true)
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
                mST_EmpDAO.Dispose();
            }
            base.Dispose(disposing);
        }      
    }
}
